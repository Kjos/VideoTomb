using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideoTomb.Core.Util
{
    class FileUtil
    {
        public static string[] SupportedVideos = new string[] { ".mp4", ".mov", ".webm" };

        public static string[] SupportedImages = new string[] { ".png", ".jpeg", ".bmp" };

        public static void CreateDirectory(string dir, bool empty = false)
        {
            if (Directory.Exists(dir))
            {
                if (empty)
                {
                    Directory.Delete(dir, true);
                    while (Directory.Exists(dir)) Thread.Sleep(100);
                }
                else
                {
                    return;
                }
            }

            Directory.CreateDirectory(dir);
            while (!Directory.Exists(dir)) Thread.Sleep(100);
        }

        public static void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                while (File.Exists(path)) Thread.Sleep(100);
            }
        }

        public static bool IsVideo(string path)
        {
            path = path.ToLower();
            foreach (string end in SupportedVideos)
            {
                if (path.EndsWith(end))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsImage(string path)
        {
            path = path.ToLower();
            foreach (string end in SupportedImages)
            {
                if (path.EndsWith(end))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
