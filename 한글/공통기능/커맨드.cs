using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 한글.공통기능
{
    public class Command<T> : DelegateCommandBase
    {
        public Command(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        public Command(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        public static Command<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new Command<T>(executeMethod);
        }

        public static Command<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new Command<T>(executeMethod, canExecuteMethod);
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public async Task Execute(T parameter)
        {
            await base.Execute(parameter);
        }


        private Command(Func<T, Task> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        private Command(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

    }


    public class Command : DelegateCommandBase
    {
        public Command(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        public Command(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        public static Command FromAsyncHandler(Func<Task> executeMethod)
        {
            return new Command(executeMethod);
        }

        public static Command FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new Command(executeMethod, canExecuteMethod);
        }

        public async Task Execute()
        {
            await Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        private Command(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {
        }

        private Command(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }
    }


    public abstract class DelegateCommandBase : ICommand
    {
        private readonly Func<object, Task> _executeMethod;
        private readonly Func<object, bool> _canExecuteMethod;

        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = (arg) => { executeMethod(arg); return Task.Delay(0); };
            _canExecuteMethod = canExecuteMethod;
        }

        protected DelegateCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }


        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        protected async Task Execute(object parameter)
        {
            await _executeMethod(parameter);
        }

        protected bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }


        public event EventHandler CanExecuteChanged;

    }

}