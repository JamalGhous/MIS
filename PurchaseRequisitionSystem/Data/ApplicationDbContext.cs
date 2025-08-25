using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PurchaseRequisitionSystem.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FacilityMaster> FacilityMasters { get; set; }
        public DbSet<HierarchyMaster> HierarchyMasters { get; set; }
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<DelegationHistory> DelegationHistories { get; set; }
        public DbSet<PRAttachment> PRAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<PurchaseRequisition>()
                .HasOne(pr => pr.Initiator)
                .WithMany(u => u.InitiatedRequests)
                .HasForeignKey(pr => pr.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseRequisition>()
                .HasOne(pr => pr.CurrentApprover)
                .WithMany()
                .HasForeignKey(pr => pr.CurrentApproverId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.Approver)
                .WithMany(u => u.ApprovalHistory)
                .HasForeignKey(ah => ah.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DelegationHistory>()
                .HasOne(dh => dh.FromUser)
                .WithMany(u => u.DelegationsFrom)
                .HasForeignKey(dh => dh.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DelegationHistory>()
                .HasOne(dh => dh.ToUser)
                .WithMany(u => u.DelegationsTo)
                .HasForeignKey(dh => dh.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seed Facilities
            builder.Entity<FacilityMaster>().HasData(
                new FacilityMaster { Id = 1, Code = FacilityCode.DXB, Name = "Dubai", Description = "Dubai Facility" },
                new FacilityMaster { Id = 2, Code = FacilityCode.SHJ, Name = "Sharjah", Description = "Sharjah Facility" },
                new FacilityMaster { Id = 3, Code = FacilityCode.AJM, Name = "Ajman", Description = "Ajman Facility" },
                new FacilityMaster { Id = 4, Code = FacilityCode.AKA, Name = "Akoya Clinic", Description = "Akoya Clinic Facility" }
            );
        }
    }
}