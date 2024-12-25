using Arsenals.Infrastructure.Migrations.Batch;

namespace Arsenals.Infrastructure.Migration.Batch.Tests;

public class ProgramTest
{
    [Fact(DisplayName = "マイグレーションのテスト")]
    public void migration()
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        int actual = Program.Main([]);

        Assert.Equal(0, actual);
    }
}
