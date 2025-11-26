namespace SoftPhone;

internal interface IRelayCommand
{
    void RaiseCanExecuteChanged();
}