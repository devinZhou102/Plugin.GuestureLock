﻿using Plugin.GuestureLock.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.GuestureLock.Control
{
	public class GuestureLockView : View
	{
        #region Fields
        //big circle Radius
        public int Circle_R = 20;
        //center circle Radius
        public int Circle_r = 3;
        //distance between two circles
        public int Distance = 40;

        //use in when RuntimePlatform is Android
        private double ViewWidth = 0;
        private double ViewHight = 0;

        //the center of the first circle
        public int X_Zero = 0;
        public int Y_Zero = 0;

        /// <summary>
        /// 未选中状态的圆点
        /// </summary>
        public List<Vec2> pointList = new List<Vec2>();
        /// <summary>
        /// 选中状态的圆点
        /// </summary>
        public List<Vec2> checkedList = new List<Vec2>();
        /// <summary>
        /// 需要绘制的圆点
        /// </summary>
        public List<Vec2> drawList = new List<Vec2>();
        /// <summary>
        /// 选中的圆点索引
        /// </summary>
        public List<int> indexList = new List<int>();

        public static readonly BindableProperty CheckCompleteCommandProperty = BindableProperty.Create("CheckCompleteCommand", typeof(ICommand), typeof(GuestureLockView), null, propertyChanged: (bo, o, n) => ((GuestureLockView)bo).OnCommandChanged());
        
        void OnCommandChanged()
        {
        }
        #endregion

        #region Command

        public ICommand CheckCompleteCommand
        {
            get
            {
                return (ICommand)GetValue(CheckCompleteCommandProperty);
            }

            set
            {
                if (CheckCompleteCommand != value)
                {
                    SetValue(CheckCompleteCommandProperty, value);
                }
            }
        } 

        #endregion

        #region event

        public delegate void CheckCompleteDelegate(List<int> checkList);

        public CheckCompleteDelegate _CheckCompleteDelegate;

        public event CheckCompleteDelegate CheckCompleteEvent
        {
            add
            {
                _CheckCompleteDelegate = Delegate.Combine(_CheckCompleteDelegate, value) as CheckCompleteDelegate;
            }
            remove
            {
                _CheckCompleteDelegate = Delegate.Remove(_CheckCompleteDelegate, value) as CheckCompleteDelegate;
            }
        }

        #endregion

        #region constructor
        public GuestureLockView()
        {
            

        }
        #endregion

        #region function

        public void Complete()
        {
            GetCheckedIndex();
            if(indexList != null && indexList.Count > 0)
            {
                if (_CheckCompleteDelegate != null)
                {
                    _CheckCompleteDelegate.Invoke(indexList);
                }
                if (CheckCompleteCommand != null)
                {
                    CheckCompleteCommand.Execute(indexList);
                }
            }
            Reset();
        }

        public void ProcessTouchEvent(double x, double y)
        {
            if(ViewWidth == 0)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        var service = DependencyService.Get<IDensityConvertService>();
                        ViewWidth = service.Dp2Px((int)WidthRequest);
                        ViewHight = service.Dp2Px((int)HeightRequest);
                        break;
                    default:
                        ViewWidth = WidthRequest;
                        ViewHight = HeightRequest;
                        break;
                }
            }

            if (x < 0 || y < 0 || x > ViewWidth || y > ViewHight)
            {

            }
            else
            {
                Vec2 item = CheckRange(x, y, out bool isIn);
                if (isIn && !IsAdded(item))
                {
                    if (checkedList.Count > 0)
                    {
                        var item2 = checkedList.Last();
                        foreach (Vec2 v in pointList)
                        {
                            if (!item.Equals(v) && !IsAdded(v) && CheckOnLine(item, item2, v))
                            {
                                checkedList.Add(v);
                            }
                        }
                    }
                    checkedList.Add(item);
                }
                else
                {
                    drawList.Clear();
                    drawList.AddRange(checkedList);
                    drawList.Add(item);
                }
            }
        }



        /// <summary>
        /// 判断 v 是否在 v1、v2连线内  用了最粗暴的方法  
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool CheckOnLine(Vec2 v1, Vec2 v2, Vec2 v)
        {
            double len = CalcLengthBetweenTwoPoint(v1, v2);
            double len1 = CalcLengthBetweenTwoPoint(v1, v);
            double len2 = CalcLengthBetweenTwoPoint(v2, v);
            return len == len1 + len2;
        }

        /// <summary>
        /// 计算v1、v2连线长度
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private double CalcLengthBetweenTwoPoint(Vec2 v1, Vec2 v2)
        {
            double value = Math.Pow(v1.X - v2.X, 2.0) + Math.Pow(v1.Y - v2.Y, 2.0);
            //return value;
            return Math.Abs(Math.Sqrt(value));
        }

        /// <summary>
        /// 判断x、y 是否在其中一个圆内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isIn"></param>
        /// <returns></returns>
        private Vec2 CheckRange(double x, double y, out bool isIn)
        {
            foreach (Vec2 v in pointList)
            {
                if (IsInCircle(x, y, v) && !IsAdded(v))
                {
                    isIn = true;
                    return v;
                }
            }
            isIn = false;
            return new Vec2 { X = (int)x, Y = (int)y };

        }

        /// <summary>
        /// 判断x、y 是否在 v 为圆心 Circle_R 为半径的圆内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool IsInCircle(double x, double y, Vec2 v)
        {
            return Math.Pow(x - v.X, 2.0) + Math.Pow(y - v.Y, 2.0) <= Math.Pow(Circle_R, 2.0);
        }

        /// <summary>
        /// 判断item 是否已经选中
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsAdded(Vec2 item)
        {
            return checkedList.Contains(item);
        }

        /// <summary>
        /// 初始化 原始数据
        /// </summary>
        public void InitPointList()
        {
            //DebugUtil.WriteLine("Distance   == "+ Distance);
            int deta_x = 0;
            int x = 0;
            int y = 0;
            for (int i = 0; i < 9; i++)
            {
                deta_x = i % 3;
                int deta_y = i / 3;
                x = X_Zero + deta_x * (Distance + 2 * Circle_R);
                y = Y_Zero + deta_y * (Distance + 2 * Circle_R);
                //DebugUtil.WriteLine("index("+i+") x = " + x + "  y = "+y);
                pointList.Add(new Vec2
                {
                    X = x,
                    Y = y
                });

            }
        }

        public void GetCheckedIndex()
        {
            indexList.Clear();
            //DebugUtil.WriteLine("Count == " + checkedList.Count);
            foreach (var item in checkedList)
            {
                int index = pointList.IndexOf(item);
                indexList.Add(index);
                //DebugUtil.WriteLine("index == " +index);
            }
            //DebugUtil.WriteLine("Size == " + indexList.Count);
        }

        public void Reset()
        {
            checkedList.Clear();
            drawList.Clear();
        }

        public void Dispose()
        {
            pointList.Clear();
            drawList.Clear();
            checkedList.Clear();
            indexList.Clear();
        }

        #endregion


    }
}