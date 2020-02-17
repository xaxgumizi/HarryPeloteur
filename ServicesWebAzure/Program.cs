using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesWebAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            RetrieveData r = new RetrieveData();
            r.getTexte(1);
            
        }
    }
}
