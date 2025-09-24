using System;

public class Photographer : Person, ISkill
{
    public void PerformSkill()
    {
        Console.WriteLine($"{GetFullName()} вміє множити великі числа (Photographer).");
    }
}
