using System;
using System.IO;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;

namespace SoftPhone;

public partial class EditProfile : Window
{
    private readonly Action<PhoneProfile>? _afterSave;
    private readonly Func<bool>? _beforeSave;
    private readonly PhoneProfile? _profile;

    public EditProfile()
    {
        InitializeComponent();
    }

    public EditProfile(Action<PhoneProfile> afterSave) : this()
    {
        Title = "新增配置";
        _afterSave = afterSave;
        DataContext = _profile = new PhoneProfile();
        _beforeSave = () =>
            !File.Exists(Path.Combine(MainWindow.ProfileDir, $"{_profile.Server}@{_profile.Number}.json"));
    }

    public EditProfile(PhoneProfile profile, Action<PhoneProfile> afterSave) : this()
    {
        Title = "编辑配置";
        _afterSave = afterSave;
        DataContext = _profile = profile;
        var oldFile = Path.Combine(MainWindow.ProfileDir, $"{_profile.Server}@{_profile.Number}.json");
        _beforeSave = () =>
        {
            var newFile = Path.Combine(MainWindow.ProfileDir, $"{_profile.Server}@{_profile.Number}.json");
            if (oldFile == newFile) return true;

            if (File.Exists(newFile)) return false;
            File.Delete(oldFile);
            return true;
        };
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_beforeSave?.Invoke() == false)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("温馨提示", "同服务器下已注册相同分级，无法重复注册！")
                .ShowAsync();
            return;
        }

        var file = Path.Combine(MainWindow.ProfileDir, $"{_profile!.Server}@{_profile.Number}.json");
        await File.WriteAllTextAsync(file, JsonSerializer.Serialize(_profile));
        _afterSave?.Invoke(_profile);
        Close();
    }
}