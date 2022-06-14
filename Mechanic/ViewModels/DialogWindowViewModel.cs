using System.Windows.Input;

using Mechanic.Commands.Dialog;


namespace Mechanic.ViewModels
{
    public class DialogWindowViewModel : ViewModelBase
    {
        private string message = null!;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }


        public DialogWindowViewModel()
        {
            YesCommand = new YesCommand();
            NoCommand = new NoCommand();
        }
    }
}
