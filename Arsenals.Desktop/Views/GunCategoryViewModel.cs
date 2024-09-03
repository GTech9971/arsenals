using Arsenals.ApplicationServices.Guns;
using Arsenals.ApplicationServices.Guns.Dto;
using Arsenals.Desktop.Views.Models;
using Arsenals.Domains.Guns.Exceptions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Arsenals.Desktop.Views;

public partial class GunCategoryViewModel : ObservableObject
{
    private readonly FetchGunCategoryApplicationService _fetchGunCategoryApplicationService;
    private readonly RegistryGunCategoryApplicationService _registryGunCategoryApplicationService;
    private readonly DeleteGunCategoryApplicationService _deleteGunCategoryApplicationService;

    public GunCategoryViewModel(FetchGunCategoryApplicationService fetchGunCategoryApplicationService,
                                    RegistryGunCategoryApplicationService registryGunCategoryApplicationService,
                                    DeleteGunCategoryApplicationService deleteGunCategoryApplicationService)
    {
        ArgumentNullException.ThrowIfNull(fetchGunCategoryApplicationService, nameof(fetchGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(registryGunCategoryApplicationService, nameof(registryGunCategoryApplicationService));
        ArgumentNullException.ThrowIfNull(deleteGunCategoryApplicationService, nameof(deleteGunCategoryApplicationService));

        _fetchGunCategoryApplicationService = fetchGunCategoryApplicationService;
        _registryGunCategoryApplicationService = registryGunCategoryApplicationService;
        _deleteGunCategoryApplicationService = deleteGunCategoryApplicationService;
    }

    /// <summary>
    /// 登録する名前
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private string _registryName = "";

    [ObservableProperty]
    private IEnumerable<GunCategoryDto> _categories = [];



    /// <summary>
    /// 登録可能かどうか
    /// </summary>
    private bool CanExecuteSubmit => string.IsNullOrWhiteSpace(RegistryName) == false;

    /// <summary>
    /// 削除可能かどうか
    /// </summary>
    private bool CanExecuteDelete => SelectedCategory != null;

    /// <summary>
    /// 選択済みのカテゴリー
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private GunCategoryDto? _selectedCategory;




    /// <summary>
    /// 登録
    /// </summary>
    /// <returns></returns>
    [RelayCommand(CanExecute = nameof(CanExecuteSubmit))]
    private async Task SubmitAsync()
    {
        RegistryGunCategoryRequestDto request = new RegistryGunCategoryRequestDto() { Name = RegistryName };
        try
        {
            RegistryGunCategoryResponseDto response = await _registryGunCategoryApplicationService.ExecuteAsync(request);
            WeakReferenceMessenger.Default.Send(new SuccessMessage($"登録に成功しました。ID:{response.Id}"));
            RegistryName = "";
            await FetchCategoryAsync();
        }
        catch (DuplicateGunCategoryNameException ex)
        {
            WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
        }
    }

    /// <summary>
    /// 選択したカテゴリーを削除する
    /// </summary>
    /// <returns></returns>
    [RelayCommand(CanExecute = nameof(CanExecuteDelete))]
    private async Task DeleteAsync()
    {
        ArgumentNullException.ThrowIfNull(SelectedCategory, nameof(SelectedCategory));

        try
        {
            await _deleteGunCategoryApplicationService.ExecuteAsync(SelectedCategory.Id);
            WeakReferenceMessenger.Default.Send(new SuccessMessage($"削除に成功しました。ID:{SelectedCategory.Id}"));
            SelectedCategory = null;
            await FetchCategoryAsync();
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
        }
    }



    public async Task FetchCategoryAsync()
    {
        IAsyncEnumerable<GunCategoryDto> categories = _fetchGunCategoryApplicationService.ExecuteAsync();
        Categories = await categories
                            .ToListAsync();
    }
}
