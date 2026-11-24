using System.Windows;

namespace G4meBoost
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show(
                    $"Произошла ошибка: {args.Exception.Message}",
                    "G4meB∞st — Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}
