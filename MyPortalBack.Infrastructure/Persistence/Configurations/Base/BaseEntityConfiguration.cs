using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPortalBack.Domain.Abstractions;

namespace MyPortalBack.Infrastructure.Persistence.Configurations.Base;

public abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Uuid);

        builder.Property(x => x.Uuid)
               .ValueGeneratedNever();
    }
}
