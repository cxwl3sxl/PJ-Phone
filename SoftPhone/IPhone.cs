using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone
{
    public interface IPhone : IDisposable
    {
        void Login();
    }
}
