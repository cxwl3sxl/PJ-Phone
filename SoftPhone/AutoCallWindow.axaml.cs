using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SoftPhone;

public partial class AutoCallWindow : Window
{
    private readonly AutoCallWindowViewModel _vm;
    private readonly List<PhoneProfile> _allProfiles = new();

    public AutoCallWindow()
    {
        DataContext = _vm = new AutoCallWindowViewModel();
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        if (Design.IsDesignMode) return;
        var mainWindow = (MainWindow)Owner!;
        foreach (var profile in mainWindow.Profiles)
        {
            _allProfiles.Add(profile);
            _vm.Profiles.Add(profile);
        }

        TryAddGroupItem(AppConfig.Instance.GetAutomation("Call"), _vm.CallGroup);
        TryAddGroupItem(AppConfig.Instance.GetAutomation("Pickup"), _vm.PickupGroup);
        base.OnOpened(e);
    }

    void TryAddGroupItem(IEnumerable<AutoGroupItem> configs, ObservableCollection<AutoGroupItem> workGroup)
    {
        foreach (var item in configs)
        {
            var target = _vm.Profiles.FirstOrDefault(a => a.Number == item.Number);
            if (target == null) continue;
            workGroup.Add(item);
            _vm.Profiles.Remove(target);
        }
    }

    private void AddToPool_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_vm.SelectedNumber)) return;
        var target = _vm.Profiles.FirstOrDefault(a => a.Number == _vm.SelectedNumber);
        if (target == null) return;
        _vm.Profiles.Remove(target);
        if (_vm.ActiveTabIndex == 0)
        {
            _vm.CallGroup.Add(new AutoGroupItem()
            {
                Number = target.Number
            });
        }
        else
        {
            _vm.PickupGroup.Add(new AutoGroupItem()
            {
                Number = target.Number
            });
        }
    }

    private void SaveSetting_OnClick(object? sender, RoutedEventArgs e)
    {
        AppConfig.Instance.SaveAutomation("Call", _vm.CallGroup);
        AppConfig.Instance.SaveAutomation("Pickup", _vm.PickupGroup);
        Close();
    }

    private void RemoveCaller_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: AutoGroupItem autoGroup }) return;
        _vm.CallGroup.Remove(autoGroup);
        var sourceProfile = _allProfiles.FirstOrDefault(a => a.Number == autoGroup.Number);
        if (sourceProfile != null)
        {
            _vm.Profiles.Add(sourceProfile);
        }
    }

    private void RemovePickup_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: AutoGroupItem autoGroup }) return;
        _vm.PickupGroup.Remove(autoGroup);
        var sourceProfile = _allProfiles.FirstOrDefault(a => a.Number == autoGroup.Number);
        if (sourceProfile != null)
        {
            _vm.Profiles.Add(sourceProfile);
        }
    }
}

public class AutoCallWindowViewModel : BaseViewModel
{
    public ObservableCollection<PhoneProfile> Profiles { get; } = new();
    public ObservableCollection<AutoGroupItem> CallGroup { get; } = new();
    public ObservableCollection<AutoGroupItem> PickupGroup { get; } = new();


    public string? SelectedNumber
    {
        get => Get<string>();
        set => Set(value);
    }

    public int ActiveTabIndex
    {
        get => Get<int>();
        set => Set(value);
    }
}

public class AutoGroupItem : BaseViewModel
{
    public string? Number { get; set; }

    public string? TargetNumber
    {
        get => Get<string>();
        set => Set(value);
    }

    public int Delay
    {
        get => Get<int>();
        set => Set(value);
    }

    public override string ToString()
    {
        return $"{Number},{TargetNumber},{Delay}";
    }

    public static bool TryParse(string value, out AutoGroupItem? groupItem)
    {
        groupItem = null;
        var items = value.Split(",");
        if (items.Length != 3) return false;
        groupItem = new AutoGroupItem()
        {
            Number = items[0],
            TargetNumber = items[1],
            Delay = int.Parse(items[2])
        };
        return true;
    }
}