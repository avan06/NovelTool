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
        Outer,    //外圍方格
        EntityHead,  //實體(首字)
        EntityBody,  //實體
        EntityEnd,  //實體(尾字)
        Blank,    //空白
        Ruby,    //ruby／ルビ／旁註標記
        Symbol,    //符號
        Illustration,    //圖形
        SplitTop,    //分割字上半部
        SplitMiddle, //分割字中間層
        SplitBottom,    //分割字下半部
        SplitLeft,    //分割字左半部
        SplitRight,    //分割字右半部
        AlignBottom,    //輸出位置置底
        AlignMiddle,    //輸出位置置中
        AozoraBold,    //粗體字
        AlignMiddleNoNewPage,    //不換行
        ScaleFullHeight,    //強制放大輸出高度至整頁
        OutputRotate,    //輸出圖片需旋轉90度
        MergeLR,    //合併(合併左右分離的文字)
        MergeLR2,    //合併(合併左右分離的文字)
        MergeLR3,    //合併(合併左右分離的文字)
        MergeLR4,    //合併(合併左右分離的文字)
        MergeTB,    //合併(合併上下分離的文字)
        MergeTB2,    //合併(合併上下分離的文字)
        MergeBlank,    //合併(與額外空白合併的文字)
        NoMerge1,    //不上下合併
        NoMerge2,    //不上下合併
        NoMerge3,    //不上下合併
        NoMerge4,    //不上下合併
        NoMerge5,    //不上下合併
        NoMerge6,    //不上下合併
        NoMerge7,    //不上下合併
        NoMerge8,    //不上下合併
        MainLeft,    //左主體
        MainRight,    //右主體
        MainTop,    //上主體
        MainBottom,    //下主體
    }
    public enum PositionType
    {
        None,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
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
