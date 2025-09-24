using System;

public class Joiner : Person, ISkill
{
    public void PerformSkill()
    {
        Console.WriteLine($"{GetFullName()} вміє множити великі числа (Joiner).");
    }
}
