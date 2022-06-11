using System;
using System.Windows.Input;


namespace Mechanic.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;


        public virtual bool CanExecute(object? parameter) { return true; }
        public abstract void Execute(object? parameter);

        /// <summary>
        /// It updates the 'CanExecute' state of the command. To see a change in the state, 'CanExecute' method also must be overriden
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
