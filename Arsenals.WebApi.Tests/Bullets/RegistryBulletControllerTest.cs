using System.Net;
using System.Net.Http.Json;
using Arsenals.Models;
using Microsoft.EntityFrameworkCore;

namespace Arsenals.WebApi.Tests.Bullets;

public class RegistryBulletControllerTest : BaseControllerTest
{
    public RegistryBulletControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact(DisplayName = "弾丸名が被っている")]
    public async void duplicate_name()
    {
        await RegistryBulletAsync("9mm");

        RegistryBulletRequestModel request = new RegistryBulletRequestModel()
        {
            Name = "9mm",
            Damage = 3
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/bullets", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        RegistryBulletResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryBulletResponseModel>();
        Assert.Null(responseModel?.Data);
    }

    [Fact(DisplayName = "弾丸登録")]
    public async void success()
    {
        RegistryBulletRequestModel request = new RegistryBulletRequestModel()
        {
            Name = "9mm",
            Damage = 3
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/bullets", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        RegistryBulletResponseModel? responseModel = await response.Content.ReadFromJsonAsync<RegistryBulletResponseModel>();
        Assert.NotNull(responseModel?.Data);
        Assert.Equal($"bullets/{responseModel.Data.Id}", response.Headers.Location?.ToString());
        Assert.Equal("B-0001", responseModel.Data.Id);

        var bullet = await _context.Bullets
                                    .AsNoTracking()
                                    .SingleAsync();

        Assert.Equal("B-0001", bullet.Id);
        Assert.Equal(request.Name, bullet.Name);
        Assert.Equal(request.Damage, bullet.Damage);
    }
}
