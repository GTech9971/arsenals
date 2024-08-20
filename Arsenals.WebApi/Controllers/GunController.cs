using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/guns")]
[ApiController]
public class GunController : ControllerBase
{
    private readonly FetchAllGunApplicationService _fetchAllGunApplicationService;

    public GunController(FetchAllGunApplicationService fetchAllGunApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchAllGunApplicationService, nameof(fetchAllGunApplicationService));

        _fetchAllGunApplicationService = fetchAllGunApplicationService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<GunDto>>>> FetchAllAsync([FromQuery] int? category)
    {
        IAsyncEnumerable<GunDto> results = _fetchAllGunApplicationService.Execute(category);

        if (await results.AnyAsync() == false)
        {
            return NoContent();
        }

        return Ok(new BaseResponse<IEnumerable<GunDto>>(await results.ToListAsync()));
    }
}

