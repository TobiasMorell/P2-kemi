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
    static class Graph
    {
        public static void sineCurve(Panel background, double altitude, double time, double center){
			Point coordinate = new Point();
			List<Point> graphPoints = new List<Point> ();
			for (int i = 0; i < background.Width; i++) {
				coordinate = sineCoord(i, altitude, time, center);
				graphPoints.Add(coordinate);
			}
			printPointList (background, graphPoints);
		}

		private static Point sineCoord(int x, double alt, double time, double center){
			Point coordinate = new Point();
			double t = (double)x;

			coordinate.X = x;
			coordinate.Y = (int) (alt * Math.Sin(t + time) + center);

			return coordinate;
		}

		public static void printPointList(Panel background, List<Point> printList){
			foreach (var coord in printList) {
				System.Drawing.Graphics point = background.CreateGraphics ();
				Point nextPoint;
				try{
					nextPoint = projectPoint(printList.ElementAt(printList.IndexOf(coord) - 1));
				}
				catch{
					nextPoint = coord;
				}
				point.DrawLine(new Pen(Brushes.DarkOliveGreen, 2), projectPoint(coord), nextPoint);
			}
		}

		private static Point projectPoint(Point original){
			Point projCoord = new Point();

			projCoord.X = original.X + 10;
			projCoord.Y = 510 - original.Y;

			return projCoord;
		}

		public static void printPoint (Panel background, Point coordinate)
		{
			//0, 0 på koordinatsystemet er i punktet (10, 510)
			Point projCoord = projectPoint (coordinate);

			System.Drawing.Graphics point = background.CreateGraphics();
			point.FillEllipse(Brushes.DarkOliveGreen, new Rectangle(projCoord.X, projCoord.Y, 3, 3));
		}


    }
}
