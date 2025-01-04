using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Exceptions;
using Arsenals.Domains.Guns.Exceptions;
using Arsenals.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/v1/arsenals/guns")]
[ApiController]
public class GunController : ControllerBase
{
    private readonly FetchAllGunApplicationService _fetchAllGunApplicationService;
    private readonly RegistryGunApplicationService _registryGunApplicationService;
    private readonly FetchGunApplicationService _fetchGunApplicationService;
    private readonly DeleteGunApplicationService _deleteGunApplicationService;
    private readonly UpdateGunApplicationService _updateGunApplicationService;
    private readonly GunImageUploadApplicationService _gunImageUploadApplicationService;

    public GunController(FetchAllGunApplicationService fetchAllGunApplicationService,
                            RegistryGunApplicationService registryGunApplicationService,
                            FetchGunApplicationService fetchGunApplicationService,
                            DeleteGunApplicationService deleteGunApplicationService,
                            UpdateGunApplicationService updateGunApplicationService,
                            GunImageUploadApplicationService gunImageUploadApplicationService,
                            ILogger<GunController> logger)
    {
        ArgumentNullException.ThrowIfNull(fetchAllGunApplicationService, nameof(fetchAllGunApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunApplicationService, nameof(registryGunApplicationService));
        ArgumentNullException.ThrowIfNull(fetchGunApplicationService, nameof(fetchGunApplicationService));
        ArgumentNullException.ThrowIfNull(deleteGunApplicationService, nameof(deleteGunApplicationService));
        ArgumentNullException.ThrowIfNull(updateGunApplicationService, nameof(updateGunApplicationService));
        ArgumentNullException.ThrowIfNull(gunImageUploadApplicationService, nameof(gunImageUploadApplicationService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _fetchAllGunApplicationService = fetchAllGunApplicationService;
        _registryGunApplicationService = registryGunApplicationService;
        _fetchGunApplicationService = fetchGunApplicationService;
        _deleteGunApplicationService = deleteGunApplicationService;
        _updateGunApplicationService = updateGunApplicationService;
        _gunImageUploadApplicationService = gunImageUploadApplicationService;
    }

    /// <summary>
    /// 全ての銃の取得
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<FetchGunsResponseModel>> FetchAllAsync([FromQuery] string? category)
    {
        FetchGunsResponseModel response = await _fetchAllGunApplicationService.ExecuteAsync(category);
        if (response.Data.Any() == false) { return NoContent(); }
        return Ok(response);
    }

    /// <summary>
    /// 銃の登録
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<RegistryGunResponseModel>> RegistryAsync([FromBody] RegistryGunRequestModel request)
    {
        try
        {
            RegistryGunResponseModel responseDto = await _registryGunApplicationService.ExecuteAsync(request);
            Response.Headers.Location = $"guns/{responseDto.Data?.Id}";
            return this.Created(responseDto);
        }
        catch (DuplicateGunNameException ex)
        {
            return BadRequest(new RegistryGunResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new RegistryGunResponseModel() { Error = new ErrorModel() { Message = ex.Message } });
        }
    }

    /// <summary>
    /// 銃の取得
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    [HttpGet("{gunId}")]
    public async Task<ActionResult<BaseResponse<GunDto>>> FetchAsync(string gunId)
    {
        try
        {
            GunDto data = await _fetchGunApplicationService.ExecuteAsync(gunId);
            return Ok(BaseResponse<GunDto>.CreateSuccess(data));
        }
        catch (ArgumentException ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (GunNotFoundException ex)
        {
            return NotFound(BaseResponse<object?>.CreateError(ex));
        }
    }

    /// <summary>
    /// 銃の削除
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    [HttpDelete("{gunId}")]
    public async Task<ActionResult> DeleteAsync(string gunId)
    {
        try
        {
            await _deleteGunApplicationService.ExecuteAsync(gunId);
            return NoContent();
        }
        catch (ArgumentException ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (GunNotFoundException ex)
        {
            return NotFound(BaseResponse<object?>.CreateError(ex));
        }
    }

    /// <summary>
    /// 銃の更新
    /// </summary>
    /// <param name="gunId"></param>
    /// <param name="fieldMask"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{gunId}")]
    public async Task<ActionResult<BaseResponse<GunDto>>> UpdateAsync(string gunId, [FromQuery] IEnumerable<string> fieldMask, [FromBody] UpdateGunRequestDto request)
    {
        try
        {
            GunDto data = await _updateGunApplicationService.ExecuteAsync(gunId, fieldMask, request);
            return Ok(BaseResponse<GunDto>.CreateSuccess(data));
        }
        catch (ArgumentException ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (DuplicateException ex)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (NotFoundException ex)
        {
            return NotFound(BaseResponse<object?>.CreateError(ex));
        }
    }

    /// <summary>
    /// 銃の画像アップロード
    /// </summary>
    /// <param name="gunId"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("{gunId}/images")]
    public async Task<ActionResult<BaseResponse<string>>> UploadGunImageAsync([FromRoute] string gunId, [FromForm] IFormFile data)
    {
        if (data == null || data.Length == 0)
        {
            return BadRequest(BaseResponse<object?>.CreateError("画像ファイルは必須です"));
        }

        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await data.CopyToAsync(memoryStream);
                Uri uri = await _gunImageUploadApplicationService.ExecuteAsync(gunId, memoryStream);
                return this.Created(BaseResponse<string>.CreateSuccess(uri.ToString()));
            }
        }
        catch (ArgumentException ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (NotFoundException ex)
        {
            return NotFound(BaseResponse<object?>.CreateError(ex));
        }
    }
}

