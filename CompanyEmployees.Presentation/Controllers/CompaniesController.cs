using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
	private readonly IServiceManager _service = service;

	[HttpGet]
	public IActionResult GetCompanies()
    {
		var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
		return Ok(companies);
	}

	[HttpGet("{id:guid}")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

}
