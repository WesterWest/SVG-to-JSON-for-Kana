using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SVG_to_JSON_for_Kana
{
    class JArraySerializer
    {
        public static JArray SerializePoints(List<Point> list)
        {
            JArray origin = new JArray();
            foreach (Point steam in list)
            {
                JArray uPlay = new JArray();
                uPlay.Add(steam.X);
                uPlay.Add(steam.Y);
                origin.Add(uPlay);
            }
            return origin;
        }
    }
}
