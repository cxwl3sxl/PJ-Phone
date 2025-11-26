using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SoftPhone;

public partial class PhoneView : UserControl
{
    private readonly PhoneViewModel? _viewModel;

    public PhoneView()
    {
        InitializeComponent();
    }

    public PhoneView(PhoneProfile profile) : this()
    {
        Profile = profile;
        DataContext = _viewModel = new PhoneViewModel(profile, Phone);
    }

    public IPhone? Phone { get; }

    public PhoneProfile? Profile { get; private set; }

    public event EventHandler? OnRemoveRequest;

    private async void RemoveMe_OnClick(object? sender, RoutedEventArgs e)
    {
        var result = await MessageBoxManager
            .GetMessageBoxStandard("温馨提示", "确定要移除该分机吗？", ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();
        if (result == ButtonResult.No) return;
        Phone?.Dispose();
        OnRemoveRequest?.Invoke(this, EventArgs.Empty);
    }

    private void Setting_OnClick(object? sender, RoutedEventArgs e)
    {
        new EditProfile(Profile!, profile =>
        {
            Profile = profile;
            _viewModel?.UpdateProfile(profile);
        }).ShowDialog((VisualRoot as Window)!);
    }
}