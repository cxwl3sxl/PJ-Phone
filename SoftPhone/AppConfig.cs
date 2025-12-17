using IniParser;
using System.IO;
using IniParser.Model;

namespace SoftPhone
{
    internal class AppConfig
    {
        #region static

        public static AppConfig Instance { get; }

        static AppConfig()
        {
            Instance = new AppConfig();
        }

        #endregion

        private readonly FileIniDataParser _parser = new FileIniDataParser();
        private readonly IniData _data;

        private AppConfig()
        {
            var isNew = false;
            if (!File.Exists("config.ini"))
            {
                File.Create("config.ini").Close();
                isNew = true;
            }

            _data = _parser.ReadFile("config.ini");
            if (!isNew) return;
            _data["UI"]["Audio"] = "false";
            Save();
        }

        public bool NoSoundDevice
        {
            get => _data["UI"]["Audio"] == "false";
            set => _data["UI"]["Audio"] = value ? "false" : "true";
        }

        public void Save()
        {
            _parser.WriteFile("config.ini", _data);
        }
    }
}
