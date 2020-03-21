using GTL_DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTL_DataAccessLibrary.DataAccess
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions options) : base(options) { }
        public DbSet<Member> Members { get; set; }
        public DbSet <Address> Addresses { get; set; }
        public DbSet <ZipCode> Zips { get; set; }


    }
}
