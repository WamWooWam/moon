// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KeyTest {
	public partial class Page : Canvas {
		public Page () {
			InitializeComponent ();

			rect.GotFocus += delegate (object sender, RoutedEventArgs e) {
				rectfill.Color = Colors.Green;
			};
		}
	}
}
