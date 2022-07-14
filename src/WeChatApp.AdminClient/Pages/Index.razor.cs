using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace WeChatApp.AdminClient.Pages
{
    public partial class Index : ComponentBase
    {
        private object _option = new
        {
            Title = new
            {
                Left = "center",
                Text = "任务统计"
            },
            Tooltip = new { },
            Legend = new
            {
                Right = "20px",
                Data = new[] { "数量" }
            },
            XAxis = new
            {
                Data = new[] { "周一", "周二", "周三", "周四", "周五", "周六", "周日" }
            },
            YAxis = new { },
            Series = new[]
            {
                new
                {
                    Name= "数量",
                    Type= "bar",
                    Data= new []{ 5, 20, 36, 10, 10, 20, 29 }
                }
            }
        };
    }
}