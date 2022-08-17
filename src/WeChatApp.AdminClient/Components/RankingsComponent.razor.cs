using BlazorComponent;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using WeChatApp.AdminClient.Extensions;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared;
using WeChatApp.Shared.FormBody;

namespace WeChatApp.AdminClient.Components;

public partial class RankingsComponent : ComponentBase
{
    [Inject] private IDepartmentService DepartmentService { get; set; } = default!;
    [Inject] private IPopupService PopupService { get; set; } = default!;
    [Inject] private IBonusPointRecordService BonusPointRecordService { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetDeptsFromApi();
            await GetRanksFromApi();

            StateHasChanged();
        }
    }

    #region Department

    private List<DepartmentDto> _departments = new();

    private async Task GetDeptsFromApi()
    {
        var result = await DepartmentService.GetDepartmentTreeAsync<WcResponse<IEnumerable<DepartmentDto>>>();

        if (result.Code == WcStatus.Success)
        {
            _departments = result.Data!.ToList();
        }
        else
        {
            await PopupService.ToastErrorAsync("获取部门失败");
        }
    }

    private List<Guid> _activeDept = new();

    private async Task HandleDeptActive(List<Guid> val)
    {
        _activeDept = val;

        await GetRanksFromApi();
    }

    #endregion Department

    #region 获取排名

    private List<Item> _items = new();

    public class Response
    {
        public List<Item> GlobalRank { get; set; } = new();
        public List<Item> DeptRank { get; set; } = new();
    }

    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public float Score { get; set; } = 0;
    }

    private async Task GetRanksFromApi()
    {
        var result = await BonusPointRecordService.GetRankingsAsync<WcResponse<Response>>(_activeDept);

        if (result.IsSuccess())
        {
            _items = result.Data!.DeptRank;
        }
        else
        {
            await PopupService.ToastErrorAsync("获取排名失败");
        }
    }

    #endregion 获取排名
}