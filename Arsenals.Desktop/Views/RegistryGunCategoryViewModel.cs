using System.Windows.Input;
using Arsenals.ApplicationServices.Guns;
using Prism.Commands;

namespace Arsenals.Desktop.Views;

public class RegistryGunCategoryViewModel : Prism.Mvvm.BindableBase
{
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;

    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value, nameof(Name));
    }

    public ICommand OnClickSubmitButton { get; private set; }

    public RegistryGunCategoryViewModel(RegistryGunCategoryApplicationService registryGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));
        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;

        OnClickSubmitButton = new DelegateCommand(async () =>
        {
            RegistryGunCategoryRequestDto requestDto = new RegistryGunCategoryRequestDto()
            {
                Name = Name
            };

            RegistryGunCategoryResponseDto responseDto = await _registryGunCategoryApplicationService.ExecuteAsync(requestDto);
            Console.WriteLine(responseDto);
        });
    }
}
