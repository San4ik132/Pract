using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pract14
{
    public class DrawCanvas : Panel
    {
        private List<DrawingVisualObject> objects = new List<DrawingVisualObject>();
        protected override Visual GetVisualChild(int index)
        {
            return objects[index];
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return objects.Count;
            }
        }
        public void AddVisual(DrawingVisualObject vis)
        {
            objects.Add(vis);
            base.AddVisualChild(vis);
            base.AddLogicalChild(vis);
        }
        public DrawingVisualObject GetVisualObject(Point p)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, p);
            return hitResult.VisualHit as DrawingVisualObject;
        }
    }

    public class DrawingVisualObject : DrawingVisual
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DrawingVisualObject(int id, string name)
        {
            Id = id;
            Name = name;

        }
    }
}
