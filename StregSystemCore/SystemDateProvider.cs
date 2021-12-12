using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    internal class SystemDateProvider : IDateProvider
    {
        public SystemDateProvider() { }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
