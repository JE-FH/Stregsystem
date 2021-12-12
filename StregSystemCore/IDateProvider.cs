using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public interface IDateProvider
    {
        DateTime GetCurrentDateTime();
    }
}
