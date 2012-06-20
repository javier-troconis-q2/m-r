using System.Linq;

namespace CQRSGui
{
    public static class ServiceLocator
    {
        public static FakeBus<object> Bus { get; set; }
    }
}