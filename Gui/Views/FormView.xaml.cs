using System.Windows.Controls;

namespace Gui.Views
{
    /// <summary>
    /// Interaction logic for FormView.xaml
    /// </summary>
    public partial class FormView : UserControl
    {
        public FormView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
