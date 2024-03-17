using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;

namespace Shared.ResponseFeatures;

/// <summary>
/// Transfers required parameters from our controller to the service layer and avoid the installation of an additional NuGet package inside the Service and Service.Contracts projects
/// </summary>
/// <param name="EmployeeParameters"></param>
/// <param name="Context"></param>
public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
