using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/guns")]
[ApiController]
public class GunController : ControllerBase
{
    private readonly FetchAllGunApplicationService _fetchAllGunApplicationService;
    private readonly RegistryGunApplicationService _registryGunApplicationService;
    private readonly DeleteGunApplicationService _deleteGunApplicationService;

    public GunController(FetchAllGunApplicationService fetchAllGunApplicationService,
                            RegistryGunApplicationService registryGunApplicationService,
                            DeleteGunApplicationService deleteGunApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchAllGunApplicationService, nameof(fetchAllGunApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunApplicationService, nameof(registryGunApplicationService));
        ArgumentNullException.ThrowIfNull(deleteGunApplicationService, nameof(deleteGunApplicationService));

        _fetchAllGunApplicationService = fetchAllGunApplicationService;
        _registryGunApplicationService = registryGunApplicationService;
        _deleteGunApplicationService = deleteGunApplicationService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<GunDto>>>> FetchAllAsync([FromQuery] int? category)
    {
        IAsyncEnumerable<GunDto> results = _fetchAllGunApplicationService.Execute(category);

        if (await results.AnyAsync() == false)
        {
            return NoContent();
        }

        return Ok(BaseResponse<IEnumerable<GunDto>>.CreateSuccess(await results.ToListAsync()));
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<RegistryGunResponseDto>>> RegistryAsync([FromBody] RegistryGunRequestDto request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        RegistryGunResponseDto responseDto = await _registryGunApplicationService.ExecuteAsync(request);
        return this.Created(BaseResponse<RegistryGunResponseDto>.CreateSuccess(responseDto));
    }

    [HttpDelete("{gunId}")]
    public async Task<ActionResult> DeleteAsync(int gunId)
    {
        try
        {
            await _deleteGunApplicationService.ExecuteAsync(gunId);
            return NoContent();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (GunNotFoundException ex)
        {
            return NotFound(BaseResponse<object?>.CreateError(ex));
        }
    }
}

