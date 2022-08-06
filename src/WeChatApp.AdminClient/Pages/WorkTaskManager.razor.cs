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
using WeChatApp.Shared.Entity;

namespace WeChatApp.AdminClient.Pages
{
    public partial class WorkTaskManager : ComponentBase
    {
        [Inject] private IPopupService PopupService { get; set; } = default!;
        [Inject] private IWorkTaskService WorkTaskService { get; set; } = default!;

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

        private AlertTypes type = 0;
        protected string _message = string.Empty;
        private ToastPosition position = ToastPosition.TopRight;

        private void Show(AlertTypes t = 0)
        {
            type = t;
            _dialogDelete = true;
        }

        private async Task AlertShow(AlertTypes t = 0)
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
                Nodes = item.Nodes,
            };

            _workTaskDate = DateOnly.FromDateTime(_editedItem.EndTime);

            _departmentWithUserKeys = _editedItem.PickUpUserIds is null || _editedItem.PickUpUserIds == String.Empty ? new() : _editedItem.PickUpUserIds.Split(',').Where(x=>x != string.Empty).ToList().Select(x => Guid.Parse(x)).ToList();

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
            new() { Text = "难度级别", Align = "center", Value = nameof(WorkTaskDto.Level), Sortable = false},
            new() { Text = "任务类型", Align = "center", Value = nameof(WorkTaskDto.WorkPublishType), Sortable = false },
            new() { Text = "发布类型", Align = "center", Value = nameof(WorkTaskDto.Type), Sortable = false },
            new() { Text = "任务名称", Align = "center", Value = nameof(WorkTaskDto.Title), Sortable = false },
            new() { Text = "任务内容", Align = "center", Value = nameof(WorkTaskDto.Content), Sortable = false },
            new() { Text = "结束时间", Align = "center", Value = "time", Sortable = false},
            new() { Text = "积分", Align = "center", Value = nameof(WorkTaskDto.PointsRewards), Sortable = false },
            new() { Text = "任务接取", Align = "center", Value = "pick", Sortable = false },
            new() { Text = "任务状态", Align = "center", Value = nameof(WorkTaskDto.Status), Sortable = false },
            new() { Text = "创建人", Align = "center", Value = nameof(WorkTaskDto.CreateUserName), Sortable = false },
            new() { Text = "创建时间", Align = "center", Value = nameof(WorkTaskDto.CreateTime), Sortable = false },
            //new() { Text = "公开节点", Align = "center", Value = nameof(WorkTaskDto.IsPublicNodes), Sortable = false },
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

        private bool _menu;

        private DateOnly _workTaskDate = DateOnly.FromDateTime(DateTime.Now);

        private void HandleWorkTaskMenuOk()
        {
            _menu = false;
            _editedItem.EndTime = _workTaskDate.ToDateTime(new TimeOnly());
        }

        private void HandleWorkTaskMenuCancel()
        {
            _menu = false;
        }

        #endregion Date

        #region 获取部门人员树状结构

        [Inject] private IDepartmentService DepartmentService { get; set; } = default!;

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

        #region 添加节点

        private int _editNodeIndex = -1;

        public string NodeFormTitle
        {
            get
            {
                return _editNodeIndex == -1 ? "添加" : "编辑";
            }
        }

        private bool _isShowNodeModel = false;
        private WorkTaskNode _node = new();

        private bool _nodeModelMenu = false;
        private DateOnly _nodeDate = DateOnly.FromDateTime(DateTime.Now);

        private void EditWorkTaskNode(WorkTaskNode node)
        {
            _editNodeIndex = 0;
            _node = node;
            _isShowNodeModel = true;

            _nodeDate = DateOnly.FromDateTime(node.NodeTime ?? DateTime.Now);
        }

        private void DeleteWorkTaskNode(WorkTaskNode node)
        {
            _editedItem.Nodes!.Remove(node);
        }

        private async Task HandleOnSaveNode()
        {
            if (_nodeDate > _workTaskDate)
            {
                await PopupService.ToastAsync("选择日期不在任务时间区间内", AlertTypes.Error);
                return;
            }
            _node.NodeTime = _nodeDate.ToDateTime(new TimeOnly());

            if (_editNodeIndex == -1)
            {
                _editedItem.Nodes!.Add(_node);
            }

            _isShowNodeModel = false;

            _node = new();
        }

        private void HandleOnCancelNode()
        {
            _isShowNodeModel = false;

            _node = new();
        }

        private void HandleOnAddWorkTaskNode()
        {
            _editNodeIndex = -1;
            _isShowNodeModel = true;

            //_node.NodeTime = _nodeDate.ToDateTime(new TimeOnly());
        }

        private void NodeModelMenuCancel()
        {
            _nodeModelMenu = false;
        }

        private async Task NodeModelMenuOk()
        {
            //_node.NodeTime = _nodeDate.ToDateTime(new TimeOnly());
            _nodeModelMenu = false;
        }

        #endregion 添加节点
    }
}