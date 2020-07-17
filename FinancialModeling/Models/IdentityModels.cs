using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using FinancialModeling.Models.DBModels;
using System;

namespace FinancialModeling.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ClientId { get; set; }
        public bool IsActive { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class FinancialModelingDbContext : IdentityDbContext<ApplicationUser>
    {
        static string authConnection = ConfigurationManager.ConnectionStrings["AuthenticationConnection"].ConnectionString;
        public FinancialModelingDbContext()
            : base(authConnection, throwIfV1Schema: false)
        {
        }

        public DbSet<ParkingClient> ParkingClients { get; set; }

        public DbSet<ClientModel> ClientModels { get; set; }

        public DbSet<OperatingDays> OperatingDays { get; set; }

        public DbSet<Zone> Zones { get; set; }

        public DbSet<HourlyZone> HourlyZones { get; set; }
        public DbSet<TimeOfDayZone> TimeOfDayZones { get; set; }
        public DbSet<EscalatingZone> EscalatingZones { get; set; }

        public DbSet<HourlyOperatingHour> HourlyOperatingHours { get; set; }
        public DbSet<TimeOfDayOperatingHour> TimeOfDayOperatingHours { get; set; }
        public DbSet<EscalatingOperatingHour> EscalatingOperatingHours { get; set; }

        //public DbSet<ClientPermitType> ClientPermitTypes { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<PermitDetail> PermitDetails { get; set; }

        public DbSet<EquipmentCost> EquipmentCosts { get; set; }

        public DbSet<LuEquipmentType> LuEquipmentTypes { get; set; }
        public DbSet<LuModelType> LuModelTypes { get; set; }
        public DbSet<LuParkingType> LuParkingTypes { get; set; }

        public DbSet<ApiError> ApiErrors { get; set; }

        public DbSet<Projection> Projections { get; set; }

        public static FinancialModelingDbContext Create()
        {
            return new FinancialModelingDbContext();
        }
    }
}