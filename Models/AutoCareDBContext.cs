using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Models.ViewModels;

namespace AutoCare.Models
{
    public class AutoCareDBContext : IdentityDbContext<ApplicationUser>
    {
        public AutoCareDBContext(DbContextOptions<AutoCareDBContext> options) : base(options)
        {

        }

        public DbSet<Cars> Cars { get; set; }
        public DbSet<checkUp> checkUps { get; set; }
        public DbSet<services> services { get; set; }
        public DbSet<ServicesCheckups> servicesCheckups { get; set; }

        public DbSet<spareCheckup> spareCheckups { get; set; }

        public DbSet<SpareParts> spareParts { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<CarCreateViewModel>(builder =>{
                builder.HasNoKey();
                builder.ToTable("MY_ENTITY");
                });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CarEditViewModel>(builder => {
                builder.HasNoKey();
                
            });
            modelBuilder.Entity<CreateRoleViewModel>(builder => {
                builder.HasNoKey();

            });
            
                 modelBuilder.Entity<EditRoleViewModel>(builder => {
                     builder.HasNoKey();

                 });
            modelBuilder.Entity<UserRoleViewModel>(builder => {
                builder.HasNoKey();

            });
         

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public DbSet<AutoCare.Models.ViewModels.CarCreateViewModel> CarCreateViewModel { get; set; }
        public DbSet<AutoCare.Models.ViewModels.CarEditViewModel> CarEditViewModel { get; set; }
        public DbSet<AutoCare.Models.ViewModels.EditUserViewModel> EditUserViewModel { get; set; }
        public DbSet<AutoCare.Models.ViewModels.CreateRoleViewModel> CreateRoleViewModel { get; set; }
        public DbSet<AutoCare.Models.ViewModels.EditRoleViewModel> EditRoleViewModel { get; set; }
        public DbSet<AutoCare.Models.ViewModels.UserRoleViewModel> UserRoleViewModel { get; set; }

    }

}
