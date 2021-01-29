using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VideoTomb.Core.Util
{
    class ImageUtil
    {
        public static Bitmap NoiseBitmap(int width, int height, String hash)
        {
            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int p = 0;
                uint h = 0;
                int skip = data.Width * data.Height / Constants.HashSkip;
                int skipCount = 0;
                for (int y = 0; y < height; y++)
                {
                    byte* ptr = (byte*)data.Scan0.ToPointer() + data.Stride * y;
                    for (int x = 0; x < width; x++)
                    {
                        for (int c = 0; c < 3; c++, p++)
                        {
                            if (skipCount == 0)
                            {
                                h = Util.HashFnv32u(hash + p);
                                skipCount = skip;
                            }
                            uint rand = h % 255;
                            *ptr = (byte)rand;
                            ptr++;
                        }
                    }
                }
            }

            bmp.UnlockBits(data);

            return bmp;
        }

        public static void BurnResolution(BitmapData data, int resolution)
        {
            unsafe
            {
                byte* ptr = (byte*)data.Scan0.ToPointer();
                for (int i = 0; i < 3; i++, ptr++)
                {
                    *ptr = (byte)(255 - resolution);
                }
            }
        }

        private const string SecretKey = "lk309fl";
        public static void EncryptMask(BitmapData data)
        {
            int width = data.Width;
            int height = data.Height;

            unsafe
            {
                int p = 0;
                uint h = 0;
                int skip = data.Width * data.Height / Constants.HashSkip;
                int skipCount = 0;
                for (int y = 0; y < height; y++)
                {
                    byte* ptr = (byte*)data.Scan0.ToPointer() + data.Stride * y;
                    for (int x = 0; x < width; x++)
                    {
                        for (int c = 0; c < 3; c++, p++)
                        {
                            if (skipCount == 0)
                            {
                                h = Util.HashFnv32u(SecretKey + p + width + height);
                                skipCount = skip;
                            }
                            bool rand = h % 2 == 1;
                            int val;
                            if (rand)
                            {
                                val = 255 - (*ptr & 0xff);
                            } else
                            {
                                val = *ptr & 0xff;
                            }
                            ptr++;
                        }
                    }
                }
            }
        }

        public static int[] GetDataFromBitmap(Bitmap frame)
        {
            int w = frame.Width;
            int h = frame.Height;

            int len = w * h;

            int[] output = new int[len * 3];

            int p = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color pixel = frame.GetPixel(x, y);
                    output[p++] = pixel.R & 0xff;
                    output[p++] = pixel.G & 0xff;
                    output[p++] = pixel.B & 0xff;
                }
            }

            return output;
        }

        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        public static void RemovePrecision(BitmapData data, int bits = 4)
        {
            int w = data.Width;
            int h = data.Height;

            unsafe
            {
                IntPtr iptr = data.Scan0;
                int stride = data.Stride;

                byte* ptr = (byte*)iptr.ToPointer();

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (int channel = 0; channel < 3; channel++)
                        {
                            int p = y * stride + x * 3;
                            p += channel;
                            int val = ptr[p] & 0xff;
                            val >>= bits;
                            val <<= bits;
                            ptr[p] = (byte)val;
                        }
                    }
                }
            }
        }

        public static void Normalize(BitmapData data)
        {
            int w = data.Width;
            int h = data.Height;

            unsafe
            {
                for (int channel = 0; channel < 3; channel++)
                {
                    IntPtr iptr = data.Scan0;
                    int stride = data.Stride;

                    byte* ptr = (byte*)iptr.ToPointer();

                    int max = 0;
                    int min = 255;
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            int p = y * stride + x * 3;
                            p += channel;
                            int val = ptr[p] & 0xff;
                            if (val > max)
                            {
                                max = val;
                            }
                            if (val < min)
                            {
                                min = val;
                            }
                        }
                    }

                    int diff = max - min;
                    diff = Math.Max(1, diff);
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            int p = y * stride + x * 3;
                            p += channel;
                            int val = ptr[p] & 0xff;
                            val = (val - min) * 255 / diff;
                            ptr[p] = (byte)val;
                        }
                    }
                }
            }
        }

        public static int Median(BitmapData data, int channel)
        {
            int w = data.Width;
            int h = data.Height;

            int[] array = new int[w * h];

            unsafe
            {
                IntPtr iptr = data.Scan0;
                int stride = data.Stride;

                byte* ptr = (byte*)iptr.ToPointer();

                int blur = 3;
                int divide = blur * 2 + 1;
                divide *= divide;

                int p = 0;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++, p++)
                    {
                        int p2 = y * stride + x * 3;
                        p2 += channel;
                        int val = ptr[p2] & 0xff;
                        array[p] = val;
                    }
                }
            }

            Array.Sort(array);

            return array[array.Length / 2];
        }

        public static void Blur(BitmapData data, int blur = 2)
        {
            if (blur == 0) return;

            int w = data.Width;
            int h = data.Height;

            unsafe
            {
                IntPtr iptr = data.Scan0;
                int stride = data.Stride;

                byte* ptr = (byte*)iptr.ToPointer();

                int divide = blur * 2 + 1;

                for (int channel = 0; channel < 3; channel++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            int sum = 0;
                            int ny = y;
                            for (int dx = -blur; dx <= blur; dx++)
                            {
                                int nx = Math.Max(0, Math.Min(w - 1, x + dx));

                                int p = ny * stride + nx * 3;
                                p += channel;
                                sum += ptr[p] & 0xff;
                            }
                            ptr[y * stride + x * 3 + channel] = (byte)(sum / divide);
                        }
                    }
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            int sum = 0;
                            for (int dy = -blur; dy <= blur; dy++)
                            {
                                int ny = y + dy;
                                ny = Math.Max(0, Math.Min(h - 1, ny));

                                int nx = x;

                                int p = ny * stride + nx * 3;
                                p += channel;
                                sum += ptr[p] & 0xff;
                            }
                            ptr[y * stride + x * 3 + channel] = (byte)(sum / divide);
                        }
                    }
                }
            }
        }
    }
}
