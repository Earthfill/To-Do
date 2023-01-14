using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaceApp.Db.Models;

namespace PlaceApp.Db
{
    public class PlaceDbContext : DbContext
    {
        public PlaceDbContext(DbContextOptions<PlaceDbContext> options) : base(options) { }
        public DbSet<Place> Places { get; set; }
    }
}