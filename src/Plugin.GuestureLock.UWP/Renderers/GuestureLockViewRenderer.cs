using Microsoft.Graphics.Canvas.UI.Xaml;
using Plugin.GuestureLock.UWP.Renderers;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using GuestureLockView = Plugin.GuestureLock.Control.GuestureLockView;

[assembly: ExportRenderer(typeof(GuestureLockView), typeof(GuestureLockViewRenderer))]
namespace Plugin.GuestureLock.UWP.Renderers
{
    public class GuestureLockViewRenderer : ViewRenderer<GuestureLockView, CanvasControl>
    {

        public GuestureLockViewRenderer() { }

        void Initialize()
        {
             var Length = 3 * Element.Circle_R * 2 + Element.Distance * 2;
            var MyPadding = (int)((Element.WidthRequest - Length) / 2);
            Element.X_Zero = MyPadding + Element.Circle_R;
            Element.Y_Zero = MyPadding + Element.Circle_R;
            Element.InitPointList();
        }


        protected override void OnElementChanged(ElementChangedEventArgs<GuestureLockView> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement == null)
            {
                SetNativeControl(new CanvasControl());
            }

            if (Control != null)
            {
                Control.ManipulationMode = ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                Control.ManipulationDelta += Control_ManipulationDelta;
                Control.ManipulationStarted += Control_ManipulationStarted;
                Control.ManipulationCompleted += Control_ManipulationCompleted;
                Control.Draw += Control_Draw;
                Control.Invalidate();
            }
            if (e.NewElement != null)
            {
                Initialize();
            }

        }


        private Vector2 GetVector2(Point v2)
        {
            Vector2 item = new Vector2
            {
                X = (float)v2.X,
                Y = (float)v2.Y
            };
            return item;
        }

        private void Control_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int size = Element.pointList.Count;
            for (int i = 0; i < size; i++)//绘制元素点图
            {
                Vector2 item = GetVector2(Element.pointList.ElementAt(i));
                args.DrawingSession.DrawCircle(item, Element.Circle_R, Colors.Blue, 3);
                args.DrawingSession.DrawCircle(item, 0, Colors.Blue, 6);
            }
            size = Element.drawList.Count;
            for (int i = 0; i < size; i++)//绘制选中点图
            {
                Vector2 item = GetVector2(Element.drawList.ElementAt(i));
                args.DrawingSession.DrawCircle(item, 0, Colors.Red, 6);
                if (i < size - 1)
                {
                    Vector2 item2 = GetVector2(Element.drawList.ElementAt(i + 1));
                    args.DrawingSession.DrawLine(item, item2, Colors.Red, 3);
                    args.DrawingSession.DrawCircle(item, Element.Circle_R, Colors.Red, 3);
                }
            }
        }

        private void Control_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            Process(e.Position.X, e.Position.Y);
        }

        private void Control_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            Process(e.Position.X, e.Position.Y);
        }

        private void Process(double x,double y)
        {
            Element.ProcessTouchEvent(x, y);
            Control?.Invalidate();
        }

        private void Control_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            Element.Complete();
            Control?.Invalidate();
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
