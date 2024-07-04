using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoonTest.System.Windows.Controls
{
	[TestClass]
	public class IScrollInfoTest : SilverlightTest
	{
		public IScrollInfo IScrollInfo
		{
			get { return ScrollInfo; }
		}

		public ScrollInfo ScrollInfo
		{
			get { return ScrollViewer.FindFirstChild<ScrollInfo> (); }
		}

		public ScrollViewer ScrollViewer
		{
			get; set;
		}

		[TestInitialize]
		public void Initialize ()
		{
			var template = (ControlTemplate)XamlReader.Load (@"
<ControlTemplate
	xmlns=""http://schemas.microsoft.com/client/2007""
	xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
	xmlns:clr=""clr-namespace:MoonTest.System.Windows.Controls;assembly=moon-unit"">
	<ScrollContentPresenter>
		<clr:ScrollInfo Width=""1000"" Height=""1000"" />
	</ScrollContentPresenter>
</ControlTemplate>
");
			ScrollViewer = new ScrollViewer {
				Template = template,
				Width = 50,
				Height = 50,
				Content = new Rectangle {
					Width = 1000,
					Height = 1000,
					Fill = new RadialGradientBrush (Colors.Red, Colors.Green)
				}
			};
		}

		[TestMethod]
		[Asynchronous]
		public void CalledMethods ()
		{
			CreateAsyncTest (ScrollViewer, () => {
				
			}, () => {
				Assert.IsTrue (ScrollInfo.CalledCanHorizontallyScroll, "#1");
				Assert.IsTrue (ScrollInfo.CalledCanVerticallyScroll, "#2");
			});
		}
	}
}
