using System;
using System.Net;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Users;

namespace Arsenals.WebApi.Tests;

public class UserControllerTest : BaseControllerTest
{
    public UserControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact]
    public async void login()
    {
        LoginRequestDto requestDto = new LoginRequestDto()
        {
            UserId = "test",
            Password = "pass"
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/api/users", requestDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        BaseResponse<LoginResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<LoginResponseDto>>();

        Assert.NotNull(baseResponse);
        Assert.NotNull(baseResponse.Data);
        Assert.NotNull(baseResponse.Data.Token);
    }
}
