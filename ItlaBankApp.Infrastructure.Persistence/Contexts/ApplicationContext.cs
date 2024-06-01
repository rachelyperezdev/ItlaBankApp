using ItlaBankApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItlaBankApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        DbSet<Product> Products { get; set; }
        DbSet<Beneficiary> Beneficiaries { get; set; }
        DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Tables
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Product>().HasKey(b => b.AccountId);
            modelBuilder.Entity<Beneficiary>().HasKey(b => b.Id);
            modelBuilder.Entity<Payment>().HasKey(p => p.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Beneficiary>()
                .HasOne(b => b.Account)
                .WithMany()
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Origin)
                .WithMany()
                .HasForeignKey(p => p.OriginId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Beneficiary)
                .WithMany()
                .HasForeignKey(p => p.BeneficiaryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Destination)
                .WithMany()
                .HasForeignKey(p => p.DestinationId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Properties
            modelBuilder.Entity<Product>()
                .Property(p => p.AccountId)
                .ValueGeneratedNever();
            modelBuilder.Entity<Product>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>()
                .Property(p => p.Debt)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            #endregion

        }
    }
}
