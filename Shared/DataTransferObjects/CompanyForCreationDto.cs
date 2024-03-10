namespace Shared.DataTransferObjects;

public record CompanyForCreationDto : CompanyForManipulationDto
{
    public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
}