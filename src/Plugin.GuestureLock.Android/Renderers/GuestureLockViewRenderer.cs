using Android.Content;
using Android.Graphics;
using Android.Views;
using Plugin.GuestureLock.Droid.Renderers;
using Plugin.GuestureLock.Droid.Utils;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;
using GuestureLockView = Plugin.GuestureLock.Control.GuestureLockView;
using Point = Xamarin.Forms.Point;

[assembly: ExportRenderer(typeof(GuestureLockView), typeof(GuestureLockViewRenderer))]
namespace Plugin.GuestureLock.Droid.Renderers
{
    public class GuestureLockViewRenderer : ViewRenderer<GuestureLockView, Android.Views.View>,IOnTouchListener
    {
        

        Paint paint;

        public GuestureLockViewRenderer(Context context) : base(context)
        {
        }
        
        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Move:
                    //System.Diagnostics.Debug.WriteLine("OnTouch  ===== " + e.Action);
                    Element.ProcessTouchEvent(e.GetX(), e.GetY());
                    PostInvalidate();
                    break;
                case MotionEventActions.Up:
                    Element.Complete();
                    PostInvalidate();
                    break;
                case MotionEventActions.Cancel:
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
                Point item = Element.pointList.ElementAt(i);
                paint.Color = Android.Graphics.Color.Blue;
                paint.SetStyle(Paint.Style.Fill);//设置为实心
                canvas.DrawCircle((int)item.X, (int)item.Y, Element.Circle_r, paint);
                paint.SetStyle(Paint.Style.Stroke);//设置为空心
                canvas.DrawCircle((int)item.X, (int)item.Y, Element.Circle_R, paint);
            }
            size = Element.drawList.Count;
            for (int i = 0; i < size; i++)//绘制选中点图
            {
                Point item = Element.drawList.ElementAt(i);
                paint.Color = Android.Graphics.Color.Red;
                paint.SetStyle(Paint.Style.Fill);//设置为实心
                canvas.DrawCircle((int)item.X, (int)item.Y, Element.Circle_r, paint);
                if (i < size - 1)
                {
                    Point item2 = Element.drawList.ElementAt(i + 1);
                    paint.Color = Android.Graphics.Color.Red;
                    canvas.DrawLine((int)item.X, (int)item.Y, (int)item2.X, (int)item2.Y, paint);
                    paint.SetStyle(Paint.Style.Stroke);//设置为空心
                    canvas.DrawCircle((int)item.X,(int)item.Y, Element.Circle_R, paint);
                }
            }

        }


        private void Initialize()
        {
            Element.Circle_r = DensityUtil.Dp2px(Context, 3f);
            Element.Circle_R = DensityUtil.Dp2px(Context, 20f);
            Element.Distance = DensityUtil.Dp2px(Context, 40f);
            var Length = 3 * Element.Circle_R * 2 + Element.Distance * 2;
            int MyWidth = DensityUtil.Dp2px(Context, (float)Element.WidthRequest);
            int MyPadding = (int)((MyWidth - Length) / 2);
            Element.X_Zero = MyPadding + Element.Circle_R;
            Element.Y_Zero = MyPadding + Element.Circle_R;
            paint = new Paint();
            SetOnTouchListener(this);
            Element.InitPointList();
            PostInvalidate();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<GuestureLockView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Initialize();
                PostInvalidate();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Element != null)
            {
                Element.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}