using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WeChatApp.Shared.FormBody;
using BlazorComponent;
using Masa.Blazor;
using WeChatApp.AdminClient.Services;
using Masa.Blazor.Presets;
using WeChatApp.Shared;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.AdminClient.Extensions;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Temp;

namespace WeChatApp.AdminClient.Pages
{
    public partial class WorkTaskManager : ComponentBase
    {
        [Inject] IPopupService PopupService { get; set; } = default!;
        [Inject] IWorkTaskService WorkTaskService { get; set; } = default!;


        private List<EnumItem<WorkPublishType>> WorkTaskTypeList = WeChatApp.AdminClient.Extensions.FormatExtensions.ToEnumList<WorkPublishType>().ToList();
        private List<EnumItem<WorkTaskTypes>> WorkTaskTypeDropList = WeChatApp.AdminClient.Extensions.FormatExtensions.ToEnumList<WorkTaskTypes>().ToList();


        protected string _search = string.Empty;

        protected int _totalCount = 0;
        protected IEnumerable<WorkTaskDto> _workTasks = new List<WorkTaskDto>();
        protected bool _loading = true;
        protected DataOptions _options = new DataOptions()
        {
            Page = 1,
            ItemsPerPage = 10
        };
        protected int _serial = 1;

        protected bool _dialog = false;
        protected bool _dialogDelete = false;

        protected int _editedIndex = -1;

        public string FormTitle
        {
            get
            {
                return _editedIndex == -1 ? "添加" : "编辑";
            }
        }

        protected DateOnly _picker = DateOnly.FromDateTime(DateTime.Now);

        AlertTypes type = 0;
        protected string _message = string.Empty;
        ToastPosition position = ToastPosition.TopRight;
        void Show(AlertTypes t = 0)
        {
            type = t;
            _dialogDelete = true;
        }

        async Task AlertShow(AlertTypes t = 0)
        {
            type = t;
            await PopupService.ToastAsync(_message, type);
        }

        protected override async Task OnInitializedAsync()
        {
            await GetDataFromApi();

            await GetDepartmentTreeWithUserFromApiAsync();

            await PopupService.ConfigToast(config =>
            {
                config.Position = ToastPosition.TopRight;
            });

            await base.OnInitializedAsync();
        }

        protected WorkTaskDto _editedItem = new();

        public void Close()
        {
            _dialog = false;
            _editedItem = new();
            _editedIndex = -1;
        }

        public async Task Save()
        {
            if (_editedIndex > -1)
            {
                var result = await WorkTaskService.EditWorkTaskAsync<WcResponse<WorkTaskDto>>(_editedItem);
                _message = result.Message ?? "";
                await AlertShow(result.Code == WcStatus.Success ? AlertTypes.Success : AlertTypes.Error);
            }
            else
            {
                //_users.Append(_editedItem);
                var result = await WorkTaskService.AddWorkTaskAsync<WcResponse<WorkTaskDto>>(_editedItem);
                _message = result.Message ?? "";
                await AlertShow(result.Code == WcStatus.Success ? AlertTypes.Success : AlertTypes.Error);
            }
            await GetDataFromApi();
            Close();
        }

        public async Task AddItem()
        {
            _editedItem = new();
            _editedIndex = -1;
            _dialog = true;
        }

        public void EditItem(WorkTaskDto item)
        {
            _editedIndex = 1;
            _editedItem = new()
            {
                Id = item.Id,
                DepartmentId = item.DepartmentId,
                Title = item.Title,
                Content = item.Content,
                MaxPickUpCount = item.MaxPickUpCount,
                WorkPublishType = item.WorkPublishType,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                PointsRewards = item.PointsRewards,
                PickUpUserIds = item.PickUpUserIds,
                PickUpUserNames = item.PickUpUserNames,
                Type = item.Type,
            };

            _dates[0] = DateOnly.FromDateTime(_editedItem.StartTime);
            _dates[1] = DateOnly.FromDateTime(_editedItem.EndTime);

            _departmentWithUserKeys = _editedItem.PickUpUserIds is null || _editedItem.PickUpUserIds == String.Empty ? new() : _editedItem.PickUpUserIds.Split(',').ToList().Select(x => Guid.Parse(x)).ToList();

            _dialog = true;
        }

        public void DeleteItem(WorkTaskDto item)
        {
            _editedIndex = _workTasks.ToList().IndexOf(item);
            _editedItem = new WorkTaskDto()
            {
                Id = item.Id,
            };
            _dialogDelete = true;
        }

