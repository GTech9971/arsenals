using System.Security.Claims;
using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Authorize]
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<BaseResponse<RegistryGunCategoryResponseDto>>> RegistryAsync([FromBody] RegistryGunCategoryRequestDto request)
    {
        if (request == null) { return BadRequest(); }

        try
        {
            //TODO テスト用にユーザーID取得
            var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            RegistryGunCategoryResponseDto response = await _registryGunCategoryApplicationService.ExecuteAsync(request);
            return this.Created(BaseResponse<RegistryGunCategoryResponseDto>.CreateSuccess(response));
        }
        catch (DuplicateGunCategoryNameException ex)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
    }
}
