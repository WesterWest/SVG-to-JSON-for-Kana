using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace SVG_to_JSON_for_Kana
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        String path;
        public MainWindow()
        {
            openDialog();
            InitializeComponent();
            shapesList.ItemsSource = shapes;
            shapesList.SelectionChanged += (kana, podouble) =>
            {
                rightSideGrid.DataContext = shapesList.SelectedItem;
            };
        }

        private void openDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"F:\OneDrive\Documents\GitHub\Kana\Assets\VanillaModule\models";
            dialog.Filter = "Scalable Vector Graphics (*.svg)|*.SVG";
            if (dialog.ShowDialog().Value)
            {
                path = System.IO.Path.GetDirectoryName(dialog.FileName);
                deserializeSVG(dialog.FileName);
            }
        }



        private void deserializeSVG(String path)
        {
            XmlDocument document = new XmlDocument();
            
            document.Load(path);
            XmlNodeList rects =  document.GetElementsByTagName("rect");
            XmlNodeList polygons = document.GetElementsByTagName("polygon");

            foreach(XmlNode rect in rects)
            {
                double x = double.Parse(rect.Attributes["x"].Value);
                double y = double.Parse(rect.Attributes["y"].Value);
                double width = double.Parse(rect.Attributes["width"].Value);
                double height = double.Parse(rect.Attributes["height"].Value);
                List<Point> points = new List<Point>();
                points.Add(new Point(x, y));
                points.Add(new Point(x + width, y));
                points.Add(new Point(x + width, y + height));
                points.Add(new Point(x, y + height));

                shapes.Add(new Shape() { Points = points, Collide = true, Name = "Untitled", UVs = new List<Point>(points.Count)});
            }


            foreach (XmlNode polygon in polygons)
            {
                String[] pointsString = polygon.Attributes["points"].Value.Split(' ');
                List<Point> points = new List<Point>();
                foreach(String point in pointsString)
                {
                    String[] cords =  point.Split(',');
                    points.Add(new Point(Int32.Parse(cords[0]), Int32.Parse(cords[1])));
                }

                shapes.Add(new Shape() { Points = points, Collide = true, Name = "Untitled", UVs = new List<Point>(points.Count) });
            }
        }

        private void openDialog(object sender, RoutedEventArgs e)
        {
            openDialog();
        }

        private void export(object sender, RoutedEventArgs e)
        {
            JObject jObject = new JObject();
            jObject.Add("name", "penis");
            JArray partsArray = new JArray();
            foreach (Shape shape in shapes)
            {
                JObject JPart = new JObject();
                JArray JVertices = JArraySerializer.SerializePoints(shape.Points);
                JArray JUVs = JArraySerializer.SerializePoints(shape.UVs);

                JPart.Add("name", shape.Name);
                JPart.Add("collide", shape.Collide);
                JPart.Add("vertices", JVertices);
                JPart.Add("uvs", JUVs);
                partsArray.Add(JPart);
            }

            jObject.Add("parts", partsArray);


            using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(path, "penis.json")))
            {
                sw.Write(JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}
