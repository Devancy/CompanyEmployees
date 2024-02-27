using Contracts;

namespace Repository
{
    public sealed class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext = repositoryContext;
        private readonly Lazy<ICompanyRepository> _companyRepository = new(() => new CompanyRepository(repositoryContext));
        private readonly Lazy<IEmployeeRepository> _employeeRepository = new(() => new EmployeeRepository(repositoryContext));

        public ICompanyRepository Company => _companyRepository.Value;
        public IEmployeeRepository Employee => _employeeRepository.Value;
        public void Save() => _repositoryContext.SaveChanges();
    }
}
