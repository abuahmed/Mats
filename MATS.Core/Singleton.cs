using MATS.Core.Enumerations;
using MATS.Core.Models;

namespace MATS.Core
{
    public class Singleton
    {
        private static Singleton _instance;
        private Singleton() { }

        public static Singleton Instance
        {
            get { return _instance ?? (_instance = new Singleton()); }
        }

        public static ProductActivationDTO ProductActivation { get; set; }

        public static UserDTO User { get; set; }

        public static MATSEdition Edition { get; set; }

        public static string SqlceFileName { get; set; }
    }
}
