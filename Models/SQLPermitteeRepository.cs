using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class SQLPermitteeRepository : IPermitteeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLPermitteeRepository> logger;

        public SQLPermitteeRepository(AppDbContext context,
                                    ILogger<SQLPermitteeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public Permittee Add(Permittee permittee)
        {
            context.Permittee.Add(permittee);
            context.SaveChanges();
            return permittee;
        }

        public Permittee Delete(string id)
        {
            Permittee permittee = context.Permittee.Find(id);
            if (permittee != null)
            {
                context.Permittee.Remove(permittee);
                context.SaveChanges();
            }
            return permittee;
        }

        public IEnumerable<Permittee> GetAllPermittee(string searchText)
        {
            //return context.Permittee
            //    .Include(p => p.Department)
            //    .Include(p => p.Permit)
            //        .ThenInclude(l => l.Lot)
            //    .AsNoTracking();
            if (searchText != null)
            {
                var postdata = context.Permittee.Where(p => p.EmployeeNo.Contains(searchText))
                    .Include(p => p.Department)
                    //.Include(p => p.Permit)
                    //    .ThenInclude(p=> p.Lot)
                    .Include(p => p.Permits)
                        .ThenInclude(l => l.Lot)
                    .AsNoTracking();
                return (postdata);
            }
            else
            {
                var appDbContext = context.Permittee
                    .Include(p => p.Department)
                    //.Include(p => p.Permit)
                    //    .ThenInclude(p => p.Lot)
                    .Include(p => p.Permits)
                        .ThenInclude(l => l.Lot)
                    .AsNoTracking();
                return (appDbContext);
            }
        }

        public Permittee GetPermittee(int Id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return context.Permittee.Find(Id);
        }

        public Permittee Update(Permittee permitteeChanges)
        {
            var permittee = context.Permittee.Attach(permitteeChanges);
            permittee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return permitteeChanges;
        }
    }

}
