using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportAI.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Domain { get; set; }
    public DateTime CreatedAt { get; set; }
}

