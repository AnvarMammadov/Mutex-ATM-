using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WpfApp11.Entities;

namespace WpfApp11.DataAccess
{
    public class MyContext : DbContext
    {
        public MyContext()
        : base("MyAtmDb") { }

        public DbSet<Account> Accounts { get; set; }
    }
}