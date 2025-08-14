using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Base
{
    public abstract class BaseWritteVM<TId>
    {
        public TId? Id { get; set; }
        public bool IsCreating { get; set; }
    }
}
