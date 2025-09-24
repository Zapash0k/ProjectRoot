using System;

public class ConsoleMenu
{
    private FileRepository repository;

    public ConsoleMenu(FileRepository repo)
    {
        repository = repo;
    }

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n1. Додати студента");
            Console.WriteLine("2. Показати всіх студентів");
            Console.WriteLine("3. Обчислити студенток 5-го курсу з Києва");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ShowAll();
                    break;
                case "3":
                    CountFemaleFromKyiv();
                    break;
                case "0":
                    return;
            }
        }
    }

    private void AddStudent()
    {
        Student st = new Student();
        Console.Write("Прізвище: ");
        st.LastName = Console.ReadLine();
        Console.Write("Ім’я: ");
        st.FirstName = Console.ReadLine();
        Console.Write("Курс: ");
        st.Course = int.Parse(Console.ReadLine());
        Console.Write("Студентський квиток: ");
        st.StudentId = Console.ReadLine();
        Console.Write("Стать (female/male): ");
        st.Gender = Console.ReadLine();
        Console.Write("Місце проживання: ");
        st.Residence = Console.ReadLine();
        Console.Write("Номер залікової книжки: ");
        st.GradeBookNumber = Console.ReadLine();

        repository.Save(st);
    }

    private void ShowAll()
    {
        Student[] students = repository.Load();
        foreach (var st in students)
        {
            Console.WriteLine(st);
        }
    }

    private void CountFemaleFromKyiv()
    {
        Student[] students = repository.Load();
        int count = 0;
        foreach (var st in students)
        {
            if (st.Course == 5 && st.Gender.ToLower() == "female" &&
                st.Residence.ToLower() == "київ")
            {
                Console.WriteLine(st);
                count++;
            }
        }
        Console.WriteLine($"Кількість студенток 5-го курсу з Києва: {count}");
    }
}
