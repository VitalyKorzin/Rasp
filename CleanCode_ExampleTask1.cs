using System;

public class CleanCode_ExampleTask1
{
    public static int Clamp(int currentValue, int minimumValue, maximumValue)
    {
        if (currentValue < minimumValue)
            return minimumValue;
        else if (currentValue > maximumValue)
            return maximumValue;
        else
            return currentValue;
    }
}