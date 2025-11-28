using System.Windows.Input;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SoftPhone;

public partial class Setting : Window
{
    public Setting()
    {
        InitializeComponent();
    }
}

public class SettingViewModel : BaseViewModel
{
    public SettingViewModel()
    {
        SaveCommand = new RelayCommand(Save);
        NoSoundDevice = AppConfig.Instance.NoSoundDevice;
    }

    public bool NoSoundDevice
    {
        get => Get<bool>();
        set => Set(value);
    }

    public ICommand SaveCommand { get; }

    async void Save()
    {
        AppConfig.Instance.NoSoundDevice = NoSoundDevice;
        AppConfig.Instance.Save();
        await MessageBoxManager
            .GetMessageBoxStandard("温馨提示",
                "设置已经保存成功，需要重启程序或重启所有话机才能生效",
                ButtonEnum.Ok,
                Icon.Info).ShowAsync();
    }
}