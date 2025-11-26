using Avalonia.Controls;

namespace SoftPhone;

public partial class PhoneView : UserControl
{
    public PhoneView()
    {
        InitializeComponent();
    }

    public PhoneView(PhoneProfile profile) : this()
    {
        DataContext = profile;
    }

    public IPhone? Phone { get; }
}