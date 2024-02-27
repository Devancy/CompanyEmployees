using Contracts;
using Entities.Models;

namespace Repository;

public sealed class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
	public EmployeeRepository(RepositoryContext repositoryContext)
		: base(repositoryContext)
	{
	}
}
