using MahApps.Metro.Controls;

namespace GUI.Views
{
    /// <summary>
    /// Interaktionslogik fÃ¼r MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
            base.DataContext = this;
        }
    }
}
