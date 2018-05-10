using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Plugin.GuestureLock.Droid.Utils
{
    public class DensityUtil
    {

        
        /**
         * dpתpx
         */
        public static int Dp2px(Context context, float dpVal)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip,
                    dpVal, context.Resources.DisplayMetrics);
        }

        /**
         * spתpx
         */
        public static int Sp2px(Context context, float spVal)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Sp,
                    spVal, context.Resources.DisplayMetrics);
        }

        /**
         * pxתdp
         */
        public static float Px2dp(Context context, float pxVal)
        {
            float scale = context.Resources.DisplayMetrics.Density;
            return (pxVal / scale);
        }

        /**
         * pxתsp
         */
        public static float Px2sp(Context context, float pxVal)
        {
            return (pxVal / context.Resources.DisplayMetrics.ScaledDensity);
        }
    }
}