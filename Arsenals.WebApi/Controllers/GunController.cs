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
    public async Task<ActionResult<List<GunDto>>> FetchAllAsync([FromQuery] int? category)
    {
        List<GunDto> results = await _fetchAllGunApplicationService.ExecuteAsync(category);

        if (results.Any() == false)
        {
            //TODO レスポンス
            return NoContent();
        }

        return Ok(results);
    }
}

