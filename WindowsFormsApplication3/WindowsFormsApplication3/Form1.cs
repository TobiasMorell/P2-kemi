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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Paint += Axis;
        }

        private void Axis(object sender, PaintEventArgs e)
        {
            Point[] graph = { new Point(10, 10),    new Point(10, 510),     new Point(510, 510),
                              new Point(2, 18),     new Point(10, 11),      new Point(17, 18),
                              new Point(502, 502),  new Point(511, 510),    new Point(502, 517)};
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Brushes.Black, 4), graph[0], graph[1]);
            g.DrawLine(new Pen(Brushes.Black, 4), graph[1], graph[2]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[3], graph[4]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[4], graph[5]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[6], graph[7]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[7], graph[8]);
            this.BackColor = Color.Gray;
        }

        Thread t1, t2;

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Thread(thread1);
            t1.IsBackground = true;
            t1.Start(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            t2 = new Thread(thread2);
            t2.IsBackground = true;
            t2.Start();
        }

        public void thread1()
        {
            Random rand = new Random();
            int x = 10, y = 100;
            for (int i = 10; i < 510; i += 6)
            {
                this.CreateGraphics().DrawLine(new Pen(Brushes.White, 1), new Point(x, y), new Point((x = i), (y = 100 + (int)(Math.Sin(i) * 100))));
                System.Threading.Thread.Sleep(20);
            }
        }

        public void thread2()
        {
            Random rand = new Random();
            int x = 10, y = 400;
            for (int i = 10; i < 510; i += 3)
            {
                this.CreateGraphics().DrawLine(new Pen(Brushes.White, 1), new Point(x, y), new Point((x = i), (y = 400 + (int)(Math.Sin(i) * 100))));
                System.Threading.Thread.Sleep(40);
            }
        }
    }
}
