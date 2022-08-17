using Microsoft.AspNetCore.Components;
using WeChatApp.Shared.Enums;
using Index = WeChatApp.AdminClient.Pages.Index;

namespace WeChatApp.AdminClient.Components;

public partial class RecentTaskListComponent:ComponentBase
{
    [Parameter]
    public List<Index.TaskAndNode> Data { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private string GetIcon(WorkTaskStatus status)
    {
        return status switch
        {
            WorkTaskStatus.PendingReview => "./images/daishen.png",
            WorkTaskStatus.PendingPublish => "./images/daishen.png",
            WorkTaskStatus.Assign => "./images/zhipai.png",
            WorkTaskStatus.Overrule => "./images/bohui.png",
            WorkTaskStatus.Publish => "./images/yifabu.png",
            WorkTaskStatus.Active => "./images/jinxing.png",
            WorkTaskStatus.Finished => "./images/yiwan.png",
            WorkTaskStatus.Grade => "./images/end.png",
            WorkTaskStatus.End => "./images/end.png",
            WorkTaskStatus.None or _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}