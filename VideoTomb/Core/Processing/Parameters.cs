using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoTomb.Core.Processing
{
    class Parameters
    {
        public String MaskPath
        {
            get;
        }

        public String NoiseHash {
            get; private set;
        }

        public bool IsNoise
        {
            get;
        }

        public int Resolution
        {
            get;
        }

        public bool ShortSample
        {
            get;
        }

        public String OutputDir
        {
            get;
        }

        public int MinHeight
        {
            get;
        }

        public bool VertLines
        {
            get;
        }

        public bool Normalize
        {
            get;
        }

        public int Blur
        {
            get;
        }

        public bool RemoveSound
        {
            get;
        }

        public bool RandomLines
        {
            get;
        }

        public int BitrateMultiplier
        {
            get;
        }

        public bool Preview
        {
            get;
        }

        public string TimeRanges
        {
            get;
        }

        public TextBox UpdateNoiseTextBox
        {
            get; set;
        }

        public bool EncryptAudio
        {
            get;
        }

        public float CutStart
        {
            get;
        }

        public Parameters(String maskPath, int resolution, int minHeight, bool vertLines, bool shortSample, bool normalize, int blur, bool removeSound, bool randomLines, int bitrateMult, bool preview, string timeRanges, bool encryptAudio, float cutStart, string outputDir)
        {
            this.IsNoise = false;
            this.NoiseHash = Util.Util.RandomNoiseHash();
            this.MaskPath = maskPath;

            this.Resolution = resolution * 125 / 100 + 1;
            this.ShortSample = shortSample;
            this.VertLines = vertLines;
            this.MinHeight = minHeight;
            this.Normalize = normalize;
            this.Blur = blur;
            this.RemoveSound = removeSound;
            this.RandomLines = randomLines;
            this.BitrateMultiplier = bitrateMult;
            this.Preview = preview;
            this.TimeRanges = timeRanges;
            this.EncryptAudio = encryptAudio;
            this.CutStart = cutStart;

            if (!outputDir.EndsWith("\\") && !outputDir.EndsWith("/"))
            {
                outputDir += "\\";
            }
            this.OutputDir = outputDir;
        }

        public Parameters(int resolution, int minHeight, bool vertLines, bool shortSample, bool normalize, int blur, bool removeSound, bool randomLines, int bitrateMult, bool preview, string timeRanges, bool encryptAudio, float cutStart, string outputDir)
        {
            this.IsNoise = true;

            this.Resolution = resolution * 125 / 100 + 1;
            this.ShortSample = shortSample;
            this.VertLines = vertLines;
            this.MinHeight = minHeight;
            this.Normalize = normalize;
            this.Blur = blur;
            this.RemoveSound = removeSound;
            this.RandomLines = randomLines;
            this.BitrateMultiplier = bitrateMult;
            this.Preview = preview;
            this.TimeRanges = timeRanges;
            this.EncryptAudio = encryptAudio;
            this.CutStart = cutStart;

            if (!outputDir.EndsWith("\\") && !outputDir.EndsWith("/"))
            {
                outputDir += "\\";
            }
            this.OutputDir = outputDir;
        }

        public void CreateNoiseHash(int videoWidth, int videoHeight, int columnWidth)
        {
            string hash = Util.Util.RandomNoiseHash(Constants.HashLength);
            int wHash = DecodeValue(hash.Substring(0, 3));
            int hHash = DecodeValue(hash.Substring(3, 3));
            int cHash = DecodeValue(hash.Substring(6, 2));
            int resHash = DecodeValue(hash.Substring(8, 2));
            int vHash = DecodeValue(hash.Substring(10, 1));
            int rHash = DecodeValue(hash.Substring(11, 1));

            wHash -= wHash % 16384;
            wHash += videoWidth;

            hHash -= hHash % 16384;
            hHash += videoHeight;

            cHash -= cHash % 100;
            cHash += columnWidth;

            resHash -= resHash % 128;
            resHash += this.Resolution;

            vHash -= vHash % 2;
            vHash += this.VertLines ? 1 : 0;

            rHash -= rHash % 2;
            rHash += this.RandomLines ? 1 : 0;

            string newHash = EncodeValue(wHash, 3);
            newHash += EncodeValue(hHash, 3);
            newHash += EncodeValue(cHash, 2);
            newHash += EncodeValue(resHash, 2);
            newHash += EncodeValue(vHash, 1);
            newHash += EncodeValue(rHash, 1);

            this.NoiseHash = newHash + hash.Substring(12);
            this.UpdateNoiseTextBox.Invoke((MethodInvoker)delegate ()
            {
                this.UpdateNoiseTextBox.Text = this.NoiseHash;
            });
        }

        public static int DecodeValue(string str)
        {
            int sum = 0;
            int m = 1;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                int value = 0;
                if (char.IsDigit(ch))
                {
                    value = ch - '0';
                } else if (char.IsLower(ch))
                {
                    value = ch - 'a';
                    value += 10;
                } else
                {
                    value = ch - 'A';
                    value += 36;
                }

                sum += value * m;
                m *= 62;
            }
            return sum;
        }

        public static string EncodeValue(int value, int length = 1)
        {
            string str = "";

            do
            {
                char ch;
                int rest = value % 62;
                if (rest < 10)
                {
                    ch = (char)((int)'0' + rest);
                }
                else if (rest < 36)
                {
                    ch = (char)((int)'a' + (rest - 10));
                }
                else
                {
                    ch = (char)((int)'A' + (rest - 36));
                }
                str += ch;
                value /= 62;
                length--;
            } while (length != 0);

            return str;
        }
    }
}
