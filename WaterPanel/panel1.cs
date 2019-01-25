using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace WaterPanel
{
    public partial class panel1 : Panel
    {
        private Timer m_Timer;//计时器  
        private const int TotalProgress = 1;
        private double Progress = 0;
        private SizeF _textSize;
        private Point mouseClickPoint;
        /// <summary>
        ///图标Icon
        /// </summary>
        private Image _icon;
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        /// <summary>
        ///温度属性
        /// </summary>
        //private int temp;
        //public int Temp
        //{
        //    get { return temp; }
        //    set
        //    {
        //        temp = value;
        //    }
        //}

        /// <summary>
        ///水波纹颜色
        /// </summary>
        private Color waterColor = Color.Green;
        public Color WaterColor
        {
            get { return waterColor; }
            set
            {
                waterColor = value;

            }
        }
        /// <summary>
        /// Panel函数
        /// </summary>
        public panel1()
        {
            AutoSize = true;
            m_Timer = new Timer();
            m_Timer.Interval = 100;//每经过一个Inteval，aTimer_Tick事件就进行一次
            m_Timer.Enabled = false;
            m_Timer.Tick += new EventHandler(aTimer_Tick);
        }
        /// <summary>
        /// 计时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aTimer_Tick(object sender, EventArgs e)
        {
            if (Progress > TotalProgress)
            {
                //m_Timer.Enabled = false;
                //m_Timer.Stop();
                Progress = 0;//原来是1
                return;
            }
            Progress += 0.1;//最开始是每次加0.1，可以改变中心矩形向四周扩充的速度
            //if (Progress > TotalProgress)
            //    Progress = 1;
            Invalidate();
        }

        #region 重载事件

        /// <summary>
        /// 重写事件Onpaint
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.Clear(Parent.BackColor);

            Color c = BackColor;
            using (Brush b = new SolidBrush(c))
                g.FillRectangle(b, ClientRectangle);

            if (Progress <= TotalProgress)
            {

                g.SmoothingMode = SmoothingMode.AntiAlias;

                //using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (Progress * 100)/2)//透明度设置 透明度逐渐减小
                //    , waterColor))) //使用什么样的颜色刷实现水波纹效果
                //{
                Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (Progress * 100) / 2)//透明度设置 透明度逐渐减小
                    , waterColor));
                var rippleSize = (int)(Progress * Width * 1.5);//圆的尺寸（初始值是*2）
                g.FillRectangle(rippleBrush, Width / 2 - rippleSize / 2, Height / 2 - rippleSize / 2, rippleSize, rippleSize);//实现鼠标点击控件上任意位置都能从panel正中间以矩形水波荡漾
               //以下两条语句分别为圆形填充和矩形填充，填充过程中起始点在不断地变化，因此能呈现出从中间向四周不断变大填充的效果
               //g.FillEllipse(rippleBrush, new Rectangle(mouseClickPoint.X - rippleSize / 2, mouseClickPoint.Y - rippleSize / 2, rippleSize, rippleSize));//实现鼠标点击控件上以鼠标所点击位置呈现圆形水波荡漾
               //g.FillRectangle(rippleBrush, mouseClickPoint.X - rippleSize / 2, mouseClickPoint.Y - rippleSize / 2, rippleSize, rippleSize);//实现鼠标点击控件上以鼠标所点击位置呈现矩形水波荡漾
               
                //以下几行语句能实现从矩形边框往中间荡漾
                //            Brush rippleBrush1 = new SolidBrush(Color.FromArgb((int)(Progress * 100)//透明度设置 透明度逐渐减小
                //, waterColor));
                //            var rippleSize1 = (int)(Width - Progress * Width);//圆的尺寸（初始值是*2）                                                                                                           //FillEllipse(Brush brush, int x, int y, int width, int height)填充rectangle指定的边框所定义的椭圆内部，具体是从以鼠标点击点为左上角向右下角填充椭圆
                //                g.FillRectangle(rippleBrush1, Width / 2 - rippleSize1 / 2, Height / 2 - rippleSize1 / 2, rippleSize1, rippleSize1);

                g.SmoothingMode = SmoothingMode.None;

            }


        }



        #endregion

        private Size GetPreferredSize()
        {
            return GetPreferredSize(new Size(0, 0));
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var extra = 16;

            if (Icon != null)
                extra += 28;

            return new Size((int)Math.Ceiling(_textSize.Width) + extra, 36);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {

                    Progress = 0;
                    mouseClickPoint = args.Location;
                    m_Timer.Enabled = true;



                }
            };
        }


    }
}



