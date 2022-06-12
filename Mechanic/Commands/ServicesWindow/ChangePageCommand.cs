using System;
using System.ComponentModel;

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
            if (parameter is not null && int.TryParse(parameter.ToString(), out int pageNumber))
            {
                viewModel.UpdatePage(pageNumber);
            }
            else
            {
                switch (direction)
                {
                    case Direction.Left:
                        viewModel.UpdatePage(viewModel.CurrentPage - 1);
                        break;
                    case Direction.Right:
                        viewModel.UpdatePage(viewModel.CurrentPage + 1);
                        break;
                    default:
                        return;
                }
            }

            viewModel.RefreshListView();
        }



        public enum Direction { Left, Right }
    }
}
