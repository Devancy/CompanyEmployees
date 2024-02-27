using Contracts;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger) : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService = new(() => new CompanyService(repositoryManager, logger));
        private readonly Lazy<IEmployeeService> _employeeService = new(() => new EmployeeService(repositoryManager, logger));

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
    }

}
