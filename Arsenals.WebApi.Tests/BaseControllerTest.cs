using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Users;
using Arsenals.Infrastructure.Ef;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arsenals.WebApi.Tests;

public class BaseControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;

    protected readonly ArsenalDbContext _context;

    public BaseControllerTest(PostgreSqlTest fixture)
    {
        var options = new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        };
        _factory = new CustomWebApplicationFactory(fixture);
        _client = _factory.CreateClient(options);
        _context = fixture.ArsenalDbContext;
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    protected async Task LoginAsync()
    {
        LoginRequestDto requestDto = new LoginRequestDto()
        {
            UserId = "test",
            Password = "pass"
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/users", requestDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        BaseResponse<LoginResponseDto>? baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<LoginResponseDto>>();

        Assert.NotNull(baseResponse?.Data?.Token);

        // Jwt設定
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", baseResponse.Data.Token);
    }
}
