using System;
using System.Collections;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace VideoTomb.Core.Util
{
    class Util
    {
        private static readonly Random Rand = new Random();

        public static string RandomNoiseHash(int length = 8)
        {
            string h = "";

            for (int i = 0; i < length; i++)
            {
                int r = Math.Abs(Rand.Next()) % (26 * 2 + 10);
                if (r < 10)
                {
                    h += r;
                    continue;
                }

                r -= 10;

                int n;
                if (r < 26)
                {
                    n = (int)'a' + r;
                } else
                {
                    n = (int)'A' + (r - 26);
                }
                char c = (char)n;
                h += c;
            }
            return h;
        }

        private static readonly UInt32 FNV_OFFSET_32 = 0x811c9dc5;   // 2166136261
        private static readonly UInt32 FNV_PRIME_32 = 0x1000193;     // 16777619

        // Unsigned 32bit integer FNV-1a
        public static UInt32 HashFnv32u(string s)
        {
            // byte[] arr = Encoding.UTF8.GetBytes(s);      // 8 bit expanded unicode array
            char[] arr = s.ToCharArray();                   // 16 bit unicode is native .net 

            UInt32 hash = FNV_OFFSET_32;
            for (var i = 0; i < s.Length; i++)
            {
                // Strips unicode bits, only the lower 8 bits of the values are used
                hash = hash ^ unchecked((byte)(arr[i] & 0xFF));
                hash = hash * FNV_PRIME_32;
            }
            return hash;
        }

        public static void Preview(string outputDir, string video, string maskImage, bool isNoise)
        {
            IEnumerable files = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/player");

            string playerDir = outputDir + "player\\";
            Directory.CreateDirectory(playerDir);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                File.Copy(file, playerDir + name);
            }

            string html = playerDir + Path.GetFileNameWithoutExtension(video) + ".html";
            string content = "<html><head><meta http-equiv=\"Refresh\" content=\"0; url=index.html#0&" + Url.ShrinkUrl("../" + Path.GetFileName(video));
            if (isNoise)
            {
                content += "&hash=" + maskImage;
            } else
            {
                content += "&" + Url.ShrinkUrl("../" + Path.GetFileName(maskImage));
            }
            content += "\"/></head></html>";
            File.WriteAllText(html, content);

            System.Diagnostics.Process.Start(html);
        }

        public static string CreateOutputDir(string inputPath, string outputPath, bool isDir)
        {
            string name = isDir ? Path.GetDirectoryName(inputPath) : Path.GetFileNameWithoutExtension(inputPath);
            string outputDir;
            int cnt = 1;
            do
            {
                outputDir = outputPath + "\\" + name;
                if (cnt > 1)
                {
                    outputDir += cnt;
                }
                cnt++;
            } while (Directory.Exists(outputDir));
            Directory.CreateDirectory(outputDir);

            return outputDir;
        }

        public static long[][] ParseRanges(string input, float framerate)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            string[] split = input.Split(',');
            long[][] timeRanges = new long[split.Length][];
            for (int i = 0; i < timeRanges.Length; i++)
            {
                long[] range = new long[2];
                string[] rangeStr = split[i].Split('-');
                if (rangeStr.Length != 2)
                {
                    throw new ArgumentException("Timerange formatting error.");
                }

                if (float.TryParse(rangeStr[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float res) && float.TryParse(rangeStr[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float res2))
                {
                    range[0] = (long)(res * framerate);
                    range[1] = (long)(res2 * framerate);
                }
                else
                {
                    throw new ArgumentException("Timerange number formatting error.");
                }

                timeRanges[i] = range;
            }

            return timeRanges;
        }

        public static bool InRange(long[][] ranges, long stamp)
        {
            if (ranges == null)
            {
                return true;
            }

            for (int i = 0; i < ranges.Length; i++)
            {
                if (stamp >= ranges[i][0] && stamp <= ranges[i][1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
