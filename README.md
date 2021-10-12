# NovelTool
NovelTool is a tool for rearranging novel/eBook image to the specified output resolution.

## Analysis
Analyze the novel/eBook image files sequentially from horizontal/vertical directions and recognized the position of the text image.

## Rearrangement
Rearrange the recognized text images and generate new image files according to the specified output resolution.

## Getting started
1. Open the folder or zip of novel/eBook file, analyze image content immediately.
2. Wait for the analysis of the novel/eBook results to be completed.
3. Maybe you need to change the option settings, when the analysis result is wrong. Re-analyze the novel/eBook file after changing settings.
4. Now execute GenerateView from the toolbar to confirm the generated content.
5. The size of the output text image can be changed.
6. Save the generated content as a new image file.

## Option settings

### Analysis
1. AnalysisFactor
   * AnalysisTaskThreadLimit
   * IgnoreMinDetectXSize
   * IgnoreMinDetectYSize
   * IllustrationMinHeight
   * IllustrationMinWidth

1. Rate
   * HeadMinRate
   * FooterMinRate
   * IllustrationRate
   * EntityMinRate
   * EntityMaxRate
   * EntityAdjacentRate
   * EntityMergeTBMaxRate

1. Confirm
   * ConfirmIllustrationMinColorsLevel
   * ConfirmIllustrationMinNonWhiteLevel
   * ConfirmWhiteLevel
   * ConfirmHeadGap
   * ConfirmFooterGap
   * ConfirmEntityHeadGap
   * ConfirmEntityEndGap

### Generate
1. Output
   * Width
   * Height
   * AdjustColor
   * BackColor
   * ForeColor
   * ForeColorRate
   * ImageType
   * PixelFormat
   * Quality
   * PagePosition

1. Body
   * MarginLeft
   * MarginRight
   * MarginTop
   * MarginBottom
   * Leading

1. Head & Footer
   * Position