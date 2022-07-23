using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NovelTool
{
    /// <summary>
    /// https://github.com/Zaid-Ajaj/Image-Processor/blob/master/ImageFilter/LockBitmap.cs
    /// </summary>
    public class BitmapTool : IDisposable
    {
        ImageLockMode imageLockMode = ImageLockMode.ReadWrite;
        Bitmap Source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        // get total locked pixels count
        readonly int PixelCount;

        // create byte array to copy pixel values／color components count
        readonly int Step;
        readonly int PixelLength;
        // Create rectangle to lock
        Rectangle Rect;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public BitmapTool(Bitmap source, bool isReadOnly = false, bool isImmediateInitPixels = false)
        {
            this.Source = source;
            // get source bitmap pixel format size
            Depth = Image.GetPixelFormatSize(source.PixelFormat);
            // Check if bpp (Bits Per Pixel) is 8, 24, or 32
            if (Depth != 8 && Depth != 24 && Depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }
            // Get width and height of bitmap
            Width = source.Width;
            Height = source.Height;
            // get total locked pixels count
            PixelCount = Width * Height;
            // Create rectangle to lock
            Rect = new Rectangle(0, 0, Width, Height);
            // create byte array to copy pixel values
            Step = Depth / 8;
            PixelLength = PixelCount * Step;
            Pixels = new byte[PixelLength];
            imageLockMode = isReadOnly ? ImageLockMode.ReadOnly : imageLockMode;

            if (isImmediateInitPixels)
            {
                ReadLockBits();
                WriteUnlockBits();
            }
        }

        /// <summary>
        /// Lock bitmap data into system memory.
        /// </summary>
        public void ReadLockBits()
        {
            try
            {
                // Lock bitmap and return bitmap data
                bitmapData = Source.LockBits(Rect, imageLockMode, Source.PixelFormat);
                Iptr = bitmapData.Scan0;
                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data from system memory.
        /// </summary>
        public void WriteUnlockBits()
        {
            try
            {
                if (bitmapData == null) return;
                // Copy data from byte array to pointer
                if (imageLockMode != ImageLockMode.ReadOnly) Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                Source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Get the argb of the specified pixel
        /// </summary>
        public int GetPixel(int x, int y)
        {
            int argb = 0;

            // Get color components count
            //int cCount = Depth / 8;

            // Get start index of the specified pixel
            int idx = ((y * Width) + x) * Step;

            if (idx > Pixels.Length - Step) throw new IndexOutOfRangeException();

            if (Depth == 32)
            { // For 32 bpp get Red, Green, Blue and Alpha
                byte b = Pixels[idx];
                byte g = Pixels[idx + 1];
                byte r = Pixels[idx + 2];
                byte a = Pixels[idx + 3];
                argb = (a << 24) | (r << 16) | (g << 8) | b; //clr = Color.FromArgb(a, r, g, b).ToArgb();
            }
            if (Depth == 24)
            { // For 24 bpp get Red, Green and Blue
                byte b = Pixels[idx];
                byte g = Pixels[idx + 1];
                byte r = Pixels[idx + 2];
                argb = (255 << 24) | (r << 16) | (g << 8) | b; //clr = Color.FromArgb(r, g, b).ToArgb();
            }
            if (Depth == 8)
            { // For 8 bpp get color value (Red, Green and Blue values are the same)
                byte c = Pixels[idx];
                argb = (255 << 24) | (c << 16) | (c << 8) | c; //clr = Color.FromArgb(c, c, c).ToArgb();
            }
            return argb;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        public void SetPixel(int x, int y, int argb)
        {
            if (imageLockMode == ImageLockMode.ReadOnly) throw new ArgumentException(string.Format("ImageLockMode is ReadOnly, can't SetPixel."));

            // Get color components count
            int cCount = Depth / 8;
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            //A (byte)(argb >> 24)
            //R (byte)(argb >> 16)
            //G (byte)(argb >> 8) 
            //B (byte)(argb)
            Pixels[i] = (byte)(argb);
            if (Depth >= 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i + 1] = (byte)(argb >> 8);
                Pixels[i + 2] = (byte)(argb >> 16);
            }
            if (Depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i + 3] = (byte)(argb >> 24);
            }
        }

        public void Dispose()
        {
            if (Iptr != IntPtr.Zero) Iptr = IntPtr.Zero;
            if (Pixels != null) Pixels = null;
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Hadamard product
    /// https://softwarebydefault.com/2013/05/01/image-convolution-filters/
    /// https://softwarebydefault.com/2013/05/18/difference-of-gaussians/
    /// https://softwarebydefault.com/2013/05/11/image-edge-detection/
    /// https://lodev.org/cgtutor/filtering.html
    /// https://desktop.arcgis.com/en/arcmap/latest/manage-data/raster-and-images/convolution-function.htm
    /// </summary>
    public static class BitmapFilter
    {
        public enum Filter
        {
            Sharpen3x3Type1,
            Sharpen3x3Type2,
            Sharpen3x3Factor,
            Sharpen3x3Type3,
            Sharpen3x3Type4,
            Sharpen5x5Type1,
            Sharpen5x5Type2,
            Smooth3x3Type1,
            Smooth3x3Type2,
            Smooth3x3Type3,
            Smooth5x5Type1,
            Smooth5x5Type2,
            IntenseSharpen,
            Laplacian3x3Type1,
            Laplacian5x5Type1,
            Laplacian3x3Type2,
            Laplacian5x5Type2,
            Gaussian3x3,
            Gaussian5x5Type1,
            Gaussian5x5Type2,
            LaplacianOfGaussian,
            GradientEast,
            GradientNorth,
            GradientNorthEast,
            GradientNorthWest,
            GradientSouth,
            GradientWest,
        }

        public static readonly Dictionary<Filter, (double[,] filterMatrix, double factor, int bias)> Filters = new Dictionary<Filter, (double[,] filterMatrix, double factor, int bias)>()
        {
            [Filter.Sharpen3x3Type1] = (new double[,] {
                    { -1, -1, -1, },
                    { -1,  9, -1, },
                    { -1, -1, -1, }, }, 1.0, 0),
            [Filter.Sharpen3x3Type2] = (new double[,] {
                    {  0, -1,  0, },
                    { -1,  5, -1, },
                    {  0, -1,  0, }, }, 1.0, 0),
            [Filter.Sharpen3x3Type3] = (new double[,] {
                    { 0,   -0.25, 0, },
                    {-0.25, 2,   -0.25 },
                    { 0,   -0.25, 0, }, }, 1.0, 0),
            [Filter.Sharpen3x3Type4] = (new double[,] {
                    { -0.25, -0.25, -0.25, },
                    { -0.25,  3,    -0.25, },
                    { -0.25, -0.25, -0.25, }, }, 1.0, 0),
            [Filter.Sharpen3x3Factor] = (new double[,] {
                    {  0, -2,  0, },
                    { -2, 11, -2, },
                    {  0, -2,  0, }, }, 1.0 / 3.0, 0),
            [Filter.Sharpen5x5Type1] = (new double[,] {
                    { -1, -1, -1, -1, -1, },
                    { -1,  2,  2,  2, -1, },
                    { -1,  2,  8,  2,  1, },
                    { -1,  2,  2,  2, -1, },
                    { -1, -1, -1, -1, -1, }, }, 1.0 / 8.0, 0),
            [Filter.Sharpen5x5Type2] = (new double[,] {
                    { -1, -3, -4, -3, -1, },
                    { -3,  0,  6,  0, -3, },
                    { -4,  6, 21,  6, -4, },
                    { -3,  0,  6,  0, -3, },
                    { -1, -3, -4, -3, -1, }, }, 1.0, 0),
            [Filter.Smooth3x3Type1] = (new double[,] {
                    {  1,  1,  1, },
                    {  1, 10,  1, },
                    {  1,  1,  1, }, }, 1.0 / 15, 0),
            [Filter.Smooth3x3Type2] = (new double[,] {
                    {  1,  2,  1, },
                    {  2,  4,  2, },
                    {  1,  2,  1, }, }, 1.0 / 11, 0),
            [Filter.Smooth3x3Type3] = (new double[,] {
                    {  1,  1,  1, },
                    {  1,  5,  1, },
                    {  1,  1,  1, }, }, 1.0 / 9, 0),
            [Filter.Smooth5x5Type1] = (new double[,] {
                    {  1,  4,  7,  4,  1, },
                    {  4, 16, 26, 16,  4, },
                    {  7, 26, 41, 26,  7, },
                    {  4, 16, 26, 16,  4, },
                    {  1,  4,  7,  4,  1, }, }, 1.0 / 180, 0),
            [Filter.Smooth5x5Type2] = (new double[,] {
                    {  1,  1,  1,  1,  1, },
                    {  1,  5,  5,  5,  1, },
                    {  1,  5, 44,  5,  1, },
                    {  1,  5,  5,  5,  1, },
                    {  1,  1,  1,  1,  1, }, }, 1.0 / 72, 0),
            [Filter.IntenseSharpen] = (new double[,] {
                    { 1,  1, 1, },
                    { 1, -7, 1, },
                    { 1,  1, 1, }, }, 1.0, 0),
            [Filter.Laplacian3x3Type1] = (new double[,] {
                    { -1, -1, -1, },
                    { -1,  8, -1, },
                    { -1, -1, -1, }, }, 1.0, 0),
            [Filter.Laplacian5x5Type1] = (new double[,] {
                    { -1, -1, -1, -1, -1, },
                    { -1, -1, -1, -1, -1, },
                    { -1, -1, 24, -1, -1, },
                    { -1, -1, -1, -1, -1, },
                    { -1, -1, -1, -1, -1 } }, 1.0, 0),
            [Filter.Laplacian3x3Type2] = (new double[,] {
                    {  0, -1,  0 },
                    { -1,  4, -1 },
                    {  0, -1,  0  }, }, 1.0, 0),
            [Filter.Laplacian5x5Type2] = (new double[,] {
                    {  0,  0, -1,  0,  0 },
                    {  1, -1, -2, -1,  0 },
                    { -1, -2, 17, -2, -1 },
                    {  1, -1, -2, -1,  0 },
                    {  1,  0, -1,  0,  0 },}, 1.0, 0),
            [Filter.Gaussian3x3] = (new double[,] {
                    { 1, 2, 1, },
                    { 2, 4, 2, },
                    { 1, 2, 1, } }, 1.0 / 16.0, 0),
            [Filter.Gaussian5x5Type1] = (new double[,] {
                    { 2, 04, 05, 04, 2 },
                    { 4, 09, 12, 09, 4 },
                    { 5, 12, 15, 12, 5 },
                    { 4, 09, 12, 09, 4 },
                    { 2, 04, 05, 04, 2 }, }, 1.0 / 159.0, 0),
            [Filter.Gaussian5x5Type2] = (new double[,] {
                    { 1,  4,  6,  4, 1 },
                    { 4, 16, 24, 16, 4 },
                    { 6, 24, 36, 24, 6 },
                    { 4, 16, 24, 16, 4 },
                    { 1,  4,  6,  4, 1 }, }, 1.0 / 256.0, 0),
            [Filter.LaplacianOfGaussian] = (new double[,] {
                    {  0,  0, -1,  0,  0 },
                    {  0, -1, -2, -1,  0 },
                    { -1, -2, 16, -2, -1 },
                    {  0, -1, -2, -1,  0 },
                    {  0,  0, -1,  0,  0 } }, 1.0, 0),
            [Filter.GradientEast] = (new double[,] {
                    {1, 0, 1 },
                    {2, 0, -2 },
                    {1, 0, -1 }, }, 1.0, 0),
            [Filter.GradientNorth] = (new double[,] {
                    { -1, -2, -1 },
                    {  0, 0, 0 },
                    {  1, 2, 1 }, }, 1.0, 0),
            [Filter.GradientNorthEast] = (new double[,] {
                    { 0, -1, -2 },
                    {  1, 0, -1 },
                    {  2, 1, 0  }, }, 1.0, 0),
            [Filter.GradientNorthWest] = (new double[,] {
                    { -2, -1, 0 },
                    {  -1, 0, 1 },
                    {  0, 1, 2  }, }, 1.0, 0),
            [Filter.GradientSouth] = (new double[,] {
                    { 1, 2, 1 },
                    {  0, 0, 0 },
                    {  -1, -2, -1 }, }, 1.0, 0),
            [Filter.GradientWest] = (new double[,] {
                    { 1, 2, 1 },
                    { 0, 0, 0 },
                    { -1, -2, -1  }, }, 1.0, 0),
        };

        public enum FilterXY
        {
            Sobel3x3,
            Prewitt3x3,
            Kirsch3x3,
        }

        public static readonly Dictionary<FilterXY, (double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias)> FilterXYs = new Dictionary<FilterXY, (double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias)>()
        {
            [FilterXY.Sobel3x3] = (
                new double[,] { //Horizontal
                    { -1, 0, 1, },
                    { -2, 0, 2, },
                    { -1, 0, 1, }, },
                new double[,] { //Vertical
                    {  1,  2,  1, },
                    {  0,  0,  0, },
                    { -1, -2, -1, }, }, 1.0, 0),
            [FilterXY.Prewitt3x3] = (
                new double[,] { //Horizontal
                    { -1, 0, 1, },
                    { -1, 0, 1, },
                    { -1, 0, 1, }, },
                new double[,] { //Vertical
                    {  1,  1,  1, },
                    {  0,  0,  0, },
                    { -1, -1, -1, }, }, 1.0, 0),
            [FilterXY.Kirsch3x3] = (
                new double[,] { //Horizontal
                    {  5,  5,  5, },
                    { -3,  0, -3, },
                    { -3, -3, -3, }, },
                new double[,] { //Vertical
                    { 5, -3, -3, },
                    { 5,  0, -3, },
                    { 5, -3, -3, }, }, 1.0, 0),
        };

        public static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] filterMatrix, double factor = 1, int bias = 0, bool grayscale = false)
        {
            if (sourceBitmap == null) return null;

            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            if (grayscale == true)
            {
                float rgb;
                for (int idx = 0; idx < pixelBuffer.Length; idx += 4)
                {
                    rgb = ((uint)pixelBuffer[idx + 2] * 38 + (uint)pixelBuffer[idx + 1] * 75 + (uint)pixelBuffer[idx] * 15) >> 7; //rgb = pixelBuffer[k] * 0.11f; rgb += pixelBuffer[k + 1] * 0.59f; rgb += pixelBuffer[k + 2] * 0.3f;
                    pixelBuffer[idx] = (byte)rgb;
                    pixelBuffer[idx + 1] = pixelBuffer[idx];
                    pixelBuffer[idx + 2] = pixelBuffer[idx];
                    //pixelBuffer[idx + 3] = pixelBuffer[idx + 3];
                }
            }

            double blue, green, red;
            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);
            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset, byteOffset;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = green = red = 0;
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    var c = Color.FromArgb(pixelBuffer[byteOffset + 3], pixelBuffer[byteOffset + 2], pixelBuffer[byteOffset + 1], pixelBuffer[byteOffset]);
                    if (c == Color.White) { } //Console.WriteLine(c);
                    else
                    {
                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                                //Console.WriteLine(string.Format("offsetX: {0}, offsetY: {1}, filterX: {2}, filterY: {3}, byteOffset: {4:X}, calcOffset: {5:X}, filterOffset: {6}", offsetX, offsetY, filterX, filterY, byteOffset, calcOffset, filterOffset));
                                blue += (double)(pixelBuffer[calcOffset]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                                green += (double)(pixelBuffer[calcOffset + 1]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                                red += (double)(pixelBuffer[calcOffset + 2]) * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            }
                        }

                        blue = factor * blue + bias;
                        green = factor * green + bias;
                        red = factor * red + bias;

                        if (blue > 255) blue = 255;
                        else if (blue < 0) blue = 0;

                        if (green > 255) green = 255;
                        else if (green < 0) green = 0;

                        if (red > 255) red = 255;
                        else if (red < 0) red = 0;
                    }

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = pixelBuffer[byteOffset + 3];
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.Tag = sourceBitmap.Tag;
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor = 1, int bias = 0, bool grayscale = false)
        {
            if (sourceBitmap == null) return null;
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            if (grayscale == true)
            {
                float rgb;
                for (int idx = 0; idx < pixelBuffer.Length; idx += 4)
                {
                    rgb = ((uint)pixelBuffer[idx + 2] * 38 + (uint)pixelBuffer[idx + 1] * 75 + (uint)pixelBuffer[idx] * 15) >> 7; //rgb = pixelBuffer[k] * 0.11f; rgb += pixelBuffer[k + 1] * 0.59f; rgb += pixelBuffer[k + 2] * 0.3f;
                    pixelBuffer[idx] = (byte)rgb;
                    pixelBuffer[idx + 1] = pixelBuffer[idx];
                    pixelBuffer[idx + 2] = pixelBuffer[idx];
                    //pixelBuffer[idx + 3] = 255;
                }
            }

            double blueX, greenX, redX, blueY, greenY, redY, blueTotal, greenTotal, redTotal;
            int filterOffset = 1, calcOffset, byteOffset;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blueX = greenX = redX = 0;
                    blueY = greenY = redY = 0;
                    blueTotal = greenTotal = redTotal = 0.0;
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);

                            blueX += (double)(pixelBuffer[calcOffset]) * xFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            greenX += (double)(pixelBuffer[calcOffset + 1]) * xFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            redX += (double)(pixelBuffer[calcOffset + 2]) * xFilterMatrix[filterY + filterOffset, filterX + filterOffset];

                            blueY += (double)(pixelBuffer[calcOffset]) * yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            greenY += (double)(pixelBuffer[calcOffset + 1]) * yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            redY += (double)(pixelBuffer[calcOffset + 2]) * yFilterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blueTotal = Math.Sqrt((blueX * blueX) + (blueY * blueY));
                    greenTotal = Math.Sqrt((greenX * greenX) + (greenY * greenY));
                    redTotal = Math.Sqrt((redX * redX) + (redY * redY));

                    if (blueTotal > 255) blueTotal = 255;
                    else if (blueTotal < 0) blueTotal = 0;

                    if (greenTotal > 255) greenTotal = 255;
                    else if (greenTotal < 0) greenTotal = 0;

                    if (redTotal > 255) redTotal = 255;
                    else if (redTotal < 0) redTotal = 0;

                    resultBuffer[byteOffset] = (byte)(blueTotal);
                    resultBuffer[byteOffset + 1] = (byte)(greenTotal);
                    resultBuffer[byteOffset + 2] = (byte)(redTotal);
                    resultBuffer[byteOffset + 3] = pixelBuffer[byteOffset + 3];
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        public static Bitmap ConvolutionXYFilter(Bitmap sourceBitmap, double[,] xFilterMatrix, double[,] yFilterMatrix, double factor, int bias)
        {
            if (xFilterMatrix == null) return sourceBitmap;

            sourceBitmap = yFilterMatrix == null ?
                BitmapFilter.ConvolutionFilter(sourceBitmap, xFilterMatrix, factor, bias) :
                BitmapFilter.ConvolutionFilter(sourceBitmap, xFilterMatrix, yFilterMatrix, factor, bias);
            //sourceBitmap.MakeTransparent(Color.Transparent);
            return sourceBitmap;
        }
    }
}
