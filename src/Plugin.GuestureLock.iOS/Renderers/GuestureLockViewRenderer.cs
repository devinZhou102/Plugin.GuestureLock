using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using Plugin.GuestureLock.Control;
using Plugin.GuestureLock.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GuestureLockView = Plugin.GuestureLock.Control.GuestureLockView;

[assembly: ExportRenderer(typeof(GuestureLockView), typeof(GuestureLockViewRenderer))]
namespace Plugin.GuestureLock.iOS.Renderers
{
    public class GuestureLockViewRenderer : ViewRenderer<GuestureLockView, UIView>
    {

        CGContext cGContext;
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            cGContext = UIGraphics.GetCurrentContext();
            int size = Element.pointList.Count;

            for (int i = 0; i < size; i++)//绘制元素点图
            {
                Vector2 item = Element.pointList.ElementAt(i);

                cGContext.SetFillColor(UIColor.Blue.CGColor);
                cGContext.AddEllipseInRect(new CGRect(item.X - Element.Circle_r, item.Y - Element.Circle_r, Element.Circle_r * 2, Element.Circle_r * 2));
                cGContext.DrawPath(CGPathDrawingMode.Fill);

                cGContext.SetStrokeColor(UIColor.Blue.CGColor);
                cGContext.SetLineWidth(2);
                cGContext.AddEllipseInRect(new CGRect(item.X - Element.Circle_R, item.Y - Element.Circle_R, Element.Circle_R * 2, Element.Circle_R * 2));
                cGContext.DrawPath(CGPathDrawingMode.Stroke);
            }
            size = Element.drawList.Count;
            for (int i = 0; i < size; i++)//绘制选中点图
            {
                Vector2 item = Element.drawList.ElementAt(i);


                cGContext.SetFillColor(UIColor.Red.CGColor);
                cGContext.AddEllipseInRect(new CGRect(item.X - Element.Circle_r, item.Y - Element.Circle_r, Element.Circle_r * 2, Element.Circle_r * 2));
                cGContext.DrawPath(CGPathDrawingMode.Fill);
                if (i < size - 1)
                {
                    Vector2 item2 = Element.drawList.ElementAt(i + 1);

                    cGContext.SetStrokeColor(UIColor.Red.CGColor);
                    cGContext.MoveTo(item.X, item.Y);
                    cGContext.AddLineToPoint(item2.X, item2.Y);
                    cGContext.DrawPath(CGPathDrawingMode.Stroke);

                    cGContext.SetStrokeColor(UIColor.Red.CGColor);
                    cGContext.SetLineWidth(2);
                    cGContext.AddEllipseInRect(new CGRect(item.X - Element.Circle_R, item.Y - Element.Circle_R, Element.Circle_R * 2, Element.Circle_R * 2));
                    cGContext.DrawPath(CGPathDrawingMode.Stroke);
                }
            }

        }


        void Initialize()
        {
            Element.Length = 3 * Element.Circle_R * 2 + Element.Distance * 2;
            Element.ViewWidth = 320f;
            Element.ViewHight = 320f;

            Element.MyPadding = (Element.ViewWidth - Element.Length) / 2;
            Element.X_Zero = (int)Element.MyPadding + Element.Circle_R;
            Element.Y_Zero = (int)Element.MyPadding + Element.Circle_R;
            Element.InitPointList();
        }

        

        protected override void OnElementChanged(ElementChangedEventArgs<GuestureLockView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Initialize();
                SetNeedsDisplay();
            }

        }


        //private double touch_x = 0;
        //private double touch_y = 0;

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                //var touch_x = touch.LocationInView(this).X;
                //var touch_y = touch.LocationInView(this).Y;
                Element.ProcessTouchEvent(touch.LocationInView(this).X, touch.LocationInView(this).Y);
                SetNeedsDisplay();
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            if (touches.AnyObject is UITouch touch)
            {
                //var touch_x = touch.LocationInView(this).X;
                //var touch_y = touch.LocationInView(this).Y;
                Element.ProcessTouchEvent(touch.LocationInView(this).X, touch.LocationInView(this).Y);
                SetNeedsDisplay();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            Element.GetCheckedIndex();
            if (Element._CheckCompeleteDelegate != null)
            {
                Element._CheckCompeleteDelegate.Invoke(Element.indexList);
            }
            Element.Reset();
            SetNeedsDisplay();
        }
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            Element.Reset();
            SetNeedsDisplay();
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