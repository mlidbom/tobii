using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Windows.Forms;
using NUnit.Framework;
using System;
using Void.Linq;

namespace Tobii
{
    [TestFixture]
    public class ReactiveExtensions
    {
        [Test, Ignore("requires gui interaction")]
        public void DemoEventFilteringAndSplitting()
        {
            var form = new Form{ Visible = true, TopMost = true, Width = 800, Height = 600 };
            var layOut = new FlowLayoutPanel();
 
            var positionLabel = new Label {Text = "No position"};
            var lastFixatedPosition = new Label { Text = "No fixated position" };
            layOut.Controls.Add(positionLabel);
            layOut.Controls.Add(lastFixatedPosition);
            form.Controls.Add(layOut);

            var allPoints =  Observable
                .FromEvent<MouseEventArgs>(form, "MouseMove")
                .Select(args => args.EventArgs.Location);

            allPoints.Subscribe(point => positionLabel.Text = point.ToString());
            allPoints.Where(IsStabilized()).Subscribe(point => lastFixatedPosition.Text = point.ToString());

            

            Application.Run(form);
        }


        protected bool ChunkIsStable(IEnumerable<Point> points)
        {
            const int maxVariance = 50;
            var xVariance = points.Max(p => p.X) - points.Min(p => p.Y);
            var yVariance = points.Max(p => p.X) - points.Min(p => p.Y);
            return xVariance < maxVariance && yVariance < maxVariance;
        }


        static Func<Point, bool> IsStabilized()
        {
            const int maxVariance = 50;
            var last10 = new LinkedList<Point>(new Point(0, 0).Repeat(50));
            return currentPosition =>
            {
                last10.RemoveLast();
                last10.AddFirst(currentPosition);
                var xVariance = last10.Max(p => p.X) - last10.Min(p => p.X);
                var yVariance = last10.Max(p => p.Y) - last10.Min(p => p.Y);
                return xVariance < maxVariance && yVariance < maxVariance;
            };
        }
    }
} 