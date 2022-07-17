//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace NovelTool
//{
//    class Config
//    {
//        //InputPath 輸入來源位置
//        public string InputPath { get; set; }
//        //InputDirectionVertical 輸入文字方向為垂直，true: 上至下右至左，false: 左至右上至下
//        public bool InputDirectionVertical { get; set; }

//        //OutputFolder 輸出目標位置
//        public string OutputFolder { get; set; }
//        //OutputWidth 輸出圖片寬度設定
//        public int OutputWidth { get; set; }
//        //OutputHeight 輸出圖片高度設定
//        public int OutputHeight { get; set; }
//        //OutputBGColor 輸出圖片底色設定
//        public string OutputBGColor { get; set; }
//        //OutputImgFormat 設定輸出格式，jpg、png、gif、bmp
//        public string OutputImgFormat { get; set; }

//        //OffsetWhite 白色的偏移量
//        public byte OffsetWhite { get; set; }
//        //DefineWhite 解析圖片時，判定為白色區域的設定（ex. 245~255）
//        public byte DefineWhite { get; set; }
//        //DefineBlack 解析圖片時，判定非白色區域自動色彩加深（ex. 0~244 自動減30）
//        public byte DefineBlack { get; set; }

//        //MarginLeft 輸出圖片左側邊距
//        public int MarginLeft { get; set; }
//        //MarginRight 輸出圖片右側邊距
//        public int MarginRight { get; set; }
//        //MarginTop 輸出圖片上側邊距
//        public int MarginTop { get; set; }
//        //MarginBottom 輸出圖片下側邊距
//        public int MarginBottom { get; set; }

//        //IsShowAnalysisImg Encode bodyRects to image file
//        public bool IsShowAnalysisImg { get; set; }
//        //IsHighContrast 是否提升文字圖形對比
//        public bool IsHighContrast { get; set; }

//        //DrawHead 是否輸出表頭
//        public bool DrawHead { get; set; }
//        //MarginHead 輸出圖片表頭邊距
//        public int MarginHead { get; set; }
//        //DrawFooter 是否輸出頁尾
//        public bool DrawFooter { get; set; }
//        //MarginFooter 輸出圖片頁尾邊距
//        public int MarginFooter { get; set; }
//        //LeadingSize 輸出圖片時，重整文字圖片的每行邊距設定
//        public int LeadingSize { get; set; }
//        //ExporScale 輸出圖片時，重整文字圖片的字體放大倍率，設定為零則以 MaxFontWidth 來自動判斷放大率
//        public double ExporScale { get; set; }
//        //MaxFontWidth 輸出圖片的文字最大寬度
//        public int MaxFontWidth { get; set; }
//        //IsFColorInversion 是否反轉前景顏色
//        public bool IsFColorInversion { get; set; }
//        //IsBColorInversion 是否反轉背景顏色
//        public bool IsBColorInversion { get; set; }

//        //IllustrationMinWidth Minimum graphic width
//        public int IllustrationMinWidth { get; set; }
//        //IllustrationhMinHeight Minimum graphic height
//        public int IllustrationhMinHeight { get; set; }
//        //IllustrationRate proportion of grap
//        public double IllustrationRate { get; set; }

//        //HeadMinRate Minimum proportion of Head
//        public double HeadMinRate { get; set; }
//        //FooterMinRate Minimum proportion of Footer
//        public double FooterMinRate { get; set; }

//        //ContextMinRate Minimum proportion of context
//        public double ContextMinRate { get; set; }
//        //ContextMaxRate Maximum proportion of context
//        public double ContextMaxRate { get; set; }
//        //RubyMinRate Minimum proportion of ruby
//        public double RubyMinRate { get; set; }
//        //RubyMaxRate Maximum proportion of ruby
//        public double RubyMaxRate { get; set; }
//        //SymbolMinRate Minimum proportion of symbol
//        public double SymbolMinRate { get; set; }
//        //SymbolMaxRate Maximum proportion of symbol
//        public double SymbolMaxRate { get; set; }

//        //SplitMaxNonBlank Allow non-blank maximum limit when splitting
//        public int SplitMaxNonBlank { get; set; }
//        //SplitMinRate Minimum proportion for split word
//        public double SplitMinRate { get; set; }
//        //SplitMaxRate Maximum proportion for split word
//        public double SplitMaxRate { get; set; }

//        //AddPageNumber 是否輸出顯示頁數
//        public bool AddPageNumber { get; set; }
//        //AddSourcePageNumber 是否輸出顯示來源檔名稱,
//        public bool AddSourceName { get; set; }
//        //PageNumberPosition TopLeft、TopRight、BottomLeft、BottomRight
//        public PositionType PageNumberPosition { get; set; }

