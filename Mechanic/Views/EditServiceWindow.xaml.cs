using System.Windows;

using Mechanic.ViewModels;


namespace Mechanic.Views
{
    /// <summary>
    /// Interaction logic for EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : Window
    {
        private EditServiceViewModel viewModel = null!;


        public EditServiceWindow(string windowName, bool isReadOnly)
        {
            InitializeComponent();
            Initialize(windowName, isReadOnly);
        }

        private void Initialize(string windowName, bool isReadOnly)
        {
            viewModel = (EditServiceViewModel)DataContext;

            viewModel.WindowName = windowName;
            viewModel.IsReadOnly = isReadOnly;
        }
    }
}
