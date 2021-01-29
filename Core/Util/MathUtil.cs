using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTomb.Core.Util
{
    class MathUtil
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees / 180f * (float)Math.PI;
        }

        public const float PI = (float)Math.PI;

        public static float Clamp(float v, float lo, float hi)
        {
            return Math.Min(hi, Math.Max(lo, v));
        }

        public static int Clamp(int v, int lo, int hi)
        {
            return Math.Min(hi, Math.Max(lo, v));
        }
    }
}
