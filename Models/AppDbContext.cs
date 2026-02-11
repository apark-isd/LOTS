using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOTS3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LOTS3.Models
{
    //public class AppDbContext : DbContext
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Permittee> Permittee { get; set; }
        public DbSet<Permit> Permit { get; set; }
        public DbSet<Coordinator> Coordinator { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Lot> Lot { get; set; }
        public DbSet<PermitType> PermitType { get; set; }
        public DbSet<PermitteeType> PermitteeType { get; set; }
        public DbSet<PayType> PayType { get; set; }
        public DbSet<StatusType> StatusType { get; set; }
        //public DbSet<PermitHistory> PermitHistory { get; set; }
        public DbSet<PermitHistory> PermitHistory { get; set; }
        public DbSet<StatusType1> StatusType1 { get; set; }
        public DbSet<Location> Location { get; set; }   
        public DbSet<CheckTime> CheckTime { get; set; } 
        public DbSet<Vacancy> Vacancy { get; set; }
        public DbSet<Allocation> Allocation { get; set; }
        public DbSet<Commissioner> Commissioner { get; set; }   
        public DbSet<VehicleLiabilityWaiver> VehicleLiabilityWaiver { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        
    }
}
