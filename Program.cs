using System;

class Program
{
    static void Main()
    {
        FileRepository repo = new FileRepository("students.txt");
        ConsoleMenu menu = new ConsoleMenu(repo);
        menu.Show();
    }
}
