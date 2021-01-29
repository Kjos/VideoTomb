using VideoTomb.Core.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VideoTomb.Core.Processing
{
    class ImageProcessor
    {
        public static readonly ImageProcessor None = new ImageProcessor();

        private int width, height;

        private int resolution = 16;
        private bool vertLines;
        private int[] lineData;

        private readonly int[] maskData;

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public string MaskImage
        {
            get;
        }

        private void Clamp(BitmapData data)
        {
            int w = data.Width;
            int h = data.Height;

            unsafe
            {
                IntPtr iptr = data.Scan0;
                int stride = data.Stride;

                int p = 0;
                for (int y = 0; y < h; y++)
                {
                    byte* ptr = (byte*)iptr.ToPointer() + y * stride;

                    for (int x = 0; x < w; x++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            byte val = *ptr;
                            int ival = (int)val & 0xff;

                            ival = ival * (255 - this.resolution * 2) / 255 + this.resolution;

                            int type = this.GetType(x, y, c);
                            ival += type;
                            if (type > 0)
                            {
                                ival = Math.Min(255 - this.resolution, ival);
                            } else
                            {
                                ival = Math.Max(this.resolution, ival);
                            }

                            *ptr = (byte)ival;
                            ptr++;
                        }
                    }
                }
            }
        }

        private ImageProcessor()
        {

        }

        public ImageProcessor(Parameters descr, int width, int height)
        {
            this.resolution = descr.Resolution;
            this.width = width;
            this.height = height;

            int columnWidth = Math.Max(1, height * 2 / descr.MinHeight) * 3;

            Bitmap mask;
            if (descr.IsNoise)
            {
                descr.CreateNoiseHash(width, height, columnWidth);
                mask = ImageUtil.NoiseBitmap(width, height, descr.NoiseHash);
            }
            else
            {
                mask = new Bitmap(descr.MaskPath);
                if (mask.Width != width || mask.Height != height)
                {
                    mask = ImageUtil.ResizeBitmap(mask, width, height);
                }
            }

            this.CreateLineData(descr, columnWidth);

            {
                BitmapData data = mask.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                ImageUtil.Blur(data, descr.IsNoise ? Constants.NoiseBlur : descr.Blur);
                if (descr.Normalize && !descr.IsNoise)
                {
                    ImageUtil.Normalize(data);
                }

                Clamp(data);

                mask.UnlockBits(data);
            }

            this.maskData = ImageUtil.GetDataFromBitmap(mask);

            if (descr.IsNoise)
            {
                File.WriteAllText(descr.OutputDir + "Hash.txt", descr.NoiseHash);
            }

            {
                BitmapData data = mask.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                ImageUtil.EncryptMask(data);
                ImageUtil.BurnResolution(data, descr.Resolution);
                this.MaskImage = descr.OutputDir + "Mask.png";
                mask.Save(this.MaskImage);
                mask.UnlockBits(data);
            }

            mask.Dispose();
        }

        private void CreateLineData(Parameters descr, int columnWidth)
        {
            this.vertLines = descr.VertLines;
            this.lineData = new int[Math.Max(width, height) * 3];

            this.lineData[0] = (int)(Util.Util.HashFnv32u(descr.NoiseHash) % this.resolution) - this.resolution / 2;
            if (!descr.RandomLines)
            {
                this.lineData[0] = this.lineData[0] >= 0 ? this.resolution / 2 : -this.resolution / 2;
            }

            int len = 0;
            for (int i = 1; i < this.lineData.Length; i++)
            {
                if (len > 0)
                {
                    this.lineData[i] = this.lineData[i - 1];
                    len--;
                    continue;
                }

                if (descr.RandomLines)
                {
                    uint rand = Util.Util.HashFnv32u(descr.NoiseHash + i);
                    int sign = this.lineData[i - 1] >= 0 ? -1 : 1;
                    this.lineData[i] = (int)(rand % this.resolution) * sign;

                    len = (int)(rand % columnWidth) + columnWidth;
                }
                else
                {
                    this.lineData[i] = -this.lineData[i - 1];

                    len = columnWidth;
                }
                len--;
            }
        }

        private byte GetVal(int x, int y, int c, int crypt, int color)
        {
            bool type = this.GetType(x, y, c) > 0;
            int val = color * this.resolution / 255;
            return (byte)(type ? Math.Abs(val - crypt) : crypt + val);
        }

        private int GetType(int x, int y, int c)
        {
            int type;
            if (this.vertLines)
            {
                type = this.lineData[x * 3 + c];
            }
            else
            {
                type = this.lineData[y * 3 + c];
            }
            return type;
        }

        public void Encrypt(BitmapData src, BitmapData dst, int frameId)
        {
            int[] cryp = this.maskData;

            int w = src.Width;
            int h = src.Height;

            unsafe
            {
                IntPtr isptr = src.Scan0;
                IntPtr idptr = dst.Scan0;
                int stride = src.Stride;

                int p = 0;
                int[] crp = new int[3];
                for (int y = 0; y < h; y++)
                {
                    byte* sptr = (byte*)isptr.ToPointer() + y * stride;
                    byte* dptr = (byte*)idptr.ToPointer() + y * stride;

                    for (int x = 0; x < w; x++)
                    {
                        crp[2] = cryp[p++];
                        crp[1] = cryp[p++];
                        crp[0] = cryp[p++];

                        int color;
                        for (int c = 0; c < 3; c++)
                        {
                            color = *sptr;
                            *dptr = this.GetVal(x, y, c, crp[c], color);
                            dptr++;
                            sptr++;
                        }
                    }
                }
            }
        }
    }
}
