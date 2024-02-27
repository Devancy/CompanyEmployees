using Contracts;
using Service.Contracts;

namespace Service
{
    internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger) : ICompanyService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
    }

}
