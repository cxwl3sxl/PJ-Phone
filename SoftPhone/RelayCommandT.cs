using System;
using System.Windows.Input;

namespace SoftPhone
{
    /// <summary>
    /// 新建一个带一个参数的命令绑定
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class RelayCommand<T> : ICommand, IRelayCommand
    {
        private readonly Action<T?> _theAction;
        private readonly Func<T?, bool>? _canExecuteFunc;


        /// <summary>
        /// 新建一个带一个参数的命令绑定
        /// </summary>
        /// <param name="action">具体的方法</param>
        /// <param name="canExecuteFunc">可否执行的判断方法</param>
        public RelayCommand(Action<T?> action, Func<T?, bool>? canExecuteFunc = null)
        {
            _theAction = action ?? throw new ArgumentNullException(nameof(action));
            _canExecuteFunc = canExecuteFunc;
        }


        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            return _canExecuteFunc?.Invoke((T?)parameter) != false;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            _theAction((T?)parameter);
        }

        /// <summary>
        /// 当可否执行发生变更时候触发
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// 触发可否执行变更
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
