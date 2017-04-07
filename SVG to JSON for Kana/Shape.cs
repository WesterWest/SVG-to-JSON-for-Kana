using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SVG_to_JSON_for_Kana
{
    class Shape
    {
        public List<Point> Points { get; set; }
        public List<Point> UVs { get; set; }

        public bool Collide { get; set; }
        public String Name { get; set; }
    }
}
