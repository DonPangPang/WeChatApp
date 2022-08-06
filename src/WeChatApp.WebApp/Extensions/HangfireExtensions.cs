using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql;
using System.Data;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.Shared.Options;
using WeChatApp.WebApp.HangfireTasks;

namespace WeChatApp.Shared.Extensions
{
    /// <summary>
    /// </summary>
    public static class HangfireExtensions
    {
        /// <summary>
        /// 添加Hangfire的支持
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddHangfireSupport(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;

            var dbOptions = configuration.GetSection("DbOptions").Get<DbOptions>();

            var hangFire = configuration.GetSection("HangfireOptions").Get<HangfireOptions>();

            var db = dbOptions.DbSettings.FirstOrDefault(x => x.IsEnable);

            //var connectionString = configuration.GetConnectionString("SqlServer");
            ////注册Hangfire定时任务
            //services.AddHangfire(x => x.UseSqlServerStorage(connectionString, new Hangfire.SqlServer.SqlServerStorageOptions
            //{
            //    UseRecommendedIsolationLevel = true, // 事务隔离级别。默认是读取已提交。
            //    QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
            //    JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
            //    CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
            //    PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
            //    DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
            //    TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
            //    SchemaName = HangfireVars.DefaultSchemaName,              //- 数据库表名称。默认值为dbo。

            //}));

            if (hangFire.EnableRedis)
            {
                services.AddHangfire(x => x.UseRedisStorage(hangFire.ConnectionString));
            }
            else
            {
                if (db!.DbType == "MySql")
                {
                    services.AddHangfire(x => x.UseStorage(new MySqlStorage(db.ConnectionString, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = (System.Transactions.IsolationLevel?)IsolationLevel.ReadUncommitted, // 事务隔离级别。默认是读取已提交。
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                        PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                        DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                        TablesPrefix = "Hangfire"                                  //- 数据库中表的前缀。默认为none
                    })));
                }
                else if (db!.DbType == "SqlServer")
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(db.ConnectionString, new Hangfire.SqlServer.SqlServerStorageOptions
                    {
                        UseRecommendedIsolationLevel = true, // 事务隔离级别。默认是读取已提交。
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                        PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                        DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                        SchemaName = HangfireVars.DefaultSchemaName,              //- 数据库表名称。默认值为dbo。
                    }));
                }
            }

            services.AddHangfireServer(options =>
            {
                //options.Queues = new[] { GlobalEnumVars.HangFireQueuesConfig.@default.ToString(), GlobalEnumVars.HangFireQueuesConfig.apis.ToString(), GlobalEnumVars.HangFireQueuesConfig.web.ToString(), GlobalEnumVars.HangFireQueuesConfig.recurring.ToString() };
                options.ServerTimeout = TimeSpan.FromMinutes(4);
                options.SchedulePollingInterval = TimeSpan.FromSeconds(15);//秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                options.ShutdownTimeout = TimeSpan.FromMinutes(30); //超时时间
                options.WorkerCount = Math.Max(Environment.ProcessorCount, 20); //工作线程数，当前允许的最大线程，默认20
            });

            return services;
        }

        /// <summary>
        /// 使用Hangfire
        /// </summary>
        /// <param name="app"> </param>
        /// <returns> </returns>
        public static WebApplication UseHangfire(this WebApplication app)
        {
            #region HangFire

            IConfiguration configuration = app.Services.GetService<IConfiguration>()!;

            var ho = configuration.Get<HangfireOptions>();

            //授权
            var filter = new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    SslRedirect = false,
                    // Require secure connection for dashboard
                    RequireSsl = false,
                    // Case sensitive login checking
                    LoginCaseSensitive = false,
                    // Users
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            // Login = ho.UserName, PasswordClear = ho.Password
                            Login = "admin",
                            PasswordClear = "admin"
                        }
                    }
                });
            var options = new DashboardOptions
            {
                AppPath = "/",//返回时跳转的地址
                DisplayStorageConnectionString = false,//是否显示数据库连接信息
                Authorization = new[] { filter },
                IsReadOnlyFunc = Context =>
                {
                    return false;//是否只读面板
                }
            };

            app.UseHangfireDashboard("/job", options); //可以改变Dashboard的url
            HangfireTask.HangfireSevices(); // 启动hangfire服务

            #endregion HangFire

            return app;
        }
    }
}