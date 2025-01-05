using Arsenals.ApplicationServices.Bullets;
using Arsenals.Domains.Bullets.Exceptions;
using Arsenals.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("v1/bullets")]
[ApiController]
public class BulletController : ControllerBase
{
    private readonly RegistryBulletApplicationService _registryBulletApplicationService;

    public BulletController(RegistryBulletApplicationService registryBulletApplicationService)
    {
        ArgumentNullException.ThrowIfNull(registryBulletApplicationService, nameof(registryBulletApplicationService));

        _registryBulletApplicationService = registryBulletApplicationService;
    }

    /// <summary>
    /// 弾丸登録
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<RegistryBulletResponseModel>> RegistryAsync([FromBody] RegistryBulletRequestModel request)
    {
        try
        {
            RegistryBulletResponseModel response = await _registryBulletApplicationService.ExecuteAsync(request);
            Response.Headers.Location = $"bullets/{response.Data?.Id}";
            return this.Created(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new RegistryBulletResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
        catch (DuplicateBulletNameException ex)
        {
            return BadRequest(new RegistryBulletResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
    }
}
