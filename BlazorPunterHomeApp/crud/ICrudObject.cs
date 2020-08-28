using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.crud
{
    public interface ICrudObject
    {
        Guid Id { get; set; }
    }
}
