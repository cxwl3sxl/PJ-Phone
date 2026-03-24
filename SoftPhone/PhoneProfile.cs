namespace SoftPhone
{
    public class PhoneProfile : BaseViewModel
    {
        public string? Number
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? Name
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? Password
        {
            get => Get<string>();
            set => Set(value);
        }

        public string? Server
        {
            get => Get<string>();
            set => Set(value);
        }

        public int? Port
        {
            get => Get<int>();
            set => Set(value);
        }

        public bool Record
        {
            get => Get<bool>();
            set => Set(value);
        }

        public string? AutoPlay
        {
            get => Get<string>();
            set => Set(value);
        }

        public override string ToString()
        {
            return $"{Name}-{Number}";
        }
    }
}
