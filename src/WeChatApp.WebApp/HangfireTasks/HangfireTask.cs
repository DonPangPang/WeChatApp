﻿using Hangfire;
using WeChatApp.Shared.Extensions;

namespace WeChatApp.WebApp.HangfireTasks
{
    /// <summary>
    /// </summary>
    public static class HangfireTask
    {
        /// <summary>
        /// </summary>
        public static void HangfireSevices()
        {
            //Fire - And - forget（发布 / 订阅）
            //这是一个主要的后台任务类型，持久化消息队列会去处理这个任务。当你创建了一个发布 / 订阅任务，该任务会被保存到默认队列里面（默认队列是"Default"，但是支持使用多队列）。多个专注的工作者（Worker）会监听这个队列，并且从中获取任务并且完成任务。
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

            //延迟
            //如果想要延迟某些任务的执行，可以是用以下任务。在给定延迟时间后，任务会被排入队列，并且和发布 / 订阅任务一样执行。
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromDays(1));

            //循环
            //按照周期性（小时，天等）来调用方法，请使用RecurringJob类。在复杂的场景，您可以使用CRON表达式指定计划时间来处理任务。
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Daily Job"), Cron.Daily);

            //连续
            //连续性允许您通过将多个后台任务链接在一起来定义复杂的工作流。
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));

            RecurringJob.AddOrUpdate<WorkTaskJob>(x => x.Execute(), Cron.Daily, TimeZoneInfo.Local);
        }

        public static void AddTimingJob(DateTime date, Action action)
        {
            var days = Math.Abs(date.DateDiff(DateTime.Now));
            BackgroundJob.Schedule(() => action(), TimeSpan.FromDays(days));
        }
    }
}