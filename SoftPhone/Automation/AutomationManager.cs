using System.Linq;

namespace SoftPhone.Automation
{
    internal class AutomationManager
    {
        private readonly Worker[] _workers;

        public AutomationManager(string group, bool isCaller, MainWindow mainWindow)
        {
            var configsInGroup = AppConfig.Instance.GetAutomation(group);
            _workers = configsInGroup
                .Select(item => new Worker(item, isCaller, mainWindow.GetPhone(item.Number!)))
                .ToArray();
        }

        public void Start()
        {
            foreach (var worker in _workers)
            {
                worker.Start();
            }
        }

        public void Stop()
        {
            foreach (var worker in _workers)
            {
                worker.Stop();
            }
        }
    }
}
