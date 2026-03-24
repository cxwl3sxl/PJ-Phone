using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SoftPhone;

public partial class PhoneView : UserControl, IDisposable
{
    private readonly PhoneViewModel? _viewModel;

    public PhoneView()
    {
        InitializeComponent();
    }

    public PhoneView(PhoneProfile profile) : this()
    {
        Profile = profile;
        DataContext = _viewModel = new PhoneViewModel(profile);
    }

    public PhoneProfile? Profile { get; private set; }

    public event EventHandler? OnRemoveRequest;

    private async void RemoveMe_OnClick(object? sender, RoutedEventArgs e)
    {
        var result = await MessageBoxManager
            .GetMessageBoxStandard("温馨提示", "确定要移除该分机吗？", ButtonEnum.YesNo, Icon.Question)
            .ShowAsync();
        if (result == ButtonResult.No) return;
        _viewModel?.Dispose();
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

    public void Dispose()
    {
        _viewModel?.Dispose();
    }

    private async void OpenRecordingDir(object? sender, PointerPressedEventArgs e)
    {
        if (Profile?.Number == null) return;
        var dir = Path.Combine("recording", Profile.Number);
        if (!Directory.Exists(dir))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("温馨提示", "录音文件夹不存在，请重新启动应用", ButtonEnum.Ok, Icon.Warning)
                .ShowAsync();
            return;
        }

        dir = Path.GetFullPath(dir);

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows 使用 explorer
                Process.Start("explorer.exe", dir);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // macOS 使用 open
                Process.Start("open", dir);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Linux 常见桌面环境使用 xdg-open
                Process.Start("xdg-open", dir);
            }
        }
        catch
        {
            await MessageBoxManager
                .GetMessageBoxStandard("温馨提示", $"打开录音文件所在目录出错，请手动打开\n位于：{dir}", ButtonEnum.Ok, Icon.Warning)
                .ShowAsync();
        }
    }
}