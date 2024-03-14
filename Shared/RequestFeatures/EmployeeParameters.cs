namespace Shared.RequestFeatures;
public class EmployeeParameters : RequestParameters
{
    public EmployeeParameters() => OrderBy = "name"; // enable 'name' as the default sorting condition
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue;
    public bool ValidAgeRange => MaxAge > MinAge;
    public string? SearchTerm { get; set; }
    
}
