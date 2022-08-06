using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Interfaces;
using WeChatApp.Shared.Options;
using WeChatApp.WebApp.Data;
using WeChatApp.Shared;

namespace WeChatApp.WebApp.Extensions
{
    /// <summary>
    /// </summary>
    public static class EfCoreExtensions
    {
        /// <summary>
        /// 添加Sql Server支持
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddSqlServer(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
            var connectionString = configuration.GetConnectionString("SqlServer");
            services.AddDbContext<WeComAppDbContext>(opts =>
            {
                opts.UseSqlServer(connectionString, x =>
                {
                    x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            return services;
        }

        /// <summary>
        /// 添加Sql Server支持
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
            var dbOptions = configuration.GetSection("DbOptions").Get<DbOptions>();

            var db = dbOptions.DbSettings.FirstOrDefault(x => x.IsEnable);

            if (db is null) throw new ArgumentNullException("服务端配置错误, 失去数据库连接.");
            services.AddDbContext<WeComAppDbContext>(opts =>
            {
                switch (db.DbType)
                {
                    default:
                    case "SqlServer":
                        opts.UseSqlServer(db.ConnectionString, x =>
                        {
                            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                        break;

                    case "MySql":
                        opts.UseMySql(db.ConnectionString, new MySqlServerVersion(new Version(5, 0)), x =>
                        {
                            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                        //opts.UseMySQL(db.ConnectionString, x =>
                        //{
                        //    x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        //});
                        break;

                    case "PostgreSql":
                        opts.UseNpgsql(db.ConnectionString, x =>
                        {
                            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                        break;
                }
            });

            return services;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"> </param>
        public static void Create(this IEntity entity)
        {
            entity.Id = Guid.NewGuid();
        }

        public static void Mock(this ModelBuilder builder)
        {
            builder.Entity<Department>()
                .HasData(
                    new Department
                    {
                        Id = Guid.Parse("02516C95-8CC3-4643-AD6E-DE08FBF5CD31"),
                        DepartmentName = "三国集团",
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31"
                    },
                    new Department
                    {
                        Id = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F4"),
                        DepartmentName = "蜀汉",
                        ParentId = Guid.Parse("02516C95-8CC3-4643-AD6E-DE08FBF5CD31"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4"
                    },
                    new Department
                    {
                        Id = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F5"),
                        DepartmentName = "桃园三小伙",
                        ParentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F4"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,EAB4C639-14A5-4458-91BA-ED77F6A955F5"
                    },
                    new Department
                    {
                        Id = Guid.Parse("170758B4-C800-4895-B373-02488FD9C6C0"),
                        DepartmentName = "蜀汉打工仔",
                        ParentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F4"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F4,170758B4-C800-4895-B373-02488FD9C6C0"
                    },
                    new Department
                    {
                        Id = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F6"),
                        DepartmentName = "魏国",
                        ParentId = Guid.Parse("02516C95-8CC3-4643-AD6E-DE08FBF5CD31"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6"
                    },
                    new Department
                    {
                        Id = Guid.Parse("00663153-CA24-4E78-AEE1-8BEAFCD045C7"),
                        DepartmentName = "魏国打工仔",
                        ParentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F6"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7"
                    },
                    new Department
                    {
                        Id = Guid.Parse("00663153-CA24-4E78-AEE1-8BEAFCD045C8"),
                        DepartmentName = "虎豹骑",
                        ParentId = Guid.Parse("00663153-CA24-4E78-AEE1-8BEAFCD045C7"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F6,00663153-CA24-4E78-AEE1-8BEAFCD045C7,00663153-CA24-4E78-AEE1-8BEAFCD045C8"
                    },
                    new Department
                    {
                        Id = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F7"),
                        DepartmentName = "吴国",
                        ParentId = Guid.Parse("02516C95-8CC3-4643-AD6E-DE08FBF5CD31"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7"
                    },
                    new Department
                    {
                        Id = Guid.Parse("B8D77DDD-A4A3-4AD0-A834-E2E7DFBAF9EF"),
                        DepartmentName = "吴国打工仔",
                        ParentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F7"),
                        TreeIds = "02516C95-8CC3-4643-AD6E-DE08FBF5CD31,EAB4C639-14A5-4458-91BA-ED77F6A955F7,B8D77DDD-A4A3-4AD0-A834-E2E7DFBAF9EF"
                    }
                );

            builder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = Guid.Parse("45D16A4C-4A1C-4CD9-A0BA-6AF0B86BBAF6"),
                        Uid = "liubei",
                        Password = "123456",
                        Name = "刘备",
                        Email = "liubei@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.高层管理员,
                        DepartmentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F5"),
                    },
                    new User
                    {
                        Id = Guid.Parse("45D16A4C-4A1C-4CD9-A0BA-6AF0B86BBAF7"),
                        Uid = "guanyu",
                        Password = "123456",
                        Name = "关羽",
                        Email = "guanyu@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.中层管理员,
                        DepartmentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F5"),
                    },
                    new User
                    {
                        Id = Guid.Parse("45D16A4C-4A1C-4CD9-A0BA-6AF0B86BBAF8"),
                        Uid = "zhangfei",
                        Password = "123456",
                        Name = "张飞",
                        Email = "zhangfei@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.中层管理员,
                        DepartmentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F5"),
                    },
                    new User
                    {
                        Id = Guid.Parse("45D16A4C-4A1C-4CD9-A0BA-6AF0B86BBAF9"),
                        Uid = "zhugeliang",
                        Password = "123456",
                        Name = "诸葛亮",
                        Email = "zhugeliang@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.普通成员,
                        DepartmentId = Guid.Parse("EAB4C639-14A5-4458-91BA-ED77F6A955F5"),
                    },
                    new User
                    {
                        Id = Guid.Parse("45D16A4C-4A1C-4CD9-A0BA-7AF0B86BBAF8"),
                        Uid = "zhaozilong",
                        Password = "123456",
                        Name = "赵子龙",
                        Email = "zhaozilong@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.中层管理员,
                        DepartmentId = Guid.Parse("170758B4-C800-4895-B373-02488FD9C6C0"),
                    },
                    new User
                    {
                        Id = Guid.Parse("8898AE8F-78C0-48F8-8E6A-F5894E9A4EC4"),
                        Uid = "zhangsan",
                        Password = "123456",
                        Name = "张三",
                        Email = "zhangsan@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.普通成员,
                        DepartmentId = Guid.Parse("170758B4-C800-4895-B373-02488FD9C6C0"),
                    },
                    new User
                    {
                        Id = Guid.Parse("8898AE8F-78C0-48F8-8E6A-F5894E9A4EC5"),
                        Uid = "lisi",
                        Password = "123456",
                        Name = "lisi",
                        Email = "zhaozilong@qq.com",
                        Tel = "13900000000",
                        Role = Shared.Enums.Role.普通成员,
                        DepartmentId = Guid.Parse("170758B4-C800-4895-B373-02488FD9C6C0"),
                    }
                );
        }
    }
}