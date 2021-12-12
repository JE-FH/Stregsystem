using StregsystemCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCoreTest
{
    internal class StaticDateProvider : IDateProvider
    {
        DateTime _dateToProvide;
        public StaticDateProvider(DateTime dateToProvide)
        {
            _dateToProvide = dateToProvide;
        }

        public DateTime GetCurrentDateTime()
        {
            return _dateToProvide;
        }
    }
}
