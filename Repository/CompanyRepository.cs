using Contracts;
using Entities.Models;

namespace Repository;

public sealed class CompanyRepository : RepositoryBase<Company>,ICompanyRepository
{
	public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
	{
	}
}
