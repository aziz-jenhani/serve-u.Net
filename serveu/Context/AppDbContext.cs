using Microsoft.EntityFrameworkCore;
using serveu.Models;

namespace serveu.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        public DbSet<restaurant> Restaurants { get; set; }
        public DbSet<MenuItemEntities> MenuItems { get; set; }
        public DbSet<MenuCategoryEntities> MenuCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la relation entre restaurant et menu_item
            modelBuilder.Entity<restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(mi => mi.Restaurant)
                .HasForeignKey(mi => mi.restaurant_id)
                .OnDelete(DeleteBehavior.Cascade);
            

            // Configuration de la relation entre menu_category et menu_item
            modelBuilder.Entity<MenuCategoryEntities>()
                .HasMany(mc => mc.MenuItems)
                .WithOne(mi => mi.Category)
                .HasForeignKey(mi => mi.category_id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); // Spécifie que la clé étrangère CategoryId est requise

            // Ajoutez ici d'autres configurations pour vos entités si nécessaire
            modelBuilder.Entity<MenuItemEntities>()
         .HasOne(mi => mi.Category)
         .WithMany(c => c.MenuItems)
         .HasForeignKey(mi => mi.category_id) // Utilisez la clé étrangère CategoryId
         .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemEntities>()
                .HasOne(mi => mi.Restaurant)
                .WithMany(r => r.MenuItems)
                .HasForeignKey(mi => mi.restaurant_id) // Utilisez la clé étrangère RestaurantId
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<serveu.Models.FileEntities> FileEntities { get; set; } = default!;

    }
}
