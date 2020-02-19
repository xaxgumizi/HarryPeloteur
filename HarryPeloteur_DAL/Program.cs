using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            DBController r = new DBController();
            r.getTexte(1);
            
        }
    }
}
