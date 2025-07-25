﻿using Agora.Domain.Abstractions;
using Agora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Agora.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : DbContext(dbContext), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<AdminProfile> Admins { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}
