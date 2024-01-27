using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace PropertyAccounting
{


    internal class Estate
    {
        public int ID { get; set; }
        public string EstateName { get; set; }
        public string EstateOwner { get; set; }
        public int EstatePrice { get; set; }
    }


    class Department
    {
        public int ID { get; set; }
        public string DName { get; set; }
    }


    static class PathManager
    {
        
        public static string GetFileBaseName(string filePath, int index)
        {
            if (filePath[index] == '\\') return "";
            return filePath[index] + GetFileBaseName(filePath, --index);
        }

    }
    

    internal class EstateContextManager : DbContext
    {
        public DbSet<Estate> Estate { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Property> Property { get; set; }

        private static EstateContextManager instance;

        private EstateContextManager()
        {

        }

        public static EstateContextManager getInstance()
        {
            if (instance == null)
            {
                instance = new EstateContextManager();
            }
            return instance;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer($"Server=DESKTOP-333ROOM\\SQL_BASE;Database=em;TrustServerCertificate=true;Trusted_Connection=yes;");

    }

    

}
