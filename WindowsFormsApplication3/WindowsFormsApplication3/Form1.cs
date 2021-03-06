﻿using System;
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
            t1 = new Thread(thread1);
            t2 = new Thread(thread2);
            InitializeComponent();
        }

        private void Axis()
        {
            Point[] graph = { new Point(10, 10),    new Point(10, 510),     new Point(510, 510),
                              new Point(2, 18),     new Point(10, 11),      new Point(17, 18),
                              new Point(502, 502),  new Point(511, 510),    new Point(502, 517)};
            Graphics g = graphPanel.CreateGraphics();
            g.DrawLine(new Pen(Brushes.Black, 4), graph[0], graph[1]);
            g.DrawLine(new Pen(Brushes.Black, 4), graph[1], graph[2]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[3], graph[4]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[4], graph[5]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[6], graph[7]);
            g.DrawLine(new Pen(Brushes.Black, 3), graph[7], graph[8]);
            this.BackColor = Color.Gray;
        }

        Thread t1, t2;
		List<Point> coordinates = new List<Point> ();

        private void button1_Click(object sender, EventArgs e)
        {
        Stop_Threads(true, false);
        t1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        Stop_Threads(false, true);
        t2.Start();
        }
		private void addButton_Click(object sender, EventArgs e){
			Point inputPoint = new Point ();
			int X, Y;
			try{
				X = int.Parse(tbX.Text);
				Y = int.Parse(tbY.Text);
				inputPoint.X = X;
				inputPoint.Y = Y;
			}
			catch{
				Console.WriteLine("An error occured when processing coordinates");
			}
			coordinates.Add(inputPoint);
			Graph.printPoint(graphPanel, inputPoint);
		}

		public void thread1()
		{
			Graph.sineCurve(graphPanel, 30, 10, 100);
			Stop_Threads(true, false);
		}

		public void thread2()
		{
			int x = 10, y = 400;
			for (int i = 10; i < 510; i += 3)
			{
				graphPanel.CreateGraphics().DrawLine(new Pen(Brushes.Red, 1), new Point(x, y), new Point((x = i), (y = 400 + (int)(Math.Sin(i) * 100))));
				System.Threading.Thread.Sleep(40);
			}
			Stop_Threads(false, true);
		}

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Axis();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Stop_Threads(true, true);
            Reset_Background();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Stop_Threads(true, true);
        }

        private void Stop_Threads(bool first, bool second)
        {
            if (first)
            {
                t1.Abort();
                t1 = new Thread(thread1);
            }
            if (second)
            {
                t2.Abort();
                t2 = new Thread(thread2);
            }
        }

        private void Reset_Background()
        {
			graphPanel.CreateGraphics ().Clear (Color.Gray);
            Axis();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
			Axis ();
        }

        private void grid_Click(object sender, EventArgs e)
        {
			for (int i = 10; i < 510; i++) {
				if (i % 20 == 0) {
					Point top = new Point (i + 10, 510);
					Point bottom = new Point (i + 10, 10);
					Point left = new Point (10, i+10);
					Point right = new Point (510, i+10);
					graphPanel.CreateGraphics ().DrawLine (new Pen (Brushes.Black), bottom, top);
					graphPanel.CreateGraphics ().DrawLine (new Pen (Brushes.Black), left, right);
				}
			}
        }

		void tbX_Click (object sender, EventArgs e)
		{
			tbX.SelectAll ();
		}
		void tbY_Click (object sender, EventArgs e)
		{
			tbY.SelectAll ();
		}
    }
}
