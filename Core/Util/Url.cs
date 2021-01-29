using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VideoTomb.Core.Processing;

namespace VideoTomb.Core.Util
{
    class Url
    {
        public static string GenerateImageUrl(string videoUrl, string imageMaskUrl, int adDelay = 0)
        {
            string version = Parameters.EncodeValue(Constants.Version, 1);
            string delayStr = adDelay > 0 ? Parameters.EncodeValue(adDelay, 1) : "";
            string url = "https://www.videotomb.com/v/#" + version + delayStr + "&";

            string vUrl = ShrinkUrl(videoUrl);
            if (string.IsNullOrEmpty(imageMaskUrl))
            {
                return vUrl;
            }

            string mUrl = ShrinkUrl(imageMaskUrl);

            url += vUrl + "&" + mUrl;
            return url;
        }

        public static string GenerateHashUrl(string videoUrl, string noiseHash, int adDelay = 0)
        {
            string version = Parameters.EncodeValue(Constants.Version, 1);
            string delayStr = adDelay > 0 ? Parameters.EncodeValue(adDelay, 1) : "";
            string url = "https://www.videotomb.com/v/#" + version + delayStr + "&";

            string vUrl = ShrinkUrl(videoUrl);
            string mUrl = "hash=" + noiseHash;

            url += vUrl + "&" + mUrl;
            return url;
        }

        public static string ShrinkUrl(string url)
        {
            string[] urls = new string[] {
                "../",
                "https://",
                "http://",
                "www.",
                "player.",
                // Allow iframe from here
                "i.imgur.com",
                "twitch.tv",
                "clips.twitch.tv",
                "bilibili.com",
                "vimeo.com",
                "facebook.com",
                "flickr.com",
                "instagram.com"
            };

            string prefix = "";
            bool found = true;
            while (found) {
                found = false;

                for (int i = urls.Length - 1; i >= 0; i--)
                {
                    if (url.StartsWith(urls[i]))
                    {
                        url = url.Substring(urls[i].Length);
                        prefix += Parameters.EncodeValue(i, 1);
                        found = true;
                    }
                }
            }

            return prefix + "=" + HttpUtility.UrlEncode(url);
        }
    }
}
