using System;

public class CleanCode_ExampleTask17
{
    public static void CreateNewObject()
    {
        //Создание объекта на карте
    }

    public static void GenerateChance()
    {
        _chance = Random.Range(0, 100);
    }

    public static int CalculateSalary(int hoursWorked)
    {
        return _hourlyRate * hoursWorked;
    }
}