using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using CourseProjectClassLibrary.Models;
using ScottPlot;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для Graph.xaml
    /// </summary>
    public partial class Graph : Window
    {


        public Graph()
        {
            InitializeComponent();
        }

        public void GraphShow(Dictionary<Client, int> dict)
        {
            ScottPlot.Plot myPlot = new(); //Создание диаграммы
            var myScatter = Diag.Plot;//Получение ссылки на диаграмму

            int i = 0;
            Tick[] ticks = new Tick[dict.Count]; //Создать массив меток по оси X
            foreach (KeyValuePair<Client, int> pair in dict)
            {
                // Добавить метку по оси X значение:Текст
                ticks[i] = new(i+1,pair.Key.ToString());
                // Добавить столбец в диаграмму
                myScatter.Add.Bar(position:i+1, value:pair.Value);
                i++;
            }
            //Добавление меток по оси X в диаграмму
            myScatter.Axes.Bottom.TickGenerator = 
                new ScottPlot.TickGenerators.NumericManual(ticks: ticks);
            myScatter.Axes.Bottom.MajorTickStyle.Length = 0;
            myScatter.HideGrid();
            myScatter.Axes.Bottom.TickLabelStyle.FontSize = 46;
            myScatter.Axes.Left.TickLabelStyle.FontSize = 46;
            // tell the plot to autoscale with no padding beneath the bars
            myScatter.Axes.Margins(bottom: 0);
            //Показать диаграмму
            Diag.Refresh();
        }
    }
}
