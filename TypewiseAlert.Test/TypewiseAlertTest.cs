using System;
using Xunit;

namespace TypewiseAlert.Test
{
  public class TypewiseAlertTest
  {
    [Fact]
    public void InfersBreachAsPerLimits()
    {
      Assert.True(TypewiseAlert.inferBreach(12, 20, 30) ==
        "TOO_LOW");
    }

    [Fact]
    public void InfersBreachAsPerCoolingLimits()
    {
        Assert.True(TypewiseAlert.classifyTemperatureBreach("PASSIVE_COOLING", 20) ==
            "TOO_LOW");
    }

    }
}
