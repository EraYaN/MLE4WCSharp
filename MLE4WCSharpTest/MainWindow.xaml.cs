using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MLE4WCSharp;

namespace MLE4WCSharpTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MATLABWrapper mlw;
		public MainWindow()
		{
			InitializeComponent();			
		}

		private void initButton_Click(object sender, RoutedEventArgs e)
		{
			mlw = new MATLABWrapper("../../../mcode", "EPO4 Test Group");
		}

		private void radarTestButton_Click(object sender, RoutedEventArgs e)
		{
			double[] r = { 5, 6, 3.6, 8.5, 2.6, 9 };
			double[] p = { 18, 20, 30, 25, 19, 12 };
			mlw.radar_epo_4(r, p);
		}
	}
}
