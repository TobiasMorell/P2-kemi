using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication3
{
    class Graph : Panel
    {
        Graphics g;

        public Graph(int x, int y, int h, int w)
        {
            this.Height = h;
            this.Width = w;
            this.Left = x;
            this.Top = y;

            Graphics g = this.CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
 	        
        }
    }
}
