using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPortalBack.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Uuid { get; protected set; } = Guid.NewGuid();
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    //protected BaseEntity()
    //{
    //    Id = Guid.NewGuid();
    //}
}

