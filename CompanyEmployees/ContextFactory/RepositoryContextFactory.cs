using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;


namespace CompanyEmployees.ContextFactory;

/// <summary>
/// RepositoryContextFactory registers the 'RepositoryContext' at design time,
/// this is required while executing migrations.
/// </summary>
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
	public RepositoryContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();
        // specify MigrationsAssembly as migration assembly is not in our main project, but in the 'Repository' project
		var builder = new DbContextOptionsBuilder<RepositoryContext>()
			.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("CompanyEmployees"));

		return new RepositoryContext(builder.Options);
	}
}
