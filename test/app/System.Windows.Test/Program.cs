using Mono;
using Mono.Xaml;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace System.Windows.Test
{
    internal class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            //Environment.SetEnvironmentVariable("MOONLIGHT_DEBUG", "all");

            //Mono.Xaml.XamlLoader.AllowMultipleSurfacesPerDomain = true;


            NativeMethods.runtime_init_desktop();

            var windowingSystem = NativeMethods.runtime_get_windowing_system();
            var windowPtr = NativeMethods.moon_windowing_system_create_window(windowingSystem, MoonWindowType.Desktop, 300, 300, IntPtr.Zero, IntPtr.Zero);

            var surface = NativeMethods.surface_new(windowPtr);
            IntPtr native_uri = UriHelper.ToNativeUri(new Uri("file:///"));
            NativeMethods.surface_set_source_location(surface, native_uri);
            NativeMethods.uri_free(native_uri);

            var deployment = Deployment.Current;
            deployment.InitializeDeployment("en-US", "en-US");

            Deployment.RegisterAssembly(typeof(Program).Assembly);

            var app = Application.Current;
            app.Startup += App_Startup;

            var dictionary = (ResourceDictionary)new XamlParser().ParseString(Moonlight.Themes.generic);
            app.Resources.MergedDictionaries.Add(dictionary);

            //dictionary = (ResourceDictionary)new XamlParser().ParseString(Resource1.ResourceDictionary1);
            //app.Resources.MergedDictionaries.Add(dictionary);

            var root = (FrameworkElement)new XamlParser().ParseFile("Test.xaml");
            app.RootVisual = root;

            var button = (Button)root.FindName("MyButton");
            button.Click += Button_Click;
            root.DataContext = new { TestString = "balls" };
            ((Storyboard)root.Resources["MyAnimation"]).Begin();

            //var markdown = new MarkdownTextBlock();
            //markdown.Foreground = new SolidColorBrush(Colors.Black);
            //markdown.Text = md;

            //((Grid)root).Children.Add(markdown);

            NativeMethods.moon_windowing_system_run_main_loop(windowingSystem, windowPtr, true);
        }

        private static void App_Startup(object sender, StartupEventArgs e)
        {

        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.FullScreenOptions = Interop.FullScreenOptions.StaysFullScreenWhenUnfocused;
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;

        }

       
    }
}