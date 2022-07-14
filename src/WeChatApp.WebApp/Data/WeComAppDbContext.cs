using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Interfaces;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;

namespace WeChatApp.WebApp.Data
{
    /// <summary>
    /// </summary>
    public class WeComAppDbContext : DbContext
    {
        private readonly Session _session;

        /// <summary>
        /// </summary>
        /// <param name="options"> </param>
        /// <param name="session"> </param>
        public WeComAppDbContext(DbContextOptions<WeComAppDbContext> options,
            Session session) : base(options)
        {
            _session = session;
        }

        /// <summary>
        /// </summary>
        /// <param name="modelBuilder"> </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var types = typeof(IEntity).Assembly.GetTypes().AsEnumerable()
                .Where(x => !x.IsInterface && !x.IsAbstract && x.IsAssignableTo(typeof(IEntity)));

            foreach (var type in types)
            {
                if (modelBuilder.Model.FindEntityType(type) is null)
                {
                    modelBuilder.Model.AddEntityType(type);
                }
            }

            modelBuilder.Mock();

            modelBuilder.Entity<Department>()
                //主语this，拥有Children
                .HasMany(x => x.Children)
                //主语Children，每个Child拥有一个Parent
                .WithOne(x => x.Parent)
                //主语Children，每个Child的外键是ParentId
                .HasForeignKey(x => x.ParentId)
                //这里必须是非强制关联，否则报错：Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.OnModelCreating(modelBuilder);
        }

        private void AutoSetChangedEntities()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries<IEntity>())
            {
                var baseentity = dbEntityEntry.Entity;
                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                        if (baseentity is ICreator)
                        {
                            if (_session.UserInfo is { } userInfo)
                            {
                                ((ICreator)baseentity).CreateUserUid = userInfo.Uid ?? "";
                                ((ICreator)baseentity).CreateUserId = _session.UserId;
                                ((ICreator)baseentity).CreateUserName = _session.UserName ?? "";
                            }
                            ((ICreator)baseentity).CreateTime = DateTime.Now;
                        }
                        break;

                    case EntityState.Modified:
                        if (baseentity is IModifyed)
                        {
                            if (_session.UserInfo is { } userInfo)
                            {
                                ((IModifyed)baseentity).ModifyUserUid = userInfo.Uid ?? "";
                                ((IModifyed)baseentity).ModifyUserId = _session.UserId;
                                ((IModifyed)baseentity).ModifyUserName = _session.UserName;
                            }
                            ((IModifyed)baseentity).ModifyTime = DateTime.Now;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override int SaveChanges()
        {
            try
            {
                AutoSetChangedEntities();
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"> </param>
        /// <returns> </returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AutoSetChangedEntities();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="exception"> </param>
        protected virtual void LogDbEntityValidationException(DbUpdateException exception)
        {
            //var sb = new StringBuilder();
            //sb.AppendLine("There are some validation errors while saving changes in EntityFramework:");
            //foreach (var exceptionEntityValidationError in exception.Entries)
            //{
            //    sb.AppendLine($"\t{exceptionEntityValidationError.Entity.GetType().Name}");
            //    foreach (var dbValidationError in exceptionEntityValidationError.)
            //    {
            //        sb.AppendLine("\t\t" + dbValidationError.PropertyName + ": " + dbValidationError.ErrorMessage);
            //    }
            //}
            //Logging.Logger.Error(sb.ToString());
        }
    }
}