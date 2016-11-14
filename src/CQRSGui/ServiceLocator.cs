using SimpleCQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSGui
{
    public static class ServiceLocator
    {
        public static FakeBus Bus { get; set; }
    }
}
