using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain
{
    public class AppDbContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Beach> Beaches { get; set; }
        public DbSet<Amentity> Amentities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BeachAmentity> BeachAmentities { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e is { Entity: BaseEntity, State: EntityState.Modified });

            foreach (var entityEntry in entries)
                ((BaseEntity) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                AddEntityQueryFilter(builder, entityType);
            
                foreach (var mutableForeignKey in entityType.GetForeignKeys())
                    mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void AddEntityQueryFilter(ModelBuilder builder, IReadOnlyTypeBase entityType)
        {
            var type = entityType.ClrType;
            if (type.IsSubclassOf(typeof(BaseEntity)))
            {
                var parameter = Expression.Parameter(type);
                var propertyInfo = Expression.Property(parameter, "DeletedAt");
                var nullConstant = Expression.Constant(null, typeof(DateTime?));
                var equalExpression = Expression.Equal(propertyInfo, nullConstant);
                var filter = Expression.Lambda(equalExpression, parameter);
                builder.Entity(type).HasQueryFilter(filter).HasIndex("DeletedAt");
            }
        }
    }
}