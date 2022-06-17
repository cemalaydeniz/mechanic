using System.Windows;

using Mechanic.ViewModels;


namespace Mechanic.Views
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private DialogWindowViewModel viewModel = null!;


        public DialogWindow(string message)
        {
            InitializeComponent();
            Initialize(message);
        }

        private void Initialize(string message)
        {
            viewModel = (DialogWindowViewModel)DataContext;

            viewModel.Message = message;
        }
    }
}
