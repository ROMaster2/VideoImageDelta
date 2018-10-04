using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoImageDeltaApp.Models
{
    public abstract class IGeometry
    {
        IGeometry Parent { get; set; }
        List<IGeometry> Children { get; set; }

        Geometry Geometry { get; set; }

        Geometry ParentGeometry
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.Geometry;
                }
                else
                {
                    return Geometry.Blank;
                }
            }
        }

        List<Geometry> ChildGeometries
        {
            get
            {
                var l = new List<Geometry>();
                foreach (var child in Children)
                {
                    l.Add(child.Geometry);
                }
                return l;
            }
        }
    }
}
