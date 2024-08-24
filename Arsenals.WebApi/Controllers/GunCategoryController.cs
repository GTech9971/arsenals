using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/categories")]
[ApiController]
public class GunCategoryController : ControllerBase
{
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;

    public GunCategoryController(RegistryGunCategoryApplicationService registryGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));

        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RegistryGunCategoryResponseDto>>> RegistryAsync([FromBody] RegistryGunCategoryRequestDto request)
    {
        if (request == null) { return BadRequest(); }

        try
        {
            RegistryGunCategoryResponseDto response = await _registryGunCategoryApplicationService.ExecuteAsync(request);
            return this.Created(BaseResponse<RegistryGunCategoryResponseDto>.CreateSuccess(response));
        }
        catch (DuplicateGunCategoryNameException ex)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
    }
}
