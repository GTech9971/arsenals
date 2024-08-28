using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Arsenals.ApplicationServices.Users;
using Arsenals.Domains.Users;
using Arsenals.Domains.Users.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arsenals.WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly LoginUserApplicationService _loginUserApplicationService;
    private readonly JwtHandler _jwtHandler;

    public UserController(LoginUserApplicationService loginUserApplicationService,
                            JwtHandler jwtHandler)
    {
        ArgumentNullException.ThrowIfNull(loginUserApplicationService, nameof(loginUserApplicationService));
        ArgumentNullException.ThrowIfNull(jwtHandler, nameof(jwtHandler));

        _loginUserApplicationService = loginUserApplicationService;
        _jwtHandler = jwtHandler;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<LoginResponseDto>>> LoginAsync(LoginRequestDto requestDto)
    {
        try
        {
            await _loginUserApplicationService.ExecuteAsync(requestDto);
            JwtSecurityToken secToken = await _jwtHandler.GetTokenAsync(requestDto.UserId);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);

            return Ok(BaseResponse<LoginResponseDto>.CreateSuccess(new LoginResponseDto() { Token = jwt }));
        }
        catch (ArgumentException ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
        {
            return BadRequest(BaseResponse<object?>.CreateError(ex));
        }
        catch (UserNotFoundException ex)
        {
            return Unauthorized(BaseResponse<object?>.CreateError(ex));
        }
        catch (InvalidCredentialException ex)
        {
            return Unauthorized(BaseResponse<object?>.CreateError(ex));
        }
    }
}

