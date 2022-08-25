using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorComponent;
using Masa.Blazor;
using Masa.Blazor.Presets;
using Microsoft.AspNetCore.Components;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Pages
{
    public partial class SorceManager : ComponentBase
    {
        [Inject] private IBonusPointRecordService BonusPointRecordService { get; set; } = default!;

        [Inject] private IPopupService PopupService { get; set; } = default!;

        private BonusPointRecord _editedItem = new();
        private List<BonusPointRecordDto> _records = new();
        protected string _search = string.Empty;
        protected int _totalCount = 0;
        protected DataOptions _options = new DataOptions()
        {
            Page = 1,
            ItemsPerPage = 10
        };
        protected bool _loading = true;


        protected int _serial = 1;

        protected bool _dialog = false;
        protected bool _dialogDelete = false;
        private AlertTypes type = 0;
        private void Show(AlertTypes t = 0)
        {
            type = t;
            _dialogDelete = true;
        }

        protected int _editedIndex = -1;

        public string FormTitle
        {
            get
            {
                return _editedIndex == -1 ? "添加" : "编辑";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetDataFromApi();

            await PopupService.ConfigToast(config =>
            {
                config.Position = ToastPosition.TopRight;
            });

            await base.OnInitializedAsync();
        }


        #region Init

        public async Task HandleOnOptionsUpdate(DataOptions options)
        {
            _options = options;
            await GetDataFromApi();
        }

        public async Task GetDataFromApi()
        {
            _loading = true;
            await ApiCallAsync().ContinueWith(async (task) =>
            {
                var data = await task;
                _records = data.items.ToList();
                _totalCount = data.total;
                _loading = false;

                await InvokeAsync(StateHasChanged);
            });
        }

        public async Task<(IEnumerable<BonusPointRecordDto> items, int total)> ApiCallAsync()
        {
            var sortBy = _options.SortBy;
            var sortDesc = _options.SortDesc;
            _parameter.Page = _options.Page;
            _parameter.PageSize = _options.ItemsPerPage;

            return await GetRecordsFromApi();
        }

        protected ParameterBase _parameter = new()
        {
            OrderBy = "CreateTime desc",
        };

        protected async Task<(IEnumerable<BonusPointRecordDto>, int)> GetRecordsFromApi()
        {
            var result = await BonusPointRecordService.GetBonusPointRecordListAsync<WcResponse<IEnumerable<BonusPointRecordDto>>>(_parameter)!;

            if (result.Code == WcStatus.Success)
            {
                return (result.Data!, result.TotalCount);
            }
            return (new List<BonusPointRecordDto>(), 0);
        }

        #endregion

        #region Table
        // header

        protected List<DataTableHeader<BonusPointRecordDto>> _headers = new List<DataTableHeader<BonusPointRecordDto>>()
        {
            new (){Text = "序号", Align="center", Value="serial", Sortable = false},
            new (){Text = "得分人", Align="center", Value=nameof(BonusPointRecordDto.PickUpUserName), Sortable=false},
            new() { Text = "积分", Align = "center", Value = nameof(BonusPointRecordDto.BonusPoints), Sortable = false },
            new() { Text = "评分人", Align = "center", Value = nameof(BonusPointRecordDto.CreateUserName), Sortable = false },
            new() { Text = "评分时间", Align = "center", Value = nameof(BonusPointRecordDto.CreateTime), Sortable = false },
            new() { Text = "操作", Align = "center", Value = "actions", Sortable = false }
        };
        #endregion

        #region Search
        protected async Task HandleSearch()
        {
            _parameter.Q = _search;
            await GetDataFromApi();
        }

        protected async Task HandleSearchChange(string val)
        {
            _parameter.Q = val;
            await GetDataFromApi();
        }

        #endregion

        #region Edit
        private void AddItem()
        {

        }

        private void EditItem(BonusPointRecordDto dto)
        {

        }
        #endregion
        public void CloseDelete()
        {
            _dialogDelete = false;
            _editedItem = new();
            _editedIndex = -1;
        }

        public async Task DeleteItemConfirm()
        {
            //_users.ToList().RemoveAt(_editedIndex);
            // var result = await WorkTaskService.DeleteWorkTaskAsync<WcResponse<WorkTaskDto>>(_editedItem);
            // _message = result.Message ?? "";
            // await AlertShow(result.Code == WcStatus.Success ? AlertTypes.Success : AlertTypes.Error);
            CloseDelete();
            await GetDataFromApi();
        }
    }
}