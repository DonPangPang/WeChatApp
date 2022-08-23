using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] private IWorkTaskService WorkTaskService { get; set; } = default!;
        [Inject] private IPopupService PopupService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetFromApi();
                StateHasChanged();
            }
        }

        private WorkTaskIndexParameters _parameters = new()
        {
            Page = 1,
            PageSize = 10
        };

        private Response _pageData = new();

        private async Task GetFromApi()
        {
            var result = await WorkTaskService.GetWorkTaskIndex<WcResponse<Response>>(_parameters);

            if (result.Code == WcStatus.Success)
            {
                _pageData = result.Data!;
            }
            else
            {
                await PopupService.ToastErrorAsync("获取数据失败");
            }
        }

        public class Response
        {
            public int PickingTaskCount { get; set; }
            public int ActiveTaskCount { get; set; }
            public int TotalTaskCount { get; set; }
            public int EndTaskCount { get; set; }

            public List<TaskAndNode> Result { get; set; } = new();
        }

        public class TaskAndNode
        {
            public WorkTaskDto WorkTask { get; set; }
            public WorkTaskNodeDto Node { get; set; }
        }

        //private object _option = new
        //{
        //    Title = new
        //    {
        //        Left = "center",
        //        Text = "任务统计"
        //    },
        //    Tooltip = new { },
        //    Legend = new
        //    {
        //        Right = "20px",
        //        Data = new[] { "数量" }
        //    },
        //    XAxis = new
        //    {
        //        Data = new[] { "周一", "周二", "周三", "周四", "周五", "周六", "周日" }
        //    },
        //    YAxis = new { },
        //    Series = new[]
        //    {
        //        new
        //        {
        //            Name= "数量",
        //            Type= "bar",
        //            Data= new []{ 5, 20, 36, 10, 10, 20, 29 }
        //        }
        //    }
        //};
    }
}