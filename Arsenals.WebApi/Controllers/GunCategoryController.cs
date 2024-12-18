using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Models;
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

    /// <summary>
    /// 銃カテゴリー登録
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<RegistryGunCategoryResponseModel>> RegistryAsync([FromBody] RegistryGunCategoryRequestModel request)
    {
        try
        {
            RegistryGunCategoryResponseModel response = await _registryGunCategoryApplicationService.ExecuteAsync(request);
            Response.Headers.Location = $"/categories/{response.Data?.Id}";
            return this.Created(response);
        }
        catch (DuplicateGunCategoryNameException ex)
        {
            return BadRequest(new RegistryGunCategoryResponseModel() { Error = new BaseResponseErrorModel() { Message = ex.Message } });
        }
    }
}
