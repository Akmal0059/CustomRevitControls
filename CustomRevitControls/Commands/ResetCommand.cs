using System;
using System.Windows;
using System.Windows.Input;

namespace CustomRevitControls.Commands
{
    public class ResetCommand : ICommand
    {
        private RevitControl revitControl;
        private DependencyProperty prop;
        public ResetCommand(RevitControl rc, DependencyProperty prop)
        {
            revitControl = rc;
            this.prop = prop;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var dialog = MessageBox.Show("Уверены, что хотите сборсить значение?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialog == MessageBoxResult.Yes)
                revitControl.SetValue(prop, null);

        }
    }
}