//        //IsAnalysisCenter 是否從中間位置切圖
//        public bool IsAnalysisCenter { get; set; }
//        //IsAnalysisHorizon 是否從水平面切圖
//        public bool IsAnalysisHorizon { get; set; }
//        //IsEdgeBlankRemove 從中間位置切圖時，是否移除邊緣空白
//        public bool IsEdgeBlankRemove { get; set; }
//        //CenterDetectorRata 從中間位置切圖時，偵測中間範圍佔 X 軸的比例
//        public double CenterDetectorRata { get; set; }

//        //IsConfirmIllustrationByCount 是否使用非白色數量大小來辨識插圖（辨識錯誤時改為false）
//        public bool IsConfirmIllustrationByCount { get; set; }
//        //ConfirmIllustrationMinCount 檢測為插圖的非白色最小數量（超過此值判定為插圖）
//        public int ConfirmIllustrationMinCount { get; set; }

//        //ConfirmHeadGap 檢測頁面的表頭離主體之間的空隙大小
//        public int ConfirmHeadGap { get; set; }
//        //ConfirmFooterGap 檢測頁面的頁尾離主體之間的空隙大小
//        public int ConfirmFooterGap { get; set; }

//        //IgnoreMinDetectYSize 偵測圖片 Head、Footer 之最小高度
//        public int IgnoreMinDetectYSize { get; set; }
//        //IgnoreEdgeSize 偵測圖片時忽略外測邊緣處的大小，0:不忽略
//        public int IgnoreEdgeSize { get; set; }

//        //IsNewLineForENDtoHEADSymbol 原稿文字連續的兩行，頭尾為符號時，輸出時是否需要換行
//        public bool IsNewLineForENDtoHEADSymbol { get; set; }

//        //EndBoundaryMinCount 統計整體底端位置時，忽略少於最小次數的值
//        public int EndBoundaryMinCount { get; set; }

//        //StartAnalysIndex 開始分析圖片的起始位置，將小於此索引數的來源檔案列為Illustration圖片
//        public int StartAnalysIndex { get; set; }

//        //isCalOuterGreaterContext 當寬度大於常規寬度(可能為標題欄位)，是否依據外圍重新計算每字寬度，否則直接用最寬的寬度
//        public bool isCalOuterGreaterContext { get; set; }

//        //for freetype
//        //public double TextDpi { get; set; }
//        //public string TextFont { get; set; } //msjh.ttc、meiryo.ttc、msgothic.ttc
//        //public string TextFontBold { get; set; } //msjhbd.ttc、meiryob.ttc、msgothic.ttc
//        //public double TextSize { get; set; }
//        //public double TextSpacing { get; set; }
//        //public font.Hinting Hinting { get; set; } //0:HintingNone、1:HintingVertical、2:HintingFull

//        public Config InitOption()
//        {
//            Config config = new Config
//            {
//                //InputDirectionVertical 輸入文字方向為垂直，true: 上至下右至左，false: 左至右上至下
//                InputDirectionVertical = true,

//                //OutputName 輸出目標位置,
//                OutputFolder = @"output\",
//                //OutputWidth 輸出圖片寬度設定,
//                OutputWidth = 1125,
//                //OutputHeight 輸出圖片高度設定,
//                OutputHeight = 1600,
//                //OutputBGColor 輸出圖片底色設定,
//                OutputBGColor = "#fff",
//                //OutputImgFormat 設定輸出格式，jpg、png、gif、bmp
//                OutputImgFormat = "png",

//                //OffsetWhite 白色的偏移量
//                OffsetWhite = 0,
//                //DefineWhite 解析圖片時，判定為白色區域的設定（ex. 245~255）,
//                DefineWhite = 245,
//                //DefineBlack 解析圖片時，判定非白色區域自動色彩加深（ex. 0~244 自動減30）,
//                DefineBlack = 50,

//                //MarginLeft 輸出圖片左側邊距,
//                MarginLeft = 30,
//                //MarginRight 輸出圖片右側邊距,
//                MarginRight = 30,
//                //MarginTop 輸出圖片上側邊距,
//                MarginTop = 30,
//                //MarginBottom 輸出圖片下側邊距,
//                MarginBottom = 30,

//                //IsShowAnalysisImg Encode bodyRects to image file,
//                IsShowAnalysisImg = true,
//                //IsHighContrast 是否提升文字圖形對比,
//                IsHighContrast = true,

//                //DrawHead 是否輸出表頭,
//                DrawHead = false,
//                //MarginHead 輸出圖片表頭邊距,
//                MarginHead = 40 + MarginTop,
//                //DrawFooter 是否輸出頁尾,
//                DrawFooter = false,
//                //MarginFooter 輸出圖片頁尾邊距,
//                MarginFooter = 40 + MarginBottom,
//                //LeadingSize 輸出圖片時，重整文字圖片的每行邊距設定,
//                LeadingSize = 30,
//                //ExporScale 輸出圖片時，重整文字圖片的字體放大倍率，設定為零則以 MaxFontWidth 來自動判斷放大率,
//                ExporScale = 0,
//                //MaxFontWidth 輸出圖片的文字最大寬度,
//                MaxFontWidth = 40,
//                //IsFColorInversion 是否反轉前景顏色,
//                IsFColorInversion = false,
//                //IsBColorInversion 是否反轉背景顏色,
//                IsBColorInversion = false,

