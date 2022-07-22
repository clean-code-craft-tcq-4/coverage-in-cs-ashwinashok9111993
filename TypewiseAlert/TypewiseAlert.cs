using System;
using System.Collections.Generic;
using System.Linq;

namespace TypewiseAlert
{
    public class TypewiseAlert
    {

        public static Dictionary<string, Func<double, double, double, bool>> BreachType = new Dictionary<string, Func<double, double, double, bool>>()
        {
            {"NORMAL",(value, lowerlimit,upperlimit) => ((value > lowerlimit) && (value<upperlimit))},
            {"TOO_LOW",(value, lowerlimit,upperlimit) => (value <= lowerlimit) },
            {"TOO_HIGH",(value, lowerlimit,upperlimit) => ( value >= upperlimit)}
        };

        public static Dictionary<string, List<double>> CoolingType = new Dictionary<string, List<double>>()
        {
            {"PASSIVE_COOLING",new List<double>(){ 0,35 } },
            {"HI_ACTIVE_COOLING",new List<double>(){0,45 } },
            {"MED_ACTIVE_COOLING",new List<double>(){0,40 } }
        };

        public static Dictionary<string, Action<string>> AlertTarget = new Dictionary<string, Action<string>>()
        {
            {"TO_CONTROLLER", (breachType) => Console.WriteLine("0x0feed : {}\n", breachType) },
            {"TO_EMAIL", (breachType) => { 
                if(!breachType.Equals("NORMAL")) Console.WriteLine("To: a.b@com \n Hi,The temperature is too {}",breachType); }}
        };

        public static string inferBreach(double value, double lowerLimit, double upperLimit)
        {
            return (from x in BreachType where (x.Value(value, lowerLimit, upperLimit)) select x.Key).ToList()[0];
        }

        public static string classifyTemperatureBreach(
           string coolingType, double temperatureInC)
        {
            return inferBreach(temperatureInC, CoolingType[coolingType][0], CoolingType[coolingType][1]);
        }

        public static void checkAndAlert(
            string alertTarget, string coolingType, double temperatureInC)
        {
            AlertTarget[alertTarget].Invoke(classifyTemperatureBreach(coolingType, temperatureInC));
        }
    }
}
