using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mechanic.ViewModels;


namespace Mechanic.Commands.ServicesWindow
{
    public class ChangePageCommand : CommandBase
    {
        private ServicesViewModel viewModel;
        private Direction direction;


        public ChangePageCommand(ServicesViewModel viewModel, Direction direction)
        {
            this.viewModel = viewModel;
            this.direction = direction;

            this.viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.CurrentPage) ||
                e.PropertyName == nameof(viewModel.NumofPages))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            switch (direction)
            {
                case Direction.Left:
                    return viewModel.CurrentPage > 1;
                case Direction.Right:
                    return viewModel.CurrentPage < viewModel.NumofPages;
                default:
                    return false;
            }
        }

        public override void Execute(object? parameter)
        {
            // Check if a specific page number is sent in order to jump to a page number (e.g. first or last)
            if (parameter is not null && Int32.TryParse(parameter.ToString(), out int pageNumber))
            {
                viewModel.CurrentPage = pageNumber;
            }
            else
            {
                switch (direction)
                {
                    case Direction.Left:
                        viewModel.CurrentPage--;
                        break;
                    case Direction.Right:
                        viewModel.CurrentPage++;
                        break;
                    default:
                        return;
                }
            }
            
            // TODO: Just to test the logic. It will be removed
            viewModel.UpdateListView();
        }



        public enum Direction { Left, Right }
    }
}
