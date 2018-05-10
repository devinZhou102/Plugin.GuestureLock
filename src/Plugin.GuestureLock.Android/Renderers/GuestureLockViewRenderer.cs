using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.GuestureLock.Control;
using Plugin.GuestureLock.Droid.Renderers;
using Plugin.GuestureLock.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;
using GuestureLockView = Plugin.GuestureLock.Control.GuestureLockView;

[assembly: ExportRenderer(typeof(GuestureLockView), typeof(GuestureLockViewRenderer))]
namespace Plugin.GuestureLock.Droid.Renderers
{
    public class GuestureLockViewRenderer : ViewRenderer<GuestureLockView, Android.Views.View>,IOnTouchListener
    {
        

        Paint paint;

        public GuestureLockViewRenderer(Context context) : base(context)
        {
        }

       // private double touch_x = 0;
        //private double touch_y = 0;
        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Move:
                    //touch_x = e.GetX();
                    //touch_y = e.GetY();
                    //Element.ProcessTouchEvent(touch_x, touch_y);
                    Element.ProcessTouchEvent(e.GetX(), e.GetY());
                    PostInvalidate();
                    break;
                case MotionEventActions.Up:
                    Element.GetCheckedIndex();
                    if (Element._CheckCompeleteDelegate != null)
                    {
                        Element._CheckCompeleteDelegate.Invoke(Element.indexList);
                    }
                    Element.Reset();
                    PostInvalidate();
                    break;
            }
            return true;
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int size = Element.pointList.Count;
            paint.StrokeWidth = 6;
            paint.AntiAlias = true;
            for (int i = 0; i < size; i++)//绘制元素点图
            {
                Vector2 item = Element.pointList.ElementAt(i);
                paint.Color = Android.Graphics.Color.Blue;
                paint.SetStyle(Paint.Style.Fill);//设置为实心
                canvas.DrawCircle(item.X, item.Y, Element.Circle_r, paint);
                paint.SetStyle(Paint.Style.Stroke);//设置为空心
                canvas.DrawCircle(item.X, item.Y, Element.Circle_R, paint);
            }
            size = Element.drawList.Count;
            for (int i = 0; i < size; i++)//绘制选中点图
            {
                Vector2 item = Element.drawList.ElementAt(i);
                paint.Color = Android.Graphics.Color.Red;
                paint.SetStyle(Paint.Style.Fill);//设置为实心
                canvas.DrawCircle(item.X, item.Y, Element.Circle_r, paint);
                if (i < size - 1)
                {
                    Vector2 item2 = Element.drawList.ElementAt(i + 1);
                    paint.Color = Android.Graphics.Color.Red;
                    canvas.DrawLine(item.X, item.Y, item2.X, item2.Y, paint);
                    paint.SetStyle(Paint.Style.Stroke);//设置为空心
                    canvas.DrawCircle(item.X, item.Y, Element.Circle_R, paint);
                }
            }

        }


        private void Initialize()
        {
            Element.Circle_r = DensityUtil.Dp2px(Context, 3f);
            Element.Circle_R = DensityUtil.Dp2px(Context, 20f);
            Element.Distance = DensityUtil.Dp2px(Context, 40f);
            Element.Length = 3 * Element.Circle_R * 2 + Element.Distance * 2;
            Element.ViewWidth = DensityUtil.Dp2px(Context, 320f);
            Element.ViewHight = DensityUtil.Dp2px(Context, 320f);

            Element.MyPadding = (Element.ViewWidth - Element.Length) / 2;
            Element.X_Zero = (int)Element.MyPadding + Element.Circle_R;
            Element.Y_Zero = (int)Element.MyPadding + Element.Circle_R;
            paint = new Paint();
            SetOnTouchListener(this);
            Element.InitPointList();
            PostInvalidate();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<GuestureLockView> e)
        {
            base.OnElementChanged(e);
            Control?.SetOnTouchListener(this);
            if (e.NewElement != null)
            {
                Initialize();
                PostInvalidate();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if(disposing && Element != null)
            {
                Element.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}