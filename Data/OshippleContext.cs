using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oshipple.Models;

namespace Oshipple.Data
{
    public class OshippleContext : DbContext
    {
        public OshippleContext (DbContextOptions<OshippleContext> options)
            : base(options)
        {
        }

        public DbSet<Oshipple.Models.AllSongs> AllSongs { get; set; } = default!;
    }
}
