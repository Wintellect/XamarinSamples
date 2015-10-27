using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace PropertyDependencyDemo.Mvvm
{
    /// <summary>
    /// Defines an <see cref="T:System.Windows.Input.ICommand"/> implementation wrapping an <see cref="System.Action"/>.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        bool _inFlight;
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;
        readonly Func<Task> _task;

        /// <summary>
        /// Occurs when the target of the Command should reevaluate whether or not the Command can be executed.
        /// </summary>
        /// 
        /// <remarks/>
        public event EventHandler CanExecuteChanged;

        internal DelegateCommand(Action<object> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
        }

        internal DelegateCommand(Action execute)
            : this(o => execute())
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
        }

        internal DelegateCommand(Func<Task> task)
            : this(task, null)
        {
        }

        internal DelegateCommand(Func<Task> task, Func<object, bool> canExecute)
        {
            _task = task;
            _canExecute = canExecute;
        }

        async void InvokeCommandTask(Task commandTask)
        {
            _inFlight = true;
            //ChangeCanExecute(); // TODO: Bug in Xamarin Forms 1.3 causes this to crash Android when command is bound to a context action
            try {
                await commandTask;
            } finally {
                _inFlight = false;
                //ChangeCanExecute(); // TODO: Bug in Xamarin Forms 1.3 causes this to crash Android when command is bound to a context action
            }
        }

        internal DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
            : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
            _canExecute = canExecute;
        }

        internal DelegateCommand(Action execute, Func<bool> canExecute)
            : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
        }

        /// <param name="parameter">An <see cref="T:System.Object"/> used as parameter for the execute Action.</param>
        /// <summary>
        /// Invokes the execute Action
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// <para>
        /// If the Command was created with non-generic execute parameter, the parameter of this method is ignored.
        /// </para>
        /// 
        /// </remarks>
        public void Execute(object parameter)
        {
            if (_task == null) {
                _execute (parameter);
                return;
            }
            InvokeCommandTask (_task ());
        }

        /// <param name="parameter">An <see cref="T:System.Object"/> used as parameter to determine if the Command can be executed.</param>
        /// <summary>
        /// Returns a <see cref="T:System.Boolean"/> indicating if the Command can be exectued with the given parameter.
        /// </summary>
        /// 
        /// <returns>
        /// <see langword="true"/> if the Command can be executed, <see langword="false"/> otherwise.
        /// </returns>
        /// 
        /// <remarks>
        /// 
        /// <para>
        /// If no canExecute parameter was passed to the Command constructor, this method always returns <see langword="true"/>.
        /// </para>
        /// 
        /// <para>
        /// If the Command was created with non-generic execute parameter, the parameter of this method is ignored.
        /// </para>
        /// 
        /// </remarks>
        public bool CanExecute(object parameter)
        {
            if (_inFlight)
            {
                return false;
            }
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Send a <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged"/>
        /// </summary>
        /// 
        /// <remarks/>
        public void ChangeCanExecute()
        {
            EventHandler eventHandler = CanExecuteChanged;
            if (eventHandler == null)
                return;
            eventHandler(this, EventArgs.Empty);
        }
    }

    /// <typeparam name="T">The Type of the parameter,</typeparam>
    /// <summary>
    /// Defines an <see cref="T:System.Windows.Input.ICommand"/> implementation wrapping a generic Action&lt;T&gt;.
    /// </summary>
    public sealed class DelegateCommand<T> : DelegateCommand
    {
        internal DelegateCommand(Action<T> execute)
            : base(o => execute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
        }

        internal DelegateCommand(Func<Task<T>> task)
            : base(task)
        {
        }

        internal DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
            : base(o => execute((T)o), o => canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
        }

        internal DelegateCommand(Func<Task<T>> task, Func<T, bool> canExecute)
            : base(task, o => canExecute((T)o))
        {
        }
    }
}

