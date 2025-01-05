using Arsenals.ApplicationServices.Guns;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("v1/categories")]
[ApiController]
public class GunCategoryController : ControllerBase
{
    private readonly FetchGunCategoryApplicationService _fetchGunCategoryApplicationService;
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;
    private readonly DeleteGunCategoryApplicationService _deleteGunCategoryApplicationService;

    public GunCategoryController(FetchGunCategoryApplicationService fetchGunCategoryApplicationService,
                                    RegistryGunCategoryApplicationService registryGunCategoryApplicationService,
                                    DeleteGunCategoryApplicationService deleteGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchGunCategoryApplicationService, nameof(fetchGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(deleteGunCategoryApplicationService, nameof(deleteGunCategoryApplicationService));

        _fetchGunCategoryApplicationService = fetchGunCategoryApplicationService;
        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;
        _deleteGunCategoryApplicationService = deleteGunCategoryApplicationService;
    }

    /// <summary>
    /// 銃カテゴリー取得
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<FetchGunCategoryResponseModel>> FetchAllAsync()
    {
        try
        {
            FetchGunCategoryResponseModel responseModel = await _fetchGunCategoryApplicationService.ExecuteAsync();
            if (responseModel.Data.Any() == false) { return NoContent(); }

            return Ok(responseModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new FetchGunCategoryResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
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
            return BadRequest(new RegistryGunCategoryResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
    }

    /// <summary>
    /// 銃カテゴリー削除
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete("{categoryId}")]
    public async Task<ActionResult> DeleteAsync(string categoryId)
    {
        try
        {
            await _deleteGunCategoryApplicationService.ExecuteAsync(categoryId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex);
        }
        catch (GunCategoryNotFoundException ex)
        {
            return NotFound(ex);
        }
    }
}
