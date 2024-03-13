﻿using Microsoft.EntityFrameworkCore;
using PartyTime.Models;

namespace PartyTime.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }

}
