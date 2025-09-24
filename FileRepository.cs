using System;
using System.IO;
using System.Text.RegularExpressions;

public class FileRepository
{
    private string filePath;

    public FileRepository(string path)
    {
        filePath = path;
        // Якщо файлу нема — створимо порожній, щоб уникнути помилок при читанні
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, string.Empty);
    }

    public void Save(Person person)
    {
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            if (person is Student st)
            {
                sw.WriteLine($"Student {st.GetFullName()}");
                sw.WriteLine("{");
                sw.WriteLine($"\"firstname\": \"{st.FirstName}\",");
                sw.WriteLine($"\"lastname\": \"{st.LastName}\",");
                sw.WriteLine($"\"studentId\": \"{st.StudentId}\",");
                sw.WriteLine($"\"course\": \"{st.Course}\",");
                sw.WriteLine($"\"gender\": \"{st.Gender}\",");
                sw.WriteLine($"\"residence\": \"{st.Residence}\",");
                sw.WriteLine($"\"gradebook\": \"{st.GradeBookNumber}\"");
                sw.WriteLine("};");
            }
        }
    }

    public Student[] Load()
    {
        if (!File.Exists(filePath))
            return new Student[0];

        string[] lines = File.ReadAllLines(filePath);
        Student[] students = new Student[100]; // масив без використання колекцій
        int index = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("Student"))
            {
                Student st = new Student();

                // рухаємося до блоку { ... };
                i++;
                while (i < lines.Length)
                {
                    string jsonLine = lines[i].Trim();

                    if (jsonLine.StartsWith("};"))
                        break;

                    if (jsonLine.StartsWith("{"))
                    {
                        i++;
                        continue;
                    }

                    if (TryGetKeyValue(jsonLine, out string key, out string value))
                    {
                        switch (key.ToLower())
                        {
                            case "firstname":
                                st.FirstName = value;
                                break;
                            case "lastname":
                                st.LastName = value;
                                break;
                            case "studentid":
                                st.StudentId = value;
                                break;
                            case "course":
                                if (!int.TryParse(value, out int courseVal))
                                    courseVal = 0; // або інше значення за замовчуванням
                                st.Course = courseVal;
                                break;
                            case "gender":
                                st.Gender = value;
                                break;
                            case "residence":
                                st.Residence = value;
                                break;
                            case "gradebook":
                                st.GradeBookNumber = value;
                                break;
                        }
                    }

                    i++;
                }

                // динамічне збільшення масиву при потребі
                if (index >= students.Length)
                    Array.Resize(ref students, students.Length + 50);

                students[index++] = st;
            }
        }

        Array.Resize(ref students, index);
        return students;
    }

    // Видалення студента за studentId
    public void Delete(string id)
    {
        var students = Load();
        // очищуємо файл і записуємо назад тільки тих, кого не видаляємо
        File.WriteAllText(filePath, string.Empty);
        foreach (var st in students)
        {
            if (st.StudentId != id)
                Save(st);
        }
    }

    public Person[] FindByLastName(string name)
    {
        var students = Load();
        Person[] result = new Person[students.Length];
        int count = 0;
        foreach (var st in students)
        {
            if (!string.IsNullOrEmpty(st.LastName) &&
                st.LastName.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                result[count++] = st;
            }
        }
        Array.Resize(ref result, count);
        return result;
    }

    public Person FindById(string id)
    {
        var students = Load();
        foreach (var st in students)
        {
            if (!string.IsNullOrEmpty(st.StudentId) && st.StudentId == id)
                return st;
        }
        return null;
    }

    // Допоміжна функція: надійно витягує ключ і значення з рядка виду: "key": "value", або "key": value
    private bool TryGetKeyValue(string line, out string key, out string value)
    {
        key = null;
        value = null;

        // знайдемо ключ (перше у лапках перед двокрапкою)
        var keyMatch = Regex.Match(line, "\"(?<k>[^\"]*)\"\\s*:");
        if (!keyMatch.Success)
            return false;

        key = keyMatch.Groups["k"].Value;

        // знайдемо значення: або "value" або value (без лапок)
        var valMatch = Regex.Match(line, @":\s*""(?<v>[^""]*)""|:\s*(?<v>[^,\}\s]+)");
        if (valMatch.Success)
        {
            value = valMatch.Groups["v"].Value;
            return true;
        }

        return false;
    }
}

