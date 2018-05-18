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
using Plugin.GuestureLock.Droid.Utils;
using Plugin.GuestureLock.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(Plugin.GuestureLock.Droid.Service.ImpDroidDensityConvertService))]
namespace Plugin.GuestureLock.Droid.Service
{
    public class ImpDroidDensityConvertService : IDensityConvertService
    {
        public int Dp2Px(int value)
        {
            return DensityUtil.Dp2px(Android.App.Application.Context, value);
        }
    }
}