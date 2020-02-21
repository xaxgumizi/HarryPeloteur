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

        public string ArrayToString(int[] array)
        {
            string newString = "";
            foreach (int i in array)
            {
                newString += i.ToString();
            }
            return newString;
        }
        public int[] StringToArray(string str)
        {
            int[] newArray = new int[] { };
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                newArray.Append((int)str[i]);
            }
            return newArray;
        }
        public int InsertRoom(SalleDTO salle)
        {
            string coordText = ArrayToString(salle.Coordonnees);
            string portesText = ArrayToString(salle.Portes);
            string q = "insert into salle(coordonnees,id_contenu,type_contenu,portes,etat,id_partie) values(" + coordText +","+salle.IdContenu+","+salle.TypeContenu+","+portesText+","+salle.Etat+","+salle.IdPartie+ ") SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(q, this.con);
            int newId = (int)cmd.ExecuteScalar();

            con.Close();
            return newId;
        }
        public void UpdateRoom(SalleDTO salle)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string q = "update salle set coordonneeX =" + salle.coordonneeX + " ,coordonneeY=" + salle.coordonneeY + " ,id_contenu=" + salle.id_contenu + " ,type_contenu=" + salle.type_contenu + " ,portes=" + salle.portes + " ,etat=" + salle.etat + ", id_partie=" + salle.id_partie + " where Id=" + salle.Id;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public bool UpdatePersonne(PersonneDTO p)
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

        public SalleDTO GetSalle(int id)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from salle where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            SalleDTO salle = new SalleDTO();

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

        public List<SalleDTO> GetSalles(int gameId)
        {
            return new List<SalleDTO>();
        }

        public PersonneDTO GetPersonne(int id)
        {

            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from personne where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            PersonneDTO personne = new PersonneDTO();
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

        public PartieDTO GetPartie(int id)
        {
            string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            string commande = "select * from partie where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            PartieDTO partie = new PartieDTO();
            while (reader.Read())

            {
                partie.Id = (int)reader.GetValue(0);
                partie.IdPersonnage = (int)reader.GetValue(1);
                partie.Difficulte = (int)reader.GetValue(2);

            }

            Console.WriteLine(partie.Id);
            Console.ReadLine();

            con.Close();
            return (partie);
        }

        public List<PartieDTO> GetParties()
        {
            return new List<PartieDTO>();
        }
        public string GetTexte(int type)
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

        public ObjetDTO GetObjet(int? id)
        {
            return new ObjetDTO();
        }

        public MonstreDTO GetMonstre(int? id)
        {
            return new MonstreDTO();
        }

        public GameInformationDTO GetGameInfos(int? id)
        {
            return new GameInformationDTO();
        }
    }
}
