﻿namespace FakeWebApp.Api;

public sealed class Employee
{
    public int ID { get; set; }
    public string Name { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public EmployeeType EmployeeType { get; set; } = EmployeeType.Values.None;
    public int[] Departments { get; set; } = new int[0];
    public Product CurrentProduct { get; set; } = new Product();
    public IDictionary<string, int> Rates { get; set; } = new Dictionary<string, int>();
    public Status Status { get; set; }
    public TimeSpan TimeEmployed { get; set; }
    public DateOnly DateHired { get; set; }
    public TimeOnly StartTime { get; set; }
}
