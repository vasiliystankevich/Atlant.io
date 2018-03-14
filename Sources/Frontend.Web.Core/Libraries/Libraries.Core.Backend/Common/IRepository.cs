using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Core.Backend.Common
{
    public interface IRepository<TRepository>
    {
        TRepository Repository { get; set; }
    }
}
