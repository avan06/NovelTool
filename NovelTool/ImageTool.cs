using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NovelTool
{
    public static class ImageTool
    {
        public static bool AnalysisPointStates(in Bitmap bmp, in PageData pageData, bool forcedAnalysis=false)
        {
            GraphicsUnit units = GraphicsUnit.Pixel;
            pageData.rectImg = NewEntity(bmp.GetBounds(ref units), RectType.None); //RectangleF rectImg = bmp.GetBounds(ref units);
            BitmapTool bmpTool = new BitmapTool(bmp, true, true);
            HashSet<int> colorDict = new HashSet<int>();
            Dictionary<(int, int), int> pStates = new Dictionary<(int, int), int>();
            HashSet<int> xStateSet = new HashSet<int>();
            HashSet<int> yStateSet = new HashSet<int>();

            int IllustrationMinColorsLevel = Properties.Settings.Default.IllustrationMinColorsLevel.Value; //判定為插圖的色彩數量
            int IllustrationMinNonWhiteLevel = Properties.Settings.Default.IllustrationMinNonWhiteLevel.Value; //判定為插圖的深色數量
            byte ConfirmWhiteLevel = Properties.Settings.Default.ConfirmWhiteLevel.Value; //指定白色數值
            int IgnoreMinDetectXSize = Properties.Settings.Default.IgnoreMinDetectXSize.Value;
            int IgnoreMinDetectYSize = Properties.Settings.Default.IgnoreMinDetectYSize.Value;

            for (int y = 0; y < bmpTool.Height; y++)
            {
                if (y < IgnoreMinDetectYSize || y > bmpTool.Height - IgnoreMinDetectYSize) continue;
                for (int x = 0; x < bmpTool.Width; x++)
                {
                    if (x < IgnoreMinDetectXSize || x > bmpTool.Width - IgnoreMinDetectXSize) continue;
                    int argb = bmpTool.GetPixel(x, y);
                    bool isBlack = IsBlack(argb, out byte gray, ConfirmWhiteLevel);
                    if (isBlack) pStates.Add((x, y), argb);
                    if (forcedAnalysis) continue;
                    if (!isBlack && !colorDict.Contains(argb)) colorDict.Add(argb);
                    if (colorDict.Count > IllustrationMinColorsLevel || pStates.Count > IllustrationMinNonWhiteLevel) //2000色以上視為插圖 or 前景數量過多判定為插圖
                    {
                        pageData.isIllustration = true;
                        return false;
                    }
                }
            }

            foreach (KeyValuePair<(int X, int Y), int> item in pStates)
            {
                if (!xStateSet.Contains(item.Key.X)) xStateSet.Add(item.Key.X);
                if (!yStateSet.Contains(item.Key.Y)) yStateSet.Add(item.Key.Y);
            }
            if (!forcedAnalysis && bmpTool.Width - xStateSet.Count < 50 && bmpTool.Height - yStateSet.Count < 50)
            {
                pageData.isIllustration = true;
                pageData.pStates = pStates;
                return false;
            }
            List<int> xStateList = new List<int>(xStateSet);
            List<int> yStateList = new List<int>(yStateSet);
            xStateList.Sort();
            yStateList.Sort();

            pageData.pStates = pStates;
            pageData.xStates = xStateList;
            pageData.yStates = yStateList;

            return true;
        }
        public static bool IsBlack(int argb, out byte gray, byte white = 240)
        {
            gray = ParseGray(argb);
            return gray < white;
        }
        /// <summary>
        /// https://blog.csdn.net/dannylsl/article/details/6261527
        /// https://stackoverflow.com/a/2693008
        /// </summary>
        public static byte ParseGray(int argb, bool is16Precision = false)
        {
            uint gray;
            if (argb >> 24 == 0) gray = 255;
            else if (is16Precision) gray = ((uint)(byte)(argb >> 16) * 19595 + (uint)(byte)(argb >> 8) * 38469 + (uint)(byte)(argb) * 7472) >> 16;
            else gray = ((uint)(byte)(argb >> 16) * 38 + (uint)(byte)(argb >> 8) * 75 + (uint)(byte)(argb) * 15) >> 7;

            return (byte)(gray);
        }
        /// <summary>
        /// 初始分析文字頁面圖，分切為三塊，標頭、身體、頁尾
        /// </summary>
        public static void AnalysisPageY(in PageData pageData)
        {
            int prevY = 0;
            (RectType RType, float X, float Y, float Width, float Height) bounds = pageData.rectImg, rect = NewEntity(), rectHead = NewEntity(), rectBody = NewEntity(), rectFooter = NewEntity();
            Dictionary<(int, int), int> pStates = pageData.pStates;
            List<int> xStates = pageData.xStates, yStates = pageData.yStates;
            float HeadMinRate = Properties.Settings.Default.HeadMinRate.Value;
            float FooterMinRate = Properties.Settings.Default.FooterMinRate.Value;
            int ConfirmHeadGap = Properties.Settings.Default.ConfirmHeadGap.Value;
            int ConfirmFooterGap = Properties.Settings.Default.ConfirmFooterGap.Value;
            for (int idx = 0; idx < yStates.Count; idx++)
            {
                int y = yStates[idx];
                if (rect.Y == 0)
                {
                    rect.X = bounds.X;
                    rect.Width = bounds.Width;
                    if (prevY == 0) rect.Y = yStates[0]; //當前一筆為0 時，則帶入Y軸最小前景值
                    else rect.Y = prevY;
                }
                if (prevY == 0 || prevY + 1 == y)
                { // 前後筆 Y 軸為連續時，記錄其範圍
                    if (rect.Y + rect.Height <= y) rect.Height = y + 1 - rect.Y;
                }
                if ((prevY > 0 && prevY + 1 != y) || idx == yStates.Count - 1)
                { //當 Y 軸位置內無前景區域、或為最後一列，則將資料寫入 RectangleArray
                    float minPosition = (rect.Y - bounds.Y) / bounds.Height;
                    float maxPosition = (rect.Y + rect.Height - bounds.Y) / bounds.Height;
                    RectType rectType = RectType.Body;
                    if (minPosition < HeadMinRate && maxPosition < HeadMinRate)
                    { //由 Rect 最小與最大兩個座標Y軸，在整張圖形中的位置比例決定資料類型
                        if (rectHead.RType == RectType.None) rectType = RectType.Head;
                        else if (rect.Y - rectHead.Y - rectHead.Height > 30) { } //當空白區域大於30，避免將Body誤判為Head
                    }
                    else if (minPosition > FooterMinRate && maxPosition > FooterMinRate)
                    {
                        if (rectFooter.RType == RectType.None) rectType = RectType.Footer;
                        else if (rectFooter.Y + rectFooter.Height < rect.Y + rect.Height && rect.Height > 5)
                        { //偵測到的rect Y軸比之前的rectFooter位置還低時，代表rectFooter之值是並非Footer，判斷是否合併至rectBody內
                            if (rectFooter.Height > 5 && rectFooter.Y - rectBody.Y - rectBody.Height < 50) rectBody = UnionEntity(rectBody, rectFooter); //當rectFooter Y軸很小，且與rectBody之間空白大於50時，則忽略該內容
                            rectFooter = NewEntity();
                        }
                        else rectType = RectType.None;
                    }
                    if (rect.Y == 0) continue;

                    if (idx == yStates.Count - 1 && rectBody.RType != RectType.None && rectFooter.RType == RectType.None) rectType = RectType.Footer;

                    rect.RType = rectType;
                    if (rectType == RectType.Head) rectHead = rectHead.RType == RectType.None ? rect : UnionEntity(rectHead, rect);
                    else if (rectType == RectType.Body && rectBody.RType == RectType.None) rectBody = rect; //rectBody = rectBody.RType == RectType.None ? rect : UnionEntity(rectBody, rect);
                    else if (rectType == RectType.Body && rectBody.RType != RectType.None)
                    {
                        if (rect.Height > 3 && rect.Y - rectBody.Y - rectBody.Height < 50) rectBody = UnionEntity(rectBody, rect); //當rect Y軸很小，且與rectBody之間空白大於50時，則忽略該內容
                    }
                    else if (rectType == RectType.Footer) rectFooter = rectFooter.RType == RectType.None ? rect : UnionEntity(rectFooter, rect);
                    rect = NewEntity();
                }
                prevY = y;
            }

            if (rectHead.RType != RectType.None && rectBody.RType != RectType.None && rectBody.Y - rectHead.Y - rectHead.Height < ConfirmHeadGap)
            { //當Head 與 Body 之間寬度過小，代表誤偵測
                rectBody = UnionEntity(rectBody, rectHead);
                rectHead = NewEntity();
            }
            if (rectFooter.RType != RectType.None && rectBody.RType != RectType.None && rectFooter.Y - rectBody.Y - rectBody.Height < ConfirmFooterGap)
            { //當Body 與 Footer 之間寬度過小，代表誤偵測
                rectBody = UnionEntity(rectBody, rectFooter);
                rectFooter = NewEntity(); //RectangleF.Empty;
            }
            if (rectBody.Y - bounds.Y >= bounds.Height * 0.15) pageData.isIllustration = true; //前景頂端由上往下介於整頁 15%以內才繼續分析內容，否則視為插圖

            if (rectHead.RType == RectType.None && rectBody.RType == RectType.None && rectFooter.RType == RectType.None) return;

            HashSet<int> xStatesHead = new HashSet<int>();
            HashSet<int> xStatesBody = new HashSet<int>();
            HashSet<int> xStatesFooter = new HashSet<int>();

            for (int iX = 0; iX < xStates.Count; iX++)
            {
                int X = xStates[iX];
                if ((X < rectHead.X && X < rectBody.X && X < rectFooter.X) || (X > rectHead.X + rectHead.Width && X > rectBody.X + rectBody.Width && X > rectFooter.X + rectFooter.Width)) continue;
                bool isHitHead = false, isHitBody = false, isHitFooter = false;
                for (int iY = 0; iY < yStates.Count; iY++)
                {
                    int Y = yStates[iY];
                    if (!isHitHead && Y >= rectHead.Y && Y <= rectHead.Y + rectHead.Height && X >= rectHead.X && X <= rectHead.X + rectHead.Width && pStates.ContainsKey((X, Y)))
                    {
                        xStatesHead.Add(X);
                        isHitHead = true;
                    }
                    else if (!isHitBody && Y >= rectBody.Y && Y <= rectBody.Y + rectBody.Height && X >= rectBody.X && X <= rectBody.X + rectBody.Width && pStates.ContainsKey((X, Y)))
                    {
                        xStatesBody.Add(X);
                        isHitBody = true;
                    }
                    else if (!isHitFooter && Y >= rectFooter.Y && Y <= rectFooter.Y + rectFooter.Height && X >= rectFooter.X && X <= rectFooter.X + rectFooter.Width && pStates.ContainsKey((X, Y)))
                    {
                        xStatesFooter.Add(X);
                        isHitFooter = true;
                    }
                    if (isHitHead && isHitBody && isHitFooter) break;
                }

            }
            if (xStatesHead.Count > 0)
            {
                List<int> xStatesHeadList = new List<int>(xStatesHead);
                xStatesHeadList.Sort();
                pageData.xStatesHead = xStatesHeadList;
                rectHead.X = xStatesHeadList[0];
                rectHead.Width = xStatesHeadList[xStatesHeadList.Count - 1] + 1 - rectHead.X;
            }
            if (xStatesBody.Count > 0)
            {
                List<int> xStatesBodyList = new List<int>(xStatesBody);
                xStatesBodyList.Sort();
                pageData.xStatesBody = xStatesBodyList;
                rectBody.X = xStatesBodyList[0];
                rectBody.Width = xStatesBodyList[xStatesBodyList.Count - 1] + 1 - rectBody.X;
            }
            if (xStatesFooter.Count > 0)
            {
                List<int> xStatesFooterList = new List<int>(xStatesFooter);
                xStatesFooterList.Sort();
                pageData.xStatesFooter = xStatesFooterList;
                rectFooter.X = xStatesFooterList[0];
                rectFooter.Width = xStatesFooterList[xStatesFooterList.Count - 1] + 1 - rectFooter.X;
            }
            pageData.rectHead = rectHead;
            pageData.rectBody = rectBody;
            pageData.rectFooter = rectFooter;
        }
        /// <summary>
        /// 分析Body區域，將文字頁面圖依行分離成各自Column區域
        /// </summary>
        public static void AnalysisPageX((RectType RType, float X, float Y, float Width, float Height) rectRegion, List<int> xStates, 
            out List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnRects)
        {
            columnRects = new List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)>();
            if (rectRegion.RType == RectType.None) return;

            var columnRect = NewEntity();
            int prevX = 0;
            for (int iX = 0; iX < xStates.Count; iX++)
            {
                int X = xStates[iX];
                if (X < rectRegion.X || X > rectRegion.X + rectRegion.Width) continue;
                if (columnRect.RType == RectType.None)
                { //紀錄起始 X 軸位置
                    columnRect.RType = RectType.EntityBody;
                    columnRect.X = X == rectRegion.X ? X : X - 1;
                    columnRect.Y = rectRegion.Y;
                    columnRect.Height = rectRegion.Height;
                    prevX = X;
                    continue;
                }
                if (X != prevX + 1 || X == rectRegion.X + rectRegion.Width - 1 || iX == xStates.Count - 1)
                { //取出單行文字範圍（假如 X 軸前景非連續，跨越空白位置時）
                    columnRect.Width = prevX + 1 - columnRect.X;
                    if (X == rectRegion.X + rectRegion.Width - 1) columnRect.Width++;
                    if (columnRect.Width > 3) columnRects.Add((RectType.Body, columnRect.X, columnRect.Y, columnRect.Width, columnRect.Height, null)); //當該行寬度過小，則忽略掉，可能為圖片躁點誤判
                    columnRect = NewEntity();
                }
                prevX = X;
            }
        }
        /// <summary>
        /// 分析 ColumnRects 將每行文字分離，並累計文字範圍上下左右位置與實體寬高，之後會用來確認眾數(Mode)
        /// </summary>
        public static void AnalysisColumnRects(Dictionary<(int X, int Y), int> pStates, (RectType RType, float X, float Y, float Width, float Height) rectRegion,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnRects,
            in (ConcurrentDictionary<float, int> TopDict, ConcurrentDictionary<float, int> BottomDict,
                ConcurrentDictionary<float, int> LeftDict, ConcurrentDictionary<float, int> RightDict,
                ConcurrentDictionary<float, int> WidthDict, ConcurrentDictionary<float, int> HeightDict) counts)
        {
            counts.TopDict.AddOrUpdate(rectRegion.Y, 1, (k, v) => v + 1);
            counts.BottomDict.AddOrUpdate(rectRegion.Y + rectRegion.Height, 1, (k, v) => v + 1);
            counts.LeftDict.AddOrUpdate(rectRegion.X, 1, (k, v) => v + 1);
            counts.RightDict.AddOrUpdate(rectRegion.X + rectRegion.Width, 1, (k, v) => v + 1);
            for (int idx = 0; idx < columnRects.Count; idx++)
            {
                var columnRect = columnRects[idx];
                float entityY = 0;
                counts.WidthDict.AddOrUpdate(columnRect.Width, 1, (k, v) => v + 1);
                for (float Y = columnRect.Y; Y <= columnRect.Y + columnRect.Height; Y++)
                {
                    for (float X = columnRect.X; X <= columnRect.X + columnRect.Width; X++)
                    {
                        if (pStates.ContainsKey(((int)X, (int)Y)))
                        {
                            if (entityY == 0) entityY = Y;
                            break; //有值代表還是同一字
                        }
                        if (entityY == 0 || X < columnRect.X + columnRect.Width) continue; //還在空白處或還未到最右側不處理
                        if (columnRect.Entitys == null) columnRect.Entitys = new List<(RectType RType, float X, float Y, float Width, float Height)>();
                        float entityHeight = Y - entityY;
                        var entityPrev = columnRect.Entitys.Count > 1 ? columnRect.Entitys[columnRect.Entitys.Count -2] : (RectType.None, 0, 0, 0, 0);

                        if (Y - entityPrev.Y - entityPrev.Height < 50 || entityHeight > 2)
                        {
                            counts.HeightDict.AddOrUpdate(entityHeight, 1, (k, v) => v + 1);
                            columnRect.Entitys.Add((RectType.EntityBody, columnRect.X, entityY, columnRect.Width, entityHeight));
                        }
                        entityY = 0;
                    }
                }
                if (columnRect.Entitys != null && columnRect.Entitys.Count > 0)
                { //依據實際文字內容，調整列 Y軸範圍
                    var lastEntity = columnRect.Entitys[columnRect.Entitys.Count - 1];
                    columnRect.Y = columnRect.Entitys[0].Y;
                    columnRect.Height = lastEntity.Y + lastEntity.Height - columnRect.Y;
                }
                columnRects[idx] = columnRect;
            }
        }

        public static void AnalysisEntityHeighWidth(Dictionary<(int X, int Y), int> pStates,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnRects,
            (float Top, float Bottom, float Left, float Right, float Width, float Heigh, float TopMin, float BottomMin, float LeftMin, float RightMin,
                float WidthMin, float WidthMax, float HeighMin, float HeighMax) modes)
        {
            float EntityMergeTBMaxRate = Properties.Settings.Default.EntityMergeTBMaxRate.Value;
            for (int idx = 0; idx < columnRects.Count; idx++)
            {
                var columnRect = columnRects[idx];
                var entitys = columnRect.Entitys;

                if (entitys == null || entitys.Count == 0) continue;

                if (columnRect.Width < modes.WidthMin)
                { //欄位寬度小於最小實體寬度
                    bool isUnion = false;
                    var columnRectPrev = idx - 1 >= 0 ? columnRects[idx - 1] : NewEntitys(); //  【> 0】 =>  【>= 0】
                    var columnRectNext = idx + 1 < columnRects.Count ? columnRects[idx + 1] : NewEntitys();
                    float colSpace = columnRect.X - columnRectPrev.X - columnRectPrev.Width;
                    if (colSpace > 4 && columnRectNext.Entitys != null && columnRectNext.Entitys.Count > 0 && columnRectNext.X > columnRect.X)
                    { //與前一列分離，且存在下一列
                        float newWidth = columnRectNext.X + columnRectNext.Width - columnRect.X;
                        if (newWidth < modes.WidthMax)
                        { //前後兩列寬度小於最大實體寬度，則判斷合併
                            isUnion = true;
                            columnRect = UnionEntitys(columnRect, columnRectNext);
                            columnRects.RemoveAt(idx + 1);
                        }
                    }
                    if (!isUnion)
                    {
                        if (columnRectPrev.RType != RectType.None && colSpace < 4)
                        { //與前一列相鄰，判斷為Ruby
                            if (columnRectPrev.RType == RectType.Ruby && columnRect.X + columnRect.Width - columnRectPrev.X < modes.WidthMin)
                            { //前一列也是Ruby，且兩者寬度小於最小實體寬度，則將之合併成為一個Ruby，也許是【い】之類的Ruby被分成兩列
                                columnRect = UnionEntitys(columnRectPrev, columnRect, RectType.Ruby);
                                columnRects.RemoveAt(idx);
                                idx--;
                            }
                            else
                            {
                                columnRect.RType = RectType.Ruby;
                                for (int eIdx = 0; eIdx < entitys.Count; eIdx++)
                                {
                                    var entity = entitys[eIdx];
                                    entity.RType = RectType.Ruby;
                                    entitys[eIdx] = entity;
                                }
                            }
                        }

                    }

                    entitys = columnRect.Entitys;
                    columnRects[idx] = columnRect;
                }
                if (columnRect.Width > modes.WidthMax)
                { //欄位寬度大於眾數寬度時
                }

                var columnRuby = new List<(RectType RType, float X, float Y, float Width, float Height)>();
                for (int eIdx = 0; eIdx < entitys.Count; eIdx++)
                {
                    bool isSeparate = false;
                    var entity = entitys[eIdx];
                    if (entity.Width > modes.WidthMax) //寬度大於眾數寬度時，判斷是否要分離左右部分
                    {
                        var ruby = NewEntity();
                        for (float yIdx = entity.Y; yIdx <= entity.Y + entity.Height; yIdx++)
                        {
                            for (float xIdx = entity.X + modes.Width + (float)Math.Round(modes.Width * 0.08); xIdx <= entity.X + entity.Width; xIdx++)
                            {
                                if (pStates.ContainsKey(((int)xIdx, (int)yIdx)))
                                {
                                    if (ruby.RType == RectType.None)
                                    {
                                        ruby.RType = RectType.Ruby;
                                        ruby.X = entity.X + modes.Width;
                                        ruby.Y = yIdx - 1;
                                        ruby.Width = entity.Width - modes.Width;
                                    }
                                    break;
                                }
                                if (xIdx == entity.X + entity.Width && ruby.RType == RectType.Ruby)
                                { //位於實體X軸的最右側，代表該處為空白，紀錄Ruby 高度
                                    isSeparate = true;
                                    ruby.Height = yIdx - ruby.Y + 1;
                                    columnRuby.Add(ruby);
                                    ruby = NewEntity();
                                }
                            }
                        }
                    }
                    if (isSeparate && entity.Height > modes.HeighMax)
                    {
                        var newIdx = eIdx;
                        var newEntity = NewEntity();
                        for (float yIdx = entity.Y; yIdx <= entity.Y + entity.Height; yIdx++)
                        {
                            for (float xIdx = entity.X; xIdx <= entity.X + modes.Width - (float)Math.Round(modes.Width * 0.08); xIdx++)
                            {
                                if (pStates.ContainsKey(((int)xIdx, (int)yIdx)))
                                {
                                    if (newEntity.RType == RectType.None)
                                    {
                                        if (newIdx == eIdx) newEntity.RType = RectType.SplitTop;
                                        else newEntity.RType = RectType.SplitMiddle;
                                        newEntity.X = entity.X;
                                        newEntity.Y = yIdx;
                                        newEntity.Width = modes.Width;
                                    }
                                    break;
                                }
                                if (xIdx == entity.X + modes.Width - (float)Math.Round(modes.Width * 0.08) && (newEntity.RType == RectType.SplitTop || newEntity.RType == RectType.SplitMiddle))
                                { //位於實體X軸的最右側，代表該處為空白，紀錄 Entity高度
                                    newEntity.Height = yIdx - newEntity.Y;
                                    entitys.Insert(newIdx++, newEntity);
                                    newEntity = NewEntity();
                                }
                            }
                        }
                        if (newIdx > eIdx)
                        {
                            if (newEntity.RType != RectType.None)
                            {
                                newEntity.RType = RectType.SplitBottom;
                                newEntity.Height = entity.Y + entity.Height - newEntity.Y;
                                entitys[newIdx] = newEntity;
                                eIdx = newIdx;
                            }
                            else
                            {
                                entitys.RemoveAt(newIdx);
                                eIdx = newIdx - 1;
                            }
                        }
                        continue;
                    }

                    if (entity.Height > modes.Heigh || eIdx == entitys.Count - 1 || entity.RType == RectType.Ruby) continue; //不調整高度大於眾數的實體、不調整最後一個實體高度

                    var entityNext = entitys[eIdx + 1];
                    float entitySpace = entityNext.Y - entity.Y - entity.Height;
                    float newHeight = entityNext.Y + entityNext.Height - entity.Y;
                    while (entitySpace < modes.HeighMin * 0.5 && newHeight < modes.Heigh * EntityMergeTBMaxRate) //前後實體高度在範圍內，則合併 * EntityMaxRate
                    {
                        entity.RType = RectType.MergeTB;
                        entity.Height = newHeight;
                        entitys[eIdx] = entity;
                        entitys.RemoveAt(eIdx + 1);

                        if (eIdx + 1 < entitys.Count)
                        {
                            entityNext = entitys[eIdx + 1];
                            entitySpace = entityNext.Y - entity.Y - entity.Height;
                            newHeight = entityNext.Y + entityNext.Height - entity.Y;
                        }
                        else break;
                    }
                }

                if (columnRuby.Count > 0) //有偵測到 Rubys 內容時，調整 columnRuby
                {
                    var firstEntity = entitys[0];
                    var lastEntity = entitys[entitys.Count - 1];
                    columnRect.Y = firstEntity.Y;
                    columnRect.Height = lastEntity.Y + lastEntity.Height - columnRect.Y;
                    columnRects[idx] = columnRect;
                    for (int eidx = 0; eidx < entitys.Count; eidx++)
                    {
                        var entity = entitys[eidx];
                        entity.Width = modes.Width;
                        entitys[eidx] = entity;
                    }
                    columnRect.Width = modes.Width;
                    columnRects[idx] = columnRect;
                    var firstRuby = columnRuby[0];
                    var lastRuby = columnRuby[columnRuby.Count - 1];
                    columnRects.Insert(++idx, (RectType.Ruby, firstRuby.X, firstRuby.Y, firstRuby.Width, lastRuby.Y + lastRuby.Height - firstRuby.Y, columnRuby));
                }
            }
        }
        public static void AnalysisEntityHeadBodyEnd((RectType RType, float X, float Y, float Width, float Height) rectRegion,
            List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnRects,
            (float Top, float Bottom, float Left, float Right, float Width, float Heigh, float TopMin, float BottomMin, float LeftMin, float RightMin,
                float WidthMin, float WidthMax, float HeighMin, float HeighMax) modes)
        {
            int ConfirmEntityHeadGap = Properties.Settings.Default.ConfirmEntityHeadGap.Value;
            int ConfirmEntityEndGap = Properties.Settings.Default.ConfirmEntityEndGap.Value;
            for (int idx = 0; idx < columnRects.Count; idx++)
            {
                var columnRect = columnRects[idx];
                if (idx == 0 && columnRect.X > modes.LeftMin) columnRect.RType = RectType.BodyOut; //判斷是否為末行／句尾
                else if (idx == columnRects.Count - 1 && columnRect.X + columnRect.Width < modes.RightMin) //判斷是否為首行／句首
                {
                    if (columnRect.RType != RectType.Ruby) columnRect.RType = RectType.BodyIn;
                    else
                    {
                        var columnRectPrev = columnRects[idx - 1];
                        columnRectPrev.RType = RectType.BodyIn;
                        columnRects[idx - 1] = columnRectPrev;
                    }
                }
                if (columnRect.RType == RectType.BodyIn || columnRect.RType == RectType.BodyOut) columnRects[idx] = columnRect;
                if (columnRect.RType == RectType.Ruby) continue;

                var entitys = columnRect.Entitys;

                if (entitys == null || entitys.Count == 0) continue;

                var entitysFirst = entitys[0];
                var entitysLast = entitys[entitys.Count - 1];

                if (entitysFirst.Y - rectRegion.Y - ConfirmEntityHeadGap > modes.HeighMin * 0.5 && entitysFirst.Height > modes.HeighMin * 0.5) // entitysFirst.Y > modes.TopMin + ConfirmEntityHeadGap //- rectRegion.Y > modes.HeighMin * 0.5)
                { //判斷是否為句首，首字上方空白要大於最小字高，且首字高度比最小字高的一半以上，例如首字為【一】上方雖然有空白，但並非句首
                    entitysFirst.RType = RectType.EntityHead;
                    entitys[0] = entitysFirst;
                }
                if (modes.BottomMin - entitysLast.Y - entitysLast.Height > modes.HeighMax + ConfirmEntityEndGap) //entitysLast.Y + entitysLast.Height + ConfirmEntityEndGap < modes.BottomMin
                { //判斷是否為句尾
                    entitysLast.RType = RectType.EntityEnd;
                    entitys[entitys.Count - 1] = entitysLast;
                }
            }
        }

        #region Entity
        /// <summary>
        /// Create new Entity
        /// </summary>
        public static (RectType RType, float X, float Y, float Width, float Height) NewEntity(RectangleF rect, RectType RType = RectType.None) => (RType, rect.X, rect.Y, rect.Width, rect.Height);
        /// <summary>
        /// Create new Entity
        /// </summary>
        public static (RectType RType, float X, float Y, float Width, float Height) NewEntity(RectType RType = RectType.None, float X = 0, float Y = 0, float Width = 0, float Height = 0) => (RType, X, Y, Width, Height);
        /// <summary>
        /// Gets a Entity that contains the union of two Entity.
        /// </summary>
        public static (RectType RType, float X, float Y, float Width, float Height) UnionEntity(
            (RectType RType, float X, float Y, float Width, float Height) entity1, 
            (RectType RType, float X, float Y, float Width, float Height) entity2, RectType NewRType = RectType.None)
        {
            float X = entity1.X < entity2.X ? entity1.X : entity2.X;
            float Y = entity1.Y < entity2.Y ? entity1.Y : entity2.Y;
            float maxX1 = entity1.X + entity1.Width;
            float maxY1 = entity1.Y + entity1.Height;
            float maxX2 = entity2.X + entity2.Width;
            float maxY2 = entity2.Y + entity2.Height;
            float Width = maxX1 > maxX2 ? maxX1 - X : maxX2 - X;
            float Height = maxY1 > maxY2 ? maxY1 - Y : maxY2 - Y;

            return (NewRType == RectType.None ? entity1.RType : NewRType, X, Y, Width, Height);
        }
        /// <summary>
        /// Create new Entitys
        /// </summary>
        public static (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) NewEntitys(
            RectType RType = RectType.None, float X = 0, float Y = 0, float Width = 0, float Height = 0,
            List<(RectType RType, float X, float Y, float Width, float Height)> Entitys = null) => (RType, X, Y, Width, Height, Entitys);
        /// <summary>
        /// Gets a Entitys that contains the union of two Entitys.
        /// </summary>
        public static (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) UnionEntitys(
            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) rect1,
            (RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys) rect2,
            RectType NewRType = RectType.None)
        {
            var rect = UnionEntity((rect1.RType, rect1.X, rect1.Y, rect1.Width, rect1.Height), (rect2.RType, rect2.X, rect2.Y, rect2.Width, rect2.Height),
                    NewRType == RectType.None ? rect1.RType : NewRType);

            var entitys1 = rect1.Entitys.ToArray();
            var entitys2 = rect2.Entitys.ToArray();

            if (entitys1.Length < entitys2.Length) //實體數較多者往前移
            {
                var entitysTmp = entitys1;
                entitys1 = entitys2;
                entitys2 = entitysTmp;
            }

            bool isMerge = false;
            int nIdx = -1;
            for (int eIdx = 0; eIdx < entitys1.Length; eIdx++)
            {
                var entity1 = entitys1[eIdx];
                if (++nIdx < entitys2.Length)
                {
                    var entity2 = entitys2[nIdx];
                    if ((entity1.Y >= entity2.Y && entity1.Y <= entity2.Y + entity2.Height) || (entity2.Y >= entity1.Y && entity2.Y <= entity1.Y + entity1.Height))
                    {
                        isMerge = true;
                        entity1 = UnionEntity(entity1, entity2, NewRType == RectType.None ? RectType.MergeLR : NewRType);
                    }
                }
                entity1.X = rect.X;
                entity1.Width = rect.Width;
                entitys1[eIdx] = entity1;
                if (eIdx == entitys1.Length - 1 && nIdx < entitys2.Length - 1)
                {

                }
            }
            if (isMerge) rect.RType = NewRType == RectType.None ? RectType.MergeLR : NewRType;

            return (rect.RType, rect.X, rect.Y, rect.Width, rect.Height, new List<(RectType RType, float X, float Y, float Width, float Height)>(entitys1));
        }

        public static RectangleF EntityToRectangleF((RectType RType, float X, float Y, float Width, float Height) entity) => new RectangleF(entity.X, entity.Y, entity.Width, entity.Height);
        #endregion
        
        /// <summary>
        /// 取得高度、寬度...等長度中的眾數(Mode)
        /// </summary>
        public static float GetModeMostOftenLen(ConcurrentDictionary<float, int> lenCountDict, float minLen = -1, float maxLen = -1)
        {
            float rtnLen = 0, rtnCount = 0;
            foreach (float len in lenCountDict.Keys)
            {
                int count = lenCountDict[len];
                if (minLen > -1 && len > minLen) continue; //設定其最小範圍
                if (maxLen > -1 && len < maxLen) continue; //設定最最大範圍
                if (rtnCount < count || (rtnCount == count && rtnLen < len)) //紀錄最大次數的寬度，多筆相同次數則取寬度較大那筆
                {
                    rtnLen = len;
                    rtnCount = count;
                }
            }

            return rtnLen;
        }

        public static void SaveImage(Image srcImage, string path, string name, string extension, ImageCodecInfo imgEncoder, EncoderParameters encoderParameters, PixelFormat pixelFormat = PixelFormat.DontCare)
        {
            Directory.CreateDirectory(path);
            if (srcImage.Tag is bool isIllustration && isIllustration)
            {
                ImageCodecInfo tmpEncoder = ImageCodecInfo.GetImageDecoders().Where(m => m.FormatID == srcImage.RawFormat.Guid).FirstOrDefault();
                if (tmpEncoder != null)
                {
                    imgEncoder = tmpEncoder;
                    var result = Regex.Match(imgEncoder.FilenameExtension.ToLower(), @"\*(\.[^;]+)");
                    if (result.Success) extension = result.Groups[1].Value;
                }
                srcImage.Save(string.Format(@"{0}\{1}{2}", path, name, extension));
            }
            else
            {
                using (Bitmap bitmap = new Bitmap(srcImage.Width, srcImage.Height))
                {
                    bitmap.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(srcImage, 0, 0);
                    }
                    if (pixelFormat == PixelFormat.Indexed) //pixelFormat != PixelFormat.DontCare
                    {
                        using (Bitmap bitmapClone = bitmap.Clone(new Rectangle(Point.Empty, bitmap.Size), pixelFormat))
                        {
                            bitmapClone.Save(string.Format(@"{0}\{1}{2}", path, name, extension), imgEncoder, encoderParameters);
                        }
                    }
                    else bitmap.Save(string.Format(@"{0}\{1}{2}", path, name, extension), imgEncoder, encoderParameters);
                }
            }
        }

        public static void ChangeForeColor(in Bitmap bmp, int foreArgb, int backArgb, float ForeColorRate = 1)
        {
            byte ConfirmWhiteLevel = Properties.Settings.Default.ConfirmWhiteLevel.Value; //指定白色數值

            BitmapTool bmpTool = new BitmapTool(bmp);
            bmpTool.ReadLockBits();

            byte foreA = (byte)(foreArgb >> 24);
            byte foreR = (byte)(foreArgb >> 16);
            byte foreG = (byte)(foreArgb >> 8);
            byte foreB = (byte)foreArgb;
            for (int y = 0; y < bmpTool.Height; y++)
            {
                for (int x = 0; x < bmpTool.Width; x++)
                {
                    int argb = bmpTool.GetPixel(x, y);
                    bool isBlack = IsBlack(argb, out _, ConfirmWhiteLevel);
                    if (isBlack)
                    {
                        byte blackA = (byte)(argb >> 24);
                        byte blackR = (byte)(argb >> 16);
                        byte blackG = (byte)(argb >> 8);
                        byte blackB = (byte)(argb);
                        byte aOut = (byte)(foreA + (float)blackA * (255 - foreA) / 255);
                        byte rOut = AverageColor(foreR, blackR, ForeColorRate, foreA, blackA); //(int)(ForeColorRate * foreR * (float)foreA / 255 + blackRate * blackR * (float)blackA / 255);
                        byte gOut = AverageColor(foreG, blackG, ForeColorRate, foreA, blackA); //(int)(ForeColorRate * foreG * (float)foreA / 255 + blackRate * blackG * (float)blackA / 255);
                        byte bOut = AverageColor(foreB, blackB, ForeColorRate, foreA, blackA);//(int)(ForeColorRate * foreB * (float)foreA  / 255 + blackRate * blackB * (float)blackA / 255);

                        int outArgb = (aOut << 24) | (rOut << 16) | (gOut << 8) | bOut;
                        bmpTool.SetPixel(x, y, outArgb);
                    }
                    else bmpTool.SetPixel(x, y, backArgb);
                }
            }
            bmpTool.WriteUnlockBits();
        }

        private static byte AverageColor(byte colorA, byte colorB, float ForeColorRate, byte alphaA=255, byte alphaB=255)
        {
            ForeColorRate = ForeColorRate > 2 ? 2 : ForeColorRate;
            float BlackForeColorRate = (float)2 - ForeColorRate;

            return (byte)((ForeColorRate * colorA * (float)alphaA / 255 + BlackForeColorRate * colorB * (float)alphaB / 255) / 2);
        }
    }
}
