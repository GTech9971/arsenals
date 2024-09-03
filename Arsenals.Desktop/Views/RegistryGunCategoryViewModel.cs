using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Domains.Guns.Exceptions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arsenals.Desktop.Views;

public partial class RegistryGunCategoryViewModel : ObservableObject
{
    private readonly FetchGunCategoryApplicationService _fetchGunCategoryApplicationService;
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private IEnumerable<string> _gunNames = [];


    public async Task FetchCategoryAsync()
    {
        IAsyncEnumerable<GunCategoryDto> categories = _fetchGunCategoryApplicationService.ExecuteAsync();
        GunNames = await categories
                            .Select(x => x.Name)
                            .ToListAsync();
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        RegistryGunCategoryRequestDto request = new RegistryGunCategoryRequestDto() { Name = Name };
        try
        {
            RegistryGunCategoryResponseDto response = await _registryGunCategoryApplicationService.ExecuteAsync(request);
        }
        catch (DuplicateGunCategoryNameException ex)
        {
            //TODO
        }
    }

    public RegistryGunCategoryViewModel(FetchGunCategoryApplicationService fetchGunCategoryApplicationService,
                                            RegistryGunCategoryApplicationService registryGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchGunCategoryApplicationService, nameof(fetchGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));

        _fetchGunCategoryApplicationService = fetchGunCategoryApplicationService;
        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;
    }
}
