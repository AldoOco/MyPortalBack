using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace MyPortalBack.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid Uuid { get; protected set; } = Guid.NewGuid();

    //protected BaseEntity()
    //{
    //    Id = Guid.NewGuid();
    //}
}

