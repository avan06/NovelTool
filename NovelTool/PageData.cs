using System.Collections.Generic;

namespace NovelTool
{
    public class PageData
    {
        public int seq;
        public string path, name, extension;
        public bool isIllustration;
        public (RectType RType, float X, float Y, float Width, float Height) rectImg, rectHead, rectBody, rectFooter;
        public List<(RectType RType, float X, float Y, float Width, float Height, List<(RectType RType, float X, float Y, float Width, float Height)> Entitys)> columnHeadList, columnBodyList, columnFooterList;
        public List<(string text, string ruby)> textList;
        public Dictionary<(int X, int Y), int> pStates;
        public List<int> xStates, yStates, xStatesHead, xStatesBody, xStatesFooter;

        public PageData(int seq, string path, string name, string extension)
        {
            this.seq = seq;
            this.path = path;
            this.name = name;
            this.extension = extension;
        }
    }

    public enum RectType
    {
        None,    //無
        Head,    //表頭
        Body,    //主體
        BodyIn,  //主體起始
        BodyOut,  //主體結束
        Footer,    //表尾
        EntityHead,  //實體(首字)
        EntityBody,  //實體
        EntityEnd,  //實體(尾字)
        Ruby,    //ruby／ルビ／旁註標記
        Illustration,    //圖形
        SplitTop,    //分割字上半部
        SplitMiddle, //分割字中間層
        SplitBottom,    //分割字下半部
        MergeLR,    //合併(合併左右分離的文字)
        MergeTB,    //合併(合併上下分離的文字)
    }

    public enum PositionType
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Top,
        Bottom,
    }

    public enum ImageType
    {
        Jpeg,
        Png,
        Tiff,
        Bmp,
        Gif,
    }
}
