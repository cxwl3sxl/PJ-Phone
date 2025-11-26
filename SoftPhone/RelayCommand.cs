using System;
using System.Windows.Input;

namespace SoftPhone;

/// <summary>
/// 新建一个无参命令绑定
/// </summary>
public class RelayCommand : ICommand, IRelayCommand
{
    private readonly Action _theAction;
    private readonly Func<bool>? _canExecuteFunc;

    /// <summary>
    /// 新建一个无参函数的绑定
    /// </summary>
    /// <param name="action">待绑定的方法</param>
    /// <param name="canExecuteFunc">是否可执行判断方法，传递此参数，需要模型主动调用<see cref="RaiseCanExecuteChanged"/>方法进行UI更新</param>
    public RelayCommand(Action action, Func<bool>? canExecuteFunc = null)
    {
        _theAction = action ?? throw new ArgumentNullException(nameof(action));
        _canExecuteFunc = canExecuteFunc;
    }


    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        return _canExecuteFunc?.Invoke() != false;
    }


    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        _theAction();
    }

    /// <summary>
    /// 当可否执行发生变更时触发
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