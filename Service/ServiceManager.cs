using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IEmployeeLinks employeeLinks) : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService = new(() => new CompanyService(repositoryManager, logger, mapper));
    private readonly Lazy<IEmployeeService> _employeeService = new(() => new EmployeeService(repositoryManager, logger, mapper, employeeLinks));

    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}
