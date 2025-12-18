using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SoftPhone;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        Title.Content =
            $"品杰软电话(版本：v{Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version})";
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}