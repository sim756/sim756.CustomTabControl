using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sim756.CustomTabControl
{
    public class CustomTabControl : TabControl
    {
        private Color _selectTabColor = Color.FromArgb(30, 70, 130);
        private Color _selectTabLineColor = Color.FromArgb(0, 0, 0);
        private Color _tabColor = Color.WhiteSmoke;

        public CustomTabControl()
        {
            try
            {
                Appearance = TabAppearance.Buttons;
                DrawMode = TabDrawMode.Normal;
                ItemSize = new Size(0, 0);
                SizeMode = TabSizeMode.Fixed;
            }
            catch
            {
                // ignored
            }
        }

        public Color SelectTabColor
        {
            get { return _selectTabColor; }
            set { _selectTabColor = value; }
        }

        public Color SelectTabLineColor
        {
            get { return _selectTabLineColor; }
            set { _selectTabLineColor = value; }
        }

        public Color TabColor
        {
            get { return _tabColor; }
            set { _tabColor = value; }
        }

        protected override void InitLayout()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            base.InitLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            try
            {
                if (!Visible)
                {
                    return;
                }

                Rectangle clientRectangle = ClientRectangle;
                clientRectangle.Inflate(2, 2);

                Pen border = new Pen(Color.White);
                g.DrawRectangle(border, clientRectangle);

                Brush solidBrush = new SolidBrush(Color.White);
                g.FillRectangle(solidBrush, ClientRectangle);

                solidBrush = new SolidBrush(_selectTabLineColor);
                Rectangle rectangle = ClientRectangle;
                rectangle.Height = 1;
                rectangle.Y = 25;
                g.FillRectangle(solidBrush, rectangle);

                solidBrush = new SolidBrush(_selectTabLineColor);
                rectangle = ClientRectangle;
                rectangle.Height = 1;
                rectangle.Y = 26;
                g.FillRectangle(solidBrush, rectangle);


                Region region = g.Clip;

                for (int i = 0; i < TabCount; i++)
                {
                    DrawTab(g, TabPages[i], i);
                    TabPages[i].BackColor = Color.White;
                }

                g.Clip = region;

                if (SelectedTab != null)
                {
                    border = new Pen(Color.White);
                    clientRectangle.Offset(1, 1);
                    clientRectangle.Width -= 2;
                    clientRectangle.Height -= 2;
                    g.DrawRectangle(border, clientRectangle);
                    clientRectangle.Width -= 1;
                    clientRectangle.Height -= 1;
                    g.DrawRectangle(border, clientRectangle);
                }
            }
            catch
            {
                // ignored
            }
        }

        internal void DrawTab(Graphics g, TabPage customTabPage, int nIndex)
        {
            Rectangle tabRect = GetTabRect(nIndex);
            RectangleF tabTextRect = GetTabRect(nIndex);
            bool isSelected = (SelectedIndex == nIndex);
            Point[] points;

            if (Alignment == TabAlignment.Top)
            {
                points = new[]
                {
                    new Point(tabRect.Left, tabRect.Bottom),
                    new Point(tabRect.Left, tabRect.Top + 0),
                    new Point(tabRect.Left + 0, tabRect.Top),
                    new Point(tabRect.Right - 0, tabRect.Top),
                    new Point(tabRect.Right, tabRect.Top + 0),
                    new Point(tabRect.Right, tabRect.Bottom),
                    new Point(tabRect.Left, tabRect.Bottom)
                };
            }
            else
            {
                points = new[]
                {
                    new Point(tabRect.Left, tabRect.Top),
                    new Point(tabRect.Right, tabRect.Top),
                    new Point(tabRect.Right, tabRect.Bottom - 0),
                    new Point(tabRect.Right - 0, tabRect.Bottom),
                    new Point(tabRect.Left + 0, tabRect.Bottom),
                    new Point(tabRect.Left, tabRect.Bottom - 0),
                    new Point(tabRect.Left, tabRect.Top)
                };
            }

            Brush brush;
            if (isSelected)
            {
                brush = new SolidBrush(_selectTabColor);
                g.FillPolygon(brush, points);
                brush.Dispose();
                g.DrawPolygon(new Pen(_selectTabColor), points);
            }
            else
            {
                brush = new SolidBrush(_tabColor);
                g.FillPolygon(brush, points);
                brush.Dispose();
                g.DrawPolygon(new Pen(_tabColor), points);
            }

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            RectangleF rectangleF = tabTextRect;
            rectangleF.Y += 2;
            brush = isSelected ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
            g.DrawString(customTabPage.Text, Font, brush, rectangleF, stringFormat);
        }
    }
}