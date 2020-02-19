using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class DBController
    {
        public string contenu;

        private string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection con;
        public DBController()
        {
            con = new SqlConnection(conString);
            this.con.Open();
        }
        public int InsertRoom(SalleDto salle)
        {
            //string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //SqlConnection con = new SqlConnection(conString);
            //this.con.Open();
            //con.Open();
            string q = "insert into salle(coordonneeX,coordonneeY,id_contenu,type_contenu,portes,etat,id_partie) values(" + salle.coordonneeX+","+salle.coordonneeY+","+salle.id_contenu+","+salle.type_contenu+","+salle.portes+","+salle.etat+","+salle.id_partie+")";
            SqlCommand cmd = new SqlCommand(q, this.con);
            cmd.ExecuteNonQuery();

            SqlCommand cmd1 = new SqlCommand("select max(Id) from salle", this.con);
            int newID = (int)cmd1.ExecuteScalar();
            Console.WriteLine(newID);
            Console.ReadLine();
            con.Close();
            return newID;
        }
        public void UpdateRoom(SalleDto salle)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string q = "update salle set coordonneeX =" + salle.coordonneeX + " ,coordonneeY=" + salle.coordonneeY + " ,id_contenu=" + salle.id_contenu + " ,type_contenu=" + salle.type_contenu + " ,portes=" + salle.portes + " ,etat=" + salle.etat + ", id_partie=" + salle.id_partie + " where Id=" + salle.Id;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public bool UpdatePersonne(PersonneDto p)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string q = "update personne set Nom =" + p.Nom +" ,Pv=" + p.Pv + " ,Force=" + p.Force + " ,Dexterite=" + p.Dexterite + " ,fuite=" + p.Fuite + " ,xp=" + p.Xp + ", po=" + p.Po + " where Id=" + p.Id;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public SalleDto getSalle(int id)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from salle where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            SalleDto salle = new SalleDto();

            while (reader.Read())

            {
                salle.Id = (int)reader.GetValue(0);
                salle.coordonneeX = (int)reader.GetValue(1);
                salle.coordonneeY = (int)reader.GetValue(2);
                salle.id_contenu = (int)reader.GetValue(3);
                salle.type_contenu = (int)reader.GetValue(4);
                salle.portes = (string)reader.GetValue(5);
                salle.etat = (int)reader.GetValue(6);
                salle.id_partie = (int)reader.GetValue(7);
            }

            Console.WriteLine(salle.Id + " " + salle.coordonneeX);
            Console.ReadLine();

            con.Close();



            return (salle);
        }

        public PersonneDto GetPersonne(int id)
        {

            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from personne where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            PersonneDto personne = new PersonneDto();
            while (reader.Read())

            {
                personne.Id = (int)reader.GetValue(0);
                personne.Nom = (string)reader.GetValue(1);
                personne.Pv = (int)reader.GetValue(2);
                personne.Force = (int)reader.GetValue(3);
                personne.Dexterite = (int)reader.GetValue(4);
                personne.Fuite = (int)reader.GetValue(5);
                personne.Xp = (int)reader.GetValue(6);
                personne.Po = (int)reader.GetValue(7);
            }

            Console.WriteLine(personne.Id);
            Console.ReadLine();

            con.Close();
            return (personne);

        }

        public PartieDto getPartie(int id)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from partie where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            PartieDto partie = new PartieDto();
            while (reader.Read())

            {
                partie.Id = (int)reader.GetValue(0);
                partie.Id_personnage = (int)reader.GetValue(1);
                partie.Salle_actuelle = (int)reader.GetValue(2);
                partie.Difficulte = (int)reader.GetValue(3);

            }

            Console.WriteLine(partie.Id);
            Console.ReadLine();

            con.Close();
            return (partie);
        }
        public string getTexte(int type)
        {
            //string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //SqlConnection con = new SqlConnection(conString);
            //con.Open();

            string commande = "select * from texte where Id=" + type;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);
            SqlDataReader reader = cmd1.ExecuteReader();
            
            while (reader.Read())
            {
                
                string contenu;
                contenu = (string)reader.GetValue(2);
                Console.WriteLine(contenu);
                Console.ReadLine();

            }


            this.con.Close();
            return contenu;
        }


    }
}
