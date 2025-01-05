using System.Net;
using System.Net.Http.Json;
using Arsenals.Domains.Guns;
using Arsenals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arsenals.WebApi.Tests.Guns;

/// <summary>
/// 銃画像アップロードAPIのテスト
/// </summary>
public class UploadGunImageControllerTest : BaseControllerTest
{
    public UploadGunImageControllerTest(PostgreSqlTest fixture) : base(fixture) { }

    [Fact(DisplayName = "画像アップロード")]
    public async Task upload()
    {
        using var scope = _factory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        string? root = configuration[GunImage.ROOT_KEY];
        string? downloadRoot = configuration[GunImage.DOWNLOAD_KEY];
        Assert.NotNull(root);
        Assert.NotNull(downloadRoot);


        string fileName = "sample.jpg";
        string fileContent = "sample file content";

        using var fileStream = new MemoryStream();
        using var writer = new StreamWriter(fileStream);

        await writer.WriteAsync(fileContent);
        await writer.FlushAsync();
        fileStream.Position = 0;

        using var fileUploadContent = new StreamContent(fileStream);
        fileUploadContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

        using var formData = new MultipartFormDataContent();
        formData.Add(fileUploadContent, "data", fileName);

        string categoryId = await RegistryCategoryAsync();
        string gunId = await RegistryGunAsync(gunName: "M911A1", categoryId);

        HttpResponseMessage response = await _client.PostAsync($"/api/v1/arsenals/guns/{gunId}/images", formData);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);


        //DB
        var gun = await _context.Guns
                                    .AsNoTracking()
                                    .Include(x => x.GunImageData)
                                    .SingleAsync(x => x.Id == gunId);

        Assert.Equal(1, gun.GunImageDataId);
        Assert.NotNull(gun.GunImageData);
        Assert.Equal(1, gun.GunImageData.Id);
        Assert.Equal(".jpg", gun.GunImageData.Extension);

        // ファイル
        string path = Path.Combine(root, gunId, "1.jpg");
        Assert.True(File.Exists(path));

        // データ取得
        using var fetchAllResponse = await _client.GetAsync($"/api/v1/arsenals/guns");
        Assert.Equal(HttpStatusCode.OK, fetchAllResponse.StatusCode);

        FetchGunsResponseModel? fetchGunsResponseModel = await fetchAllResponse.Content.ReadFromJsonAsync<FetchGunsResponseModel>();
        Assert.NotNull(fetchGunsResponseModel?.Data);
        Assert.Single(fetchGunsResponseModel.Data);

        Assert.Equal($"{downloadRoot}/{gunId}/1.jpg", fetchGunsResponseModel.Data.Single().ImageUrl);
    }
}
