using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSPlab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double CalcPz(double tn, int n)
        {
            /*
            return (6 * Math.Pow(
                Math.Sin(Math.PI * n) -
                Math.PI * n * Math.Cos(Math.PI * n), 2)) /
                (Math.Pow(Math.PI, 4) *
                Math.Pow(n, 4));
                */
            return (3 * Math.Pow(10 * Math.Pow(tn, 2)
                * Math.Sin(Math.PI * n)
                - 10 * Math.PI * Math.Pow(tn, 2) * n
                * Math.Cos(Math.PI * n), 2))
                / (50 * Math.Pow(Math.PI, 4) * Math.Pow(tn, 4)
                * Math.Pow(n, 4));
        }

        private double Calcbn(double tn, double Emax, int n)
        {
            /*
            return -(2 * Emax *
                (Math.Sin(Math.PI * n) -
                Math.PI * n * Math.Cos(Math.PI * n))) /
                (Math.Pow(Math.PI, 2) * Math.Pow(n, 2));
            */
            return (10 * (Math.Pow(tn, 2) * Math.Sin(Math.PI * n)
                - 10 * Math.PI * Math.Pow(tn, 2)
                * n * Math.Cos(Math.PI * n)))
                / (11 * Math.Pow(Math.PI, 2) * tn * Math.Pow(n, 2));
        }

        private double CalcFin(double an, double bn)
        {
            return Math.Atan2(bn, an);
        }

        private double CalcAn(double tn, double Emax, int n)
        {
            /*
            return (2 * Math.Sqrt((Math.Pow(Emax, 2) *
                Math.Pow(Math.Sin(Math.PI * n) - Math.PI * n *
                Math.Cos(Math.PI * n), 2)) / Math.Pow(n, 4))) /
                Math.Pow(Math.PI, 2);
            */
            return Math.Sqrt(Math.Pow(10 * Math.Pow(tn, 2)
                * Math.Sin(Math.PI * n)
                - 10 * Math.PI * Math.Pow(tn, 2)
                * n * Math.Cos(Math.PI * n), 2)
                / (121 * Math.Pow(tn, 2) * Math.Pow(n, 4)))
                / (Math.Pow(Math.PI, 2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Emax = 0;
            double T = 0;
            double tn = 88;
            double.TryParse(textBoxEmax.Text, out Emax);
            double.TryParse(textBoxtn.Text, out tn);
            double.TryParse(textBoxT.Text, out T);
            double[] y = new double[200];

            int n = 0;
            List<double> PzList = new List<double>();
            List<double> An = new List<double>();
            List<double> Fin = new List<double>();
            double Pz = 0;
            double P = 0;
            string s = textBoxP.Text.Replace('.', ',');
            double.TryParse(s, out P);

            while (Pz < P)
            {
                double d = CalcPz(tn, n + 1);
                Pz += d;
                PzList.Add(d);
                n++;
            }

            listBox3.Items.Clear();
            for (int i = 1; i < n + 1; i++)
            {
                Fin.Add(CalcFin(0, Calcbn(tn, Emax, i)));
                listBox3.Items.Add("φ" + i.ToString() + ":\t" + Fin[i-1].ToString());
            }

            listBox2.Items.Clear();
            for (int i = 1; i < n + 1; i++)
            {
                An.Add(CalcAn(tn, Emax, i));
                listBox2.Items.Add("A"+i.ToString()+":\t" + An[i-1].ToString());
            }

            listBox1.Items.Clear();
            listBox1.Items.Add("Рк / Рс:\t" + Pz.ToString());
            listBox1.Items.Add("Кол.-во гармоник:\t" + n.ToString());
            foreach (var l in PzList.ToArray())
                listBox1.Items.Add(l);

            int max = 2;
            int.TryParse(textBox1.Text, out max);
            chart2.Series[0].Points.Clear();
            for (int j = -max / 2; j < max / 2; j++)
            {
                for (double x = -tn / 2, i = 0; x <= tn / 2 && i < 200; x++, i++)
                {
                    //y[(int)i] = (2 * Emax * x) / tn;
                    y[(int)i] = (10 * x) / 11;
                    chart2.Series[0].Points.AddXY(x + j * tn, y[(int)i]);
                }
            }
            
            chart2.Series[0].Points.AddXY(tn / 2 + (max / 2 -1 ) * tn, -Emax);
            chart2.Series[0].Name = "Emax:\n" + textBoxEmax.Text + "\n\ntn:\n" + textBoxtn.Text + "\n\nT:\n" + textBoxT.Text;

            chart1.Series[0].Points.Clear();
            for (int i = 0; i < n; i++)
            {
                chart1.Series[0].Points.AddXY(i + 1, listBox1.Items[i + 2]);
            }

            chart3.Series[0].Points.Clear();
            for (int i = 0; i < n; i++)
            {
                chart3.Series[0].Points.AddXY(i + 1, An[i]);
            }

            chart4.Series[0].Points.Clear();
            for (int i = 0; i < n; i++)
            {
                chart4.Series[0].Points.AddXY(i + 1, Fin[i]);
            }
        }

        private void textBoxP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }

        private void textBoxEmax_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxP_KeyDown(sender, e);
        }

        private void textBoxtn_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxP_KeyDown(sender, e);
        }

        private void textBoxT_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxP_KeyDown(sender, e);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxP_KeyDown(sender, e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    break;
                case 1:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    break;
                case 2:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    break;
                case 3:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    break;
                case 4:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                    break;
                case 5:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                    break;
                case 6:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    break;
                case 7:
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;
                    //chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;
                    chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;
                    chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;
                    break;
            }
        }
    }
}
