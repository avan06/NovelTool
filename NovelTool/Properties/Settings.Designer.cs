﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NovelTool.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.2.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2|1_Analysis|1_AnalysisFactor|Determines the maximum number of threads to perform" +
            " analysis tasks (Default 2)")]
        public global::OptionTreeView.Option<System.Int32> AnalysisTaskThreadLimit {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["AnalysisTaskThreadLimit"]));
            }
            set {
                this["AnalysisTaskThreadLimit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3|1_Analysis|1_AnalysisFactor|Decided to ignore left and right widths when detect" +
            "ing images (Default 3, if there is non-text content on the edge)")]
        public global::OptionTreeView.Option<System.Int32> IgnoreMinDetectXSize {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IgnoreMinDetectXSize"]));
            }
            set {
                this["IgnoreMinDetectXSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3|1_Analysis|1_AnalysisFactor|Decided to ignore top and bottom widths when detect" +
            "ing images (Default 3, if there is non-text content on the edge)")]
        public global::OptionTreeView.Option<System.Int32> IgnoreMinDetectYSize {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IgnoreMinDetectYSize"]));
            }
            set {
                this["IgnoreMinDetectYSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("130|1_Analysis|1_AnalysisFactor|XXX")]
        public global::OptionTreeView.Option<System.Int32> IllustrationMinWidth {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IllustrationMinWidth"]));
            }
            set {
                this["IllustrationMinWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("130|1_Analysis|1_AnalysisFactor|XXX")]
        public global::OptionTreeView.Option<System.Int32> IllustrationMinHeight {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IllustrationMinHeight"]));
            }
            set {
                this["IllustrationMinHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.1|1_Analysis|2_Rate|Determines the scale value of the header on the page from t" +
            "op to bottom (Default 0.1, Max 1)")]
        public global::OptionTreeView.Option<System.Single> HeadMinRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["HeadMinRate"]));
            }
            set {
                this["HeadMinRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.95|1_Analysis|2_Rate|Determines the scale value of the footer on the page from " +
            "top to bottom (Default 0.95, Max 1)")]
        public global::OptionTreeView.Option<System.Single> FooterMinRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["FooterMinRate"]));
            }
            set {
                this["FooterMinRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3|1_Analysis|2_Rate|XXX")]
        public global::OptionTreeView.Option<System.Int32> IllustrationRate {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IllustrationRate"]));
            }
            set {
                this["IllustrationRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.6|1_Analysis|2_Rate|Determines the scale value of the smallest text (rubi chara" +
            "cters/ルビ) when the standard text scale is 1 (Default 0.6)")]
        public global::OptionTreeView.Option<System.Single> EntityMinRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["EntityMinRate"]));
            }
            set {
                this["EntityMinRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.2|1_Analysis|2_Rate|Determines the scale value of the largest text (parsable as" +
            " standard text) when the standard text scale is 1 (Default 1.2)")]
        public global::OptionTreeView.Option<System.Single> EntityMaxRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["EntityMaxRate"]));
            }
            set {
                this["EntityMaxRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.2|1_Analysis|2_Rate|Determines the minimum scale value that is immediately adja" +
            "cent between two lines (Default 0.2, for example standard text and rubi characte" +
            "rs)")]
        public global::OptionTreeView.Option<System.Single> EntityAdjacentRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["EntityAdjacentRate"]));
            }
            set {
                this["EntityAdjacentRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1|1_Analysis|2_Rate|Determines the scale value that can be merged into a one char" +
            "acter, when a character is broken by wrong analysis (Default 1, for example 元, 二" +
            ", 言...)")]
        public global::OptionTreeView.Option<System.Single> EntityMergeTBMaxRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["EntityMergeTBMaxRate"]));
            }
            set {
                this["EntityMergeTBMaxRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2000|1_Analysis|3_Confirm|Determine the minimum number of colors for illustration" +
            "s that can be detected as non-text (Default 2000, duplicate colors are not count" +
            "ed)")]
        public global::OptionTreeView.Option<System.Int32> IllustrationMinColorsLevel {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IllustrationMinColorsLevel"]));
            }
            set {
                this["IllustrationMinColorsLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("800000|1_Analysis|3_Confirm|Determine the minimum number of foreground colors for" +
            " illustrations that can be detected as non-text (Default 800000)")]
        public global::OptionTreeView.Option<System.Int32> IllustrationMinNonWhiteLevel {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["IllustrationMinNonWhiteLevel"]));
            }
            set {
                this["IllustrationMinNonWhiteLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("240|1_Analysis|3_Confirm|Determines the color value detected as the background(wh" +
            "ite) color (Default 240)")]
        public global::OptionTreeView.Option<System.Byte> ConfirmWhiteLevel {
            get {
                return ((global::OptionTreeView.Option<System.Byte>)(this["ConfirmWhiteLevel"]));
            }
            set {
                this["ConfirmWhiteLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15|1_Analysis|3_Confirm|Determines the minimum height  of the top margin above th" +
            "e header (Default 15)")]
        public global::OptionTreeView.Option<System.Int32> ConfirmHeadGap {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["ConfirmHeadGap"]));
            }
            set {
                this["ConfirmHeadGap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15|1_Analysis|3_Confirm|Determines the minimum height of the bottom margin below " +
            "the footer (Default 15)")]
        public global::OptionTreeView.Option<System.Int32> ConfirmFooterGap {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["ConfirmFooterGap"]));
            }
            set {
                this["ConfirmFooterGap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0|1_Analysis|3_Confirm|This value is related to determining whether it is the fir" +
            "st word of the line (Default 0)")]
        public global::OptionTreeView.Option<System.Int32> ConfirmEntityHeadGap {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["ConfirmEntityHeadGap"]));
            }
            set {
                this["ConfirmEntityHeadGap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0|1_Analysis|3_Confirm|This value is related to determining whether it is the end" +
            " of the line (Default 0)")]
        public global::OptionTreeView.Option<System.Int32> ConfirmEntityEndGap {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["ConfirmEntityEndGap"]));
            }
            set {
                this["ConfirmEntityEndGap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1|2_PreviewRect|1_Rect View|Determines the rendered Rect line width (Default 1)")]
        public global::OptionTreeView.Option<System.Int32> RectViewWidth {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["RectViewWidth"]));
            }
            set {
                this["RectViewWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkMagenta|2_PreviewRect|2_Rect Color|Determines the rendered header Rect line c" +
            "olor (Default DarkMagenta)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectHeadColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectHeadColor"]));
            }
            set {
                this["RectHeadColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Coral|2_PreviewRect|2_Rect Color|Determines the rendered body Rect line color (De" +
            "fault Coral)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectBodyColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectBodyColor"]));
            }
            set {
                this["RectBodyColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightSkyBlue|2_PreviewRect|2_Rect Color|Determines the rendered footer Rect line " +
            "color (Default LightSkyBlue)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectFooterColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectFooterColor"]));
            }
            set {
                this["RectFooterColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightSkyBlue|2_PreviewRect|2_Rect Color|Determines the rendered column Rect line " +
            "color (Default LightSkyBlue)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectColumnColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectColumnColor"]));
            }
            set {
                this["RectColumnColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red|2_PreviewRect|2_Rect Color|Determines the rendered entity body Rect line colo" +
            "r (Default Red)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectEntityBodyColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectEntityBodyColor"]));
            }
            set {
                this["RectEntityBodyColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MediumSpringGreen|2_PreviewRect|2_Rect Color|Determines the rendered column ruby " +
            "Rect line color (Default MediumSpringGreen)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectColumnRubyColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectColumnRubyColor"]));
            }
            set {
                this["RectColumnRubyColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkGreen|2_PreviewRect|2_Rect Color|Determines the rendered ruby Rect line color" +
            " (Default DarkGreen)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectRubyColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectRubyColor"]));
            }
            set {
                this["RectRubyColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MediumOrchid|2_PreviewRect|2_Rect Color|Determines the rendered mergeTB Rect line" +
            " color (Default MediumOrchid)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectMergeTBColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectMergeTBColor"]));
            }
            set {
                this["RectMergeTBColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Tomato|2_PreviewRect|2_Rect Color|Determines the rendered mergeLR Rect line color" +
            " (Default Tomato)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectMergeLRColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectMergeLRColor"]));
            }
            set {
                this["RectMergeLRColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Gold|2_PreviewRect|2_Rect Color|Determines the rendered split top Rect line color" +
            " (Default Gold)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectSplitTopColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectSplitTopColor"]));
            }
            set {
                this["RectSplitTopColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RoyalBlue|2_PreviewRect|2_Rect Color|Determines the rendered split middle Rect li" +
            "ne color (Default RoyalBlue)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectSplitMiddleColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectSplitMiddleColor"]));
            }
            set {
                this["RectSplitMiddleColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightCoral|2_PreviewRect|2_Rect Color|Determines the rendered split bottom Rect l" +
            "ine color (Default LightCoral)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> RectSplitBottomColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["RectSplitBottomColor"]));
            }
            set {
                this["RectSplitBottomColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1200|3_Generate|1_Output|Determine the output width (Default 1200)")]
        public global::OptionTreeView.Option<System.Int32> OutputWidth {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["OutputWidth"]));
            }
            set {
                this["OutputWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1600|3_Generate|1_Output|Determine the output height (Default 1600)")]
        public global::OptionTreeView.Option<System.Int32> OutputHeight {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["OutputHeight"]));
            }
            set {
                this["OutputHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False|3_Generate|1_Output|Determine whether to use `ForeColorRate` to adjust the " +
            "contrast color when generating (Default false)")]
        public global::OptionTreeView.Option<System.Boolean> OutputAdjustColorCheck {
            get {
                return ((global::OptionTreeView.Option<System.Boolean>)(this["OutputAdjustColorCheck"]));
            }
            set {
                this["OutputAdjustColorCheck"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.2|3_Generate|1_Output|This value adjusts the contrast color when `OutputAdjustC" +
            "olorCheck` is enabled (Default 0.2, the range is 0~2)")]
        public global::OptionTreeView.Option<System.Single> ForeColorRate {
            get {
                return ((global::OptionTreeView.Option<System.Single>)(this["ForeColorRate"]));
            }
            set {
                this["ForeColorRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("White|3_Generate|1_Output|Determines the output background color (Default White)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> OutputBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["OutputBackColor"]));
            }
            set {
                this["OutputBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black|3_Generate|1_Output|Determines the output foreground color (Default Black)")]
        public global::OptionTreeView.Option<System.Drawing.KnownColor> OutputForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.KnownColor>)(this["OutputForeColor"]));
            }
            set {
                this["OutputForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Jpeg|3_Generate|1_Output|Determine the output image type (Default Jpeg)")]
        public global::OptionTreeView.Option<NovelTool.ImageType> OutputImageType {
            get {
                return ((global::OptionTreeView.Option<NovelTool.ImageType>)(this["OutputImageType"]));
            }
            set {
                this["OutputImageType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DontCare|3_Generate|1_Output|Determine the output Pixel format (Default DontCare)" +
            "")]
        public global::OptionTreeView.Option<System.Drawing.Imaging.PixelFormat> OutputPixelFormat {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Imaging.PixelFormat>)(this["OutputPixelFormat"]));
            }
            set {
                this["OutputPixelFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("90|3_Generate|1_Output|Determine the output image quality, A quality level of 0 c" +
            "orresponds to the greatest compression, and a quality level of 100 corresponds t" +
            "o the least compression (Default 90)")]
        public global::OptionTreeView.Option<System.Int64> OutputQuality {
            get {
                return ((global::OptionTreeView.Option<System.Int64>)(this["OutputQuality"]));
            }
            set {
                this["OutputQuality"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None|3_Generate|1_Output|Determine the page number position of the output image (" +
            "Default None)")]
        public global::OptionTreeView.Option<NovelTool.PositionType> PagePositionType {
            get {
                return ((global::OptionTreeView.Option<NovelTool.PositionType>)(this["PagePositionType"]));
            }
            set {
                this["PagePositionType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30|3_Generate|2_Body|Determines the left margin width of the output image (Defaul" +
            "t 30)")]
        public global::OptionTreeView.Option<System.Int32> MarginLeft {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["MarginLeft"]));
            }
            set {
                this["MarginLeft"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30|3_Generate|2_Body|Determines the width of the right margin of the output image" +
            " (Default 30)")]
        public global::OptionTreeView.Option<System.Int32> MarginRight {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["MarginRight"]));
            }
            set {
                this["MarginRight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30|3_Generate|2_Body|Determines the width of the top margin of the output image (" +
            "Default 30)")]
        public global::OptionTreeView.Option<System.Int32> MarginTop {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["MarginTop"]));
            }
            set {
                this["MarginTop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30|3_Generate|2_Body|Determines the bottom margin width of the output image (Defa" +
            "ult 30)")]
        public global::OptionTreeView.Option<System.Int32> MarginBottom {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["MarginBottom"]));
            }
            set {
                this["MarginBottom"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30|3_Generate|2_Body|Determine the output image line spacing width (Default 30)")]
        public global::OptionTreeView.Option<System.Int32> Leading {
            get {
                return ((global::OptionTreeView.Option<System.Int32>)(this["Leading"]));
            }
            set {
                this["Leading"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None|3_Generate|3_Head|Determine the header position of the output image (Default" +
            " None)")]
        public global::OptionTreeView.Option<NovelTool.PositionType> HeadPositionType {
            get {
                return ((global::OptionTreeView.Option<NovelTool.PositionType>)(this["HeadPositionType"]));
            }
            set {
                this["HeadPositionType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None|3_Generate|3_Footer|Determine the footer position of the output image (Defau" +
            "lt None)")]
        public global::OptionTreeView.Option<NovelTool.PositionType> FooterPositionType {
            get {
                return ((global::OptionTreeView.Option<NovelTool.PositionType>)(this["FooterPositionType"]));
            }
            set {
                this["FooterPositionType"] = value;
            }
        }
    }
}
