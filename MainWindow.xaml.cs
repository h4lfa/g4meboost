using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using G4meBoost.ViewModels;

namespace G4meBoost
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // Enable dragging the title bar
            MouseLeftButtonDown += (s, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();
            };
        }

        private void OnAuthorClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/h4lfa",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        private void OnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnMaximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaxBtn.Content = "□";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaxBtn.Content = "❐";
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