        public async Task DeleteItemConfirm()
        {
            //_users.ToList().RemoveAt(_editedIndex);
            var result = await WorkTaskService.DeleteWorkTaskAsync<WcResponse<WorkTaskDto>>(_editedItem);
            _message = result.Message ?? "";
            await AlertShow(result.Code == WcStatus.Success ? AlertTypes.Success : AlertTypes.Error);
            CloseDelete();
            await GetDataFromApi();
        }

        public void CloseDelete()
        {
            _dialogDelete = false;
            _editedItem = new();
            _editedIndex = -1;
        }
        protected List<DataTableHeader<WorkTaskDto>> _headers = new List<DataTableHeader<WorkTaskDto>>
        {
            new() { Text = "序号", Align = "center", Value = "serial", Sortable = false },
            new() { Text = "任务类型", Align = "center", Value = nameof(WorkTaskDto.Type), Sortable = false },
            new() { Text = "任务名称", Align = "center", Value = nameof(WorkTaskDto.Title), Sortable = false },
            new() { Text = "任务内容", Align = "center", Value = nameof(WorkTaskDto.Content), Sortable = false },
            new() { Text = "时间", Align = "center", Value = "time", Sortable = false},
            new() { Text = "积分", Align = "center", Value = nameof(WorkTaskDto.PointsRewards), Sortable = false },
            new() { Text = "任务接取", Align = "center", Value = "pick", Sortable = false },
            new() { Text = "任务状态", Align = "center", Value = nameof(WorkTaskDto.Status), Sortable = false },
            new() { Text = "创建人", Align = "center", Value = nameof(WorkTaskDto.CreateUserName), Sortable = false },
            new() { Text = "创建时间", Align = "center", Value = nameof(WorkTaskDto.CreateTime), Sortable = false },
            new() { Text = "公开节点", Align = "center", Value = nameof(WorkTaskDto.IsPublicNodes), Sortable = false },
            new() { Text = "详情", Align = "center", Value = "details", Sortable = false },
            new() { Text = "操作", Align = "center", Value = "actions", Sortable = false }
        };

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
                _workTasks = data.items;
                _totalCount = data.total;
                _loading = false;

                await InvokeAsync(StateHasChanged);
            });
        }

        public async Task<(IEnumerable<WorkTaskDto> items, int total)> ApiCallAsync()
        {
            var sortBy = _options.SortBy;
            var sortDesc = _options.SortDesc;
            _parameter.Page = _options.Page;
            _parameter.PageSize = _options.ItemsPerPage;

            return await GetWorkTasksFromApi();
        }

        protected WorkTaskDtoParameters _parameter = new()
        {
            OrderBy = "CreateTime desc,Title",
        };

        protected async Task<(IEnumerable<WorkTaskDto>, int)> GetWorkTasksFromApi()
        {
            var result = await WorkTaskService.GetWorkTaskListAsync<WcResponse<IEnumerable<WorkTaskDto>>>(_parameter);

            if (result.Code == WcStatus.Success)
            {
                return (result.Data!, result.TotalCount);
            }
            return (new List<WorkTaskDto>(), 0);
        }

        protected async Task<WorkTaskDto> HandelDetail(Guid id)
        {
            return new();
        }

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

        #region Date
        public string DateRangeText => string.Join(" ~ ", _dates.Select(date => date.ToString("yyyy-MM-dd")));
        private bool _menu;
        private List<DateOnly> _dates = new List<DateOnly>
        {
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now)
        };
        public void HandleOutsideClick()
        {
            _editedItem.StartTime = _dates[0].ToDateTime(new TimeOnly());
            _editedItem.EndTime = _dates[1].ToDateTime(new TimeOnly());
        }
        #endregion Date

        #region 获取部门人员树状结构
        [Inject] IDepartmentService DepartmentService { get; set; } = default!;

        private List<TreeItem> _departmentWithUsers = new();

        private List<Guid> _departmentWithUserKeys = new();

        private async Task GetDepartmentTreeWithUserFromApiAsync()
        {
            var result = await DepartmentService.GetDepartmentTreeWithUserAsync<WcResponse<IEnumerable<TreeItem>>>();

            if (result.Code == WcStatus.Success)
            {
                _departmentWithUsers = result.Data!.ToList();
            }
            else
            {
                _message = result.Message ?? "";
                await PopupService.ToastAsync(_message, AlertTypes.Error);
            }
        }
        #endregion 获取部门人员树状结构
    }
}