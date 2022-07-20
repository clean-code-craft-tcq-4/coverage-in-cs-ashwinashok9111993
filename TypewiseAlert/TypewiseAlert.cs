using System;
using System.Collections.Generic;
using System.Linq;

namespace TypewiseAlert
{
    public class TypewiseAlert
    {
        public enum BreachType
        {
            NORMAL,
            TOO_LOW,
            TOO_HIGH
        };
        public enum CoolingType
        {
            PASSIVE_COOLING,
            HI_ACTIVE_COOLING,
            MED_ACTIVE_COOLING
        };

        public static Dictionary<BreachType, Func<double, double, double, bool>> BreachCondition = new Dictionary<BreachType, Func<double, double, double, bool>>()
        {
            {BreachType.NORMAL,(value, lowerlimit,upperlimit) => ((value > lowerlimit) && (value<upperlimit))},
            {BreachType.TOO_LOW,(value, lowerlimit,upperlimit) => (value <= lowerlimit) },
            {BreachType.TOO_HIGH,(value, lowerlimit,upperlimit) => ( value >= upperlimit)}
        };

        public static Dictionary<CoolingType, List<double>> CoolingLimits = new Dictionary<CoolingType, List<double>>()
        {
            {CoolingType.PASSIVE_COOLING,new List<double>(){ 0,35 } },
            {CoolingType.HI_ACTIVE_COOLING,new List<double>(){0,45 } },
            {CoolingType.MED_ACTIVE_COOLING,new List<double>(){0,40 } }
        };

        public static BreachType inferBreach(double value, double lowerLimit, double upperLimit)
        {
            return (from x in BreachCondition where (x.Value(value, lowerLimit, upperLimit)) select x.Key).ToList()[0];
        }

        public static BreachType classifyTemperatureBreach(
           CoolingType coolingType, double temperatureInC)
        {
            return inferBreach(temperatureInC, CoolingLimits[coolingType][0], CoolingLimits[coolingType][1]);
        }
        public enum AlertTarget
        {
            TO_CONTROLLER,
            TO_EMAIL
        };
        public struct BatteryCharacter
        {
            public CoolingType coolingType;
            public string brand;
        }
        public static void checkAndAlert(
            AlertTarget alertTarget, BatteryCharacter batteryChar, double temperatureInC)
        {

            BreachType breachType = classifyTemperatureBreach(
              batteryChar.coolingType, temperatureInC
            );

            switch (alertTarget)
            {
                case AlertTarget.TO_CONTROLLER:
                    sendToController(breachType);
                    break;
                case AlertTarget.TO_EMAIL:
                    sendToEmail(breachType);
                    break;
            }
        }
        public static void sendToController(BreachType breachType)
        {
            const ushort header = 0xfeed;
            Console.WriteLine("{} : {}\n", header, breachType);
        }
        public static void sendToEmail(BreachType breachType)
        {
            string recepient = "a.b@c.com";
            switch (breachType)
            {
                case BreachType.TOO_LOW:
                    Console.WriteLine("To: {}\n", recepient);
                    Console.WriteLine("Hi, the temperature is too low\n");
                    break;
                case BreachType.TOO_HIGH:
                    Console.WriteLine("To: {}\n", recepient);
                    Console.WriteLine("Hi, the temperature is too high\n");
                    break;
                case BreachType.NORMAL:
                    break;
            }
        }
    }
}
