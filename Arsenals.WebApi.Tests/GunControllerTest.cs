using System;
using System.Net;
using System.Net.Http.Json;
using Arsenals.ApplicationServices.Guns.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arsenals.WebApi.Tests;

[Collection("TestContainer Collection")]
public class GunControllerTest : IClassFixture<PostgreSqlTest>, IDisposable
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GunControllerTest(PostgreSqlTest fixture)
    {
        var options = new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = true
        };
        _factory = new CustomWebApplicationFactory(fixture);
        _client = _factory.CreateClient(options);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    [Fact]
    public async void fetch_guns_empty()
    {
        using HttpResponseMessage response = await _client.GetAsync("/api/guns");

        //var jsonResponse = await response.Content.ReadFromJsonAsync<BaseResponse<IEnumerable<GunDto>>>();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