//                //IllustrationMinWidth Minimum graphic width,
//                IllustrationMinWidth = 100,
//                //IllustrationhMinHeight Minimum graphic height,
//                IllustrationhMinHeight = 100,
//                //IllustrationRate proportion of grap
//                IllustrationRate = 3,

//                //HeadMinRate Minimum proportion of Head
//                HeadMinRate = 0.1,
//                //FooterMinRate Minimum proportion of Footer
//                FooterMinRate = 0.9,

//                //ContextMinRate Minimum proportion of context
//                ContextMinRate = 0.9,
//                //ContextMaxRate Maximum proportion of context
//                ContextMaxRate = 1.1,
//                //RubyMinRate Minimum proportion of ruby
//                RubyMinRate = 0.5,
//                //RubyMaxRate Maximum proportion of ruby,
//                RubyMaxRate = 0.7,
//                //SymbolMinRate Minimum proportion of symbol
//                SymbolMinRate = 0.2,
//                //SymbolMaxRate Maximum proportion of symbol
//                SymbolMaxRate = 0.5,

//                //SplitMaxNonBlank Allow non-blank maximum limit when splitting
//                SplitMaxNonBlank = 4,
//                //SplitMinRate Minimum proportion for split word
//                SplitMinRate = 0.85,
//                //SplitMaxRate Maximum proportion for split word
//                SplitMaxRate = 1.5,

//                //AddPageNumber 是否輸出顯示頁數,
//                AddPageNumber = true,
//                //AddSourcePageNumber 是否輸出顯示來源檔名稱,
//                AddSourceName = true,
//                //PageNumberPosition TopLeft、TopRight、BottomLeft、BottomRight,
//                PageNumberPosition = PositionType.BottomLeft,

//                //IsAnalysisCenter 是否從中間位置切圖,
//                IsAnalysisCenter = false,
//                //IsAnalysisHorizon 是否從水平面切圖,
//                IsAnalysisHorizon = false,
//                //IsEdgeBlankRemove 從中間位置切圖時，是否移除邊緣空白,
//                IsEdgeBlankRemove = false,
//                //CenterDetectorRata 從中間位置切圖時，偵測中間範圍佔 X 軸的比例,
//                CenterDetectorRata = 0.1,

//                //IsConfirmIllustrationByCount 是否使用非白色數量大小來辨識插圖（辨識錯誤時改為false）
//                IsConfirmIllustrationByCount = true,
//                //ConfirmIllustrationMinCount 檢測為插圖的非白色最小數量（超過此值判定為插圖）
//                ConfirmIllustrationMinCount = 390000,

//                //ConfirmHeadGap 檢測頁面的表頭離主體之間的空隙大小
//                ConfirmHeadGap = 15,
//                //ConfirmFooterGap 檢測頁面的頁尾離主體之間的空隙大小
//                ConfirmFooterGap = 15,

//                //IgnoreMinDetectYSize 偵測圖片 Head、Footer 之最小高度
//                IgnoreMinDetectYSize = 3,
//                //IgnoreEdgeSize 偵測圖片時忽略外測邊緣處的大小，0:不忽略
//                IgnoreEdgeSize = 3,

//                //IsNewLineForENDtoHEADSymbol 原稿文字連續的兩行，頭尾為符號時，輸出時是否需要換行
//                IsNewLineForENDtoHEADSymbol = false,

//                //EndBoundaryMinCount 統計整體底端位置時，忽略少於最小次數的值
//                EndBoundaryMinCount = 3,

//                //StartAnalysIndex 開始分析圖片的起始位置，將小於此索引數的來源檔案列為Illustration圖片
//                StartAnalysIndex = 0,

//                //isCalOuterGreaterContext 當寬度大於常規寬度(可能為標題欄位)，是否依據外圍重新計算每字寬度，否則直接用最寬的寬度
//                isCalOuterGreaterContext = true,

//                //for TextContext *freetype.Context,
//                //TextDpi = 72,
//                //TextFont = "meiryo.ttc",  //msgothic.ttc、msjhbd.ttc、meiryo.ttc
//                //TextFontBold = "meiryob.ttc", //msjhbd.ttc、meiryob.ttc、msgothic.ttc
//                //TextSize = 40,   //option.MaxFontWidth
//                //TextSpacing = 1.5,
//                //Hinting = font.HintingFull,
//            };
//            return config;
//        }
//    }
//}
