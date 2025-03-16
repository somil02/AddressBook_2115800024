using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class AddressBookDbContext : DbContext
    {
        public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : base(options)
        {
        }

        public DbSet<AddressBookModel> AddressBook { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}


