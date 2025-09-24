public class Student : Person
{
    public string StudentId { get; set; }
    public int Course { get; set; }
    public string Gender { get; set; }
    public string Residence { get; set; }
    public string GradeBookNumber { get; set; }

    public void Study()
    {
        // логіка навчання
    }

    public override string ToString()
    {
        return $"Student: {GetFullName()}, Id={StudentId}, Course={Course}, " +
               $"Gender={Gender}, Residence={Residence}, GradeBook={GradeBookNumber}";
    }
}
