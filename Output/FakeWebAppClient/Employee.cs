// Generated Code
namespace FakeWebAppClient;
public sealed partial class Employee
{
    public int ID { get; set; }

    public string Name { get; set; } = "";
    public DateTime BirthDate { get; set; }

    public EmployeeType EmployeeType { get; set; } = EmployeeType.Values.GetDefault();
    public int[] Departments { get; set; } = new int[0];
    public Product CurrentProduct { get; set; } = new Product();
    public IDictionary<string, int> Rates { get; set; } = new Dictionary<string, int>();
}