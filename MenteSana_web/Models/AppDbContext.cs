using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MenteSana_web.Models
{
    public class MenteSanaDbContext : DbContext
    {
        public MenteSanaDbContext(DbContextOptions<MenteSanaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Persona{ get; set; }
    }
}
