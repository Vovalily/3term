using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DataContext() : base("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\time\\Desktop\\ \\не мусор\\c#\\3 семестр\\1 лаба\\Lab1\\DataAccessLayer\\Players.mdf\";Integrated Security=True") 
        { }

    }
}
