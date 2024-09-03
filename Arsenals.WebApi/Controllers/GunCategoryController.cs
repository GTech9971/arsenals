using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/categories")]
[ApiController]
public class GunCategoryController : ControllerBase
{
    private readonly FetchGunCategoryApplicationService _fetchGunCategoryApplicationService;
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;

    public GunCategoryController(FetchGunCategoryApplicationService fetchGunCategoryApplicationService,
                                    RegistryGunCategoryApplicationService registryGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchGunCategoryApplicationService, nameof(fetchGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));

        _fetchGunCategoryApplicationService = fetchGunCategoryApplicationService;
        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<GunCategoryDto>>>> FetchAllAsync()
    {
        IAsyncEnumerable<GunCategoryDto> data = _fetchGunCategoryApplicationService.ExecuteAsync();

        if (await data.AnyAsync() == false) { return NoContent(); }

        return Ok(BaseResponse<IEnumerable<GunCategoryDto>>.CreateSuccess(await data.ToListAsync()));
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
