using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public interface IPermitteeRepository
    {
        Permittee GetPermittee(int Id);
        IEnumerable<Permittee> GetAllPermittee(string searchText);
        Permittee Add(Permittee permittee);
        Permittee Update(Permittee permitteeChanges);
        Permittee Delete(string id);
    }
}
