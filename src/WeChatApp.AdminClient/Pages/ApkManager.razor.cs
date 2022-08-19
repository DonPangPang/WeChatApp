using BlazorComponent;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Data.Common;
using WeChatApp.AdminClient.Extensions;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Pages
{
    public partial class ApkManager : ComponentBase
    {
        [Inject] private IApkHistoryService ApkHistoryService { get; set; } = default!;
        [Inject] private IPopupService PopupService { get; set; } = default!;

        protected int _serial = 1;
        protected int _editedIndex = -1;
        protected int _totalCount = 0;
        protected bool _loading = true;

        public string FormTitle
        {
            get
            {
                return _editedIndex == -1 ? "添加" : "编辑";
            }
        }

        private List<DataTableHeader<AppHistoryDto>> _headers = new()
        {
            new() { Text = "序号", Align = "center", Value = "serial", Sortable = false },
            new() { Text = "版本", Align="center", Value = nameof(AppHistoryDto.Version), Sortable = false },
            new() { Text = "描述", Align = "center", Value = nameof(AppHistoryDto.Description), Sortable = false },
            new() { Text = "备注", Align = "center", Value = nameof(AppHistoryDto.Remark), Sortable = false },
            new() { Text = "路径", Align = "center", Value = nameof(AppHistoryDto.Source), Sortable = false },
            new() { Text = "创建人", Align = "center", Value = nameof(AppHistoryDto.CreateUserName), Sortable = false },
            new() { Text = "创建时间", Align = "center", Value = nameof(AppHistoryDto.CreateTime), Sortable = false },
            new() { Text = "操作", Align = "center", Value = "actions", Sortable = false }
        };

        protected override async Task OnInitializedAsync()
        {
            await GetDataFromApi();

            await base.OnInitializedAsync();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await GetAppHistoryFromApi();

        //        StateHasChanged();
        //    }
        //}

        private List<AppHistoryDto> _appHistoryList = new();
        private AppHistoryUploadDto _editItem = new();

        private ParameterBase _parameter = new()
        {
            OrderBy = "CreateTime desc",
        };

        private async Task<(IEnumerable<AppHistoryDto>, int)> GetAppHistoryFromApi()
        {
            var result = await ApkHistoryService.GetPageListAsync<WcResponse<IEnumerable<AppHistoryDto>>>(_parameter);

            if (result.IsSuccess())
            {
                return (result.Data!, result.TotalCount);
            }
            else
            {
                await PopupService.ToastErrorAsync("数据获取异常");

                return (new List<AppHistoryDto>(), 0);
            }
        }

        public async Task<(IEnumerable<AppHistoryDto> items, int total)> ApiCallAsync()
        {
            var sortBy = _options.SortBy;
            var sortDesc = _options.SortDesc;
            _parameter.Page = _options.Page;
            _parameter.PageSize = _options.ItemsPerPage;

            return await GetAppHistoryFromApi();
        }

        public async Task GetDataFromApi()
        {
            _loading = true;
            await ApiCallAsync().ContinueWith(async (task) =>
            {
                var data = await task;
                _appHistoryList = data.items.ToList();
                _totalCount = data.total;
                _loading = false;

                await InvokeAsync(StateHasChanged);
            });
        }

        protected DataOptions _options = new DataOptions()
        {
            Page = 1,
            ItemsPerPage = 20
        };

        public async Task HandleOnOptionsUpdate(DataOptions options)
        {
            _options = options;
            await GetAppHistoryFromApi();
        }

        private string _filePath = string.Empty;

        //private MFileInput<IBrowserFile> MFileInput { get; set; } = default!;

        private IBrowserFile? _browserFile = null;

        private async Task HandleUploadFile(IBrowserFile file)
        {
            _browserFile = file;

            if (file is null)
            {
                return;
            }

            //var file = _files.FirstOrDefault();

            var result = await ApkHistoryService
                .UploadAppVersion<WcResponse<UploadFileDto>>(_browserFile);

            if (result.IsSuccess())
            {
                _filePath = result.Data!.Path;
                await PopupService.ToastSuccessAsync("上传成功");
            }
            else
            {
                await PopupService.ToastErrorAsync("上传文件失败");
                _filePath = string.Empty;
            }
        }

        public class UploadFileDto
        {
            public string Path { get; set; } = string.Empty;
        }

        private void AddVersion()
        {
            _editItem = new();
            _editedIndex = -1;
            _isShowEditModal = true;
        }

        private void EditVersion(AppHistoryDto dto)
        {
        }

        private async Task Save()
        {
            if (_filePath.IsEmpty())
            {
                await PopupService.ToastWarningAsync("请上传安装文件");
                return;
            }
            _editItem.Path = _filePath;

            var result = await ApkHistoryService.AddAppVersionAsync<WcResponse<AppHistoryUploadDto>>(_editItem);

            if (result.IsSuccess())
                await PopupService.ToastSuccessAsync(result.Message);
            else
                await PopupService.ToastErrorAsync(result.Message);

            Close();
            await GetDataFromApi();
        }

        private void Close()
        {
            _isShowEditModal = false;
            _editItem = new();
            _editedIndex = 1;

            _browserFile = null;
        }

        private bool _dialogDelete = false;
        private AlertTypes type = 0;

        private void CloseDelete()
        {
            _dialogDelete = false;
        }

        private AppHistoryDto _chooesItem = new();

        private async Task DeleteItemConfirm()
        {
            var result = await ApkHistoryService.DeleteVersionAsync<WcResponse<AppHistoryUploadDto>>(_chooesItem);

            if (result.IsSuccess())
                await PopupService.ToastSuccessAsync(result.Message);
            else
                await PopupService.ToastErrorAsync(result.Message);

            _dialogDelete = false;

            await GetDataFromApi();
        }

        private void Show(AppHistoryDto dto, AlertTypes t = 0)
        {
            type = t;
            _dialogDelete = true;

            _chooesItem = dto;
        }

        #region 编辑框

        private bool _isShowEditModal = false;

        #endregion 编辑框
    }
}