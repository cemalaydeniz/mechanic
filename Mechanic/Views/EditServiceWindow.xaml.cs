using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Mechanic.ViewModels;


namespace Mechanic.Views
{
    /// <summary>
    /// Interaction logic for EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : Window
    {
        private EditServiceViewModel viewModel = null!;


        public EditServiceWindow(bool isReadOnly)
        {
            InitializeComponent();
            Initialize(isReadOnly);
        }

        private void Initialize(bool isReadOnly)
        {
            viewModel = (EditServiceViewModel)DataContext;

            viewModel.IsReadOnly = isReadOnly;
        }
    }
}
