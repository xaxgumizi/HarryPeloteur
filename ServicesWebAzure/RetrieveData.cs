using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesWebAzure
{
    public class RetrieveData
    {
        public string contenu;
        string conString;
        SqlConnection con;

        public RetrieveData()
        {
            conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            con = new SqlConnection(conString);
        }
        public int InsertRoom(SalleDto salle)
        {
            con.Open();
            string q = "insert into salle(coordonnees,id_contenu,type_contenu,portes,etat,id_partie) values(" + salle.coordonnees+","+salle.id_contenu+","+salle.type_contenu+","+salle.portes+","+salle.etat+","+salle.id_partie+")";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();

            SqlCommand cmd1 = new SqlCommand("select max(Id) from salle", con);
            int newID = (int)cmd1.ExecuteScalar();
            Console.WriteLine(newID);
            Console.ReadLine();
            con.Close();
            return newID;
        }
        public void UpdateRoom(SalleDto salle)
        {
            con.Open();
            string q = "update salle set coordonnees =" + salle.coordonnees + " ,id_contenu=" + salle.id_contenu + " ,type_contenu=" + salle.type_contenu + " ,portes=" + salle.portes + " ,etat=" + salle.etat + ", id_partie=" + salle.id_partie + " where Id=" + salle.Id;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public bool UpdatePersonne(PersonneDto p)
        {
            con.Open();
            string q = "update personne set Nom =" + p.Nom +" ,Pv=" + p.Pv + " ,Force=" + p.Force + " ,Dexterite=" + p.Dexterite + " ,fuite=" + p.Fuite + " ,xp=" + p.Xp + ", po=" + p.Po+", salle_actuelle="+p.salle_actuelle + " where Id=" + p.Id;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }

        public SalleDto getSalle(int id)
        {
            con.Open();
            string commande = "select * from salle where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            SalleDto salle = new SalleDto();

            while (reader.Read())

            {
                salle.Id = (int)reader.GetValue(0);

                string sallees = (string)reader.GetValue(1);
                int salles = Int32.Parse(sallees);
                int[] tabSalles = new int[2];
                tabSalles[0] = salles % 10;
                salles = salles / 10;
                tabSalles[1] = salles % 10;
                salle.coordonnees = tabSalles;

                salle.id_contenu = (int)reader.GetValue(2);

                string contenu = reader.GetValue(3).ToString();
                salle.type_contenu = Int32.Parse(contenu);

                string doorrs= reader.GetValue(4).ToString(); // par exemple si j'ai 1000 tabDoors[0]=0 , tabDoors[1]=0 ,tabDoors[2]=0 ,tabDoors[3]=1 , 
                int doors = Int32.Parse(doorrs);
                int[] tabDoors = new int[4];
                tabDoors[0] = doors % 10;
                doors = doors / 10;
                tabDoors[1] = doors % 10;
                doors = doors / 10;
                tabDoors[2] = doors % 10;
                doors = doors / 10;
                tabDoors[3] = doors % 10;
                salle.portes = tabDoors;

                salle.etat = (int)reader.GetValue(5);

                salle.id_partie = (int)reader.GetValue(6);
            }

            Console.WriteLine(salle.coordonnees[1] + " " + salle.coordonnees[0]);

            Console.ReadLine();

            con.Close();



            return (salle);
        }

        public PersonneDto GetPersonne(int id)
        {
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
                personne.salle_actuelle = (int)reader.GetValue(8);
            }

            Console.WriteLine(personne.Id);
            Console.ReadLine();

            con.Close();
            return (personne);

        }

        public PartieDto getPartie(int id)
        {
            con.Open();
            string commande = "select * from partie where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            PartieDto partie = new PartieDto();
            while (reader.Read())

            {
                partie.Id = (int)reader.GetValue(0);
                partie.Id_personnage = (int)reader.GetValue(1);
                partie.Difficulte = (int)reader.GetValue(3);

            }

            Console.WriteLine(partie.Id);
            Console.ReadLine();

            con.Close();
            return (partie);
        }
        public string getTexte(int type)
        {
            con.Open();
            string commande = "select * from texte where Id=" + type;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            
            while (reader.Read())

            {
                
                string contenu;
                contenu = (string)reader.GetValue(2);
                Console.WriteLine(contenu);
                Console.ReadLine();

            }


            con.Close();
            return contenu;
        }

        public void deletePersonne(int identifiant)
        {
            con.Open();
            string q = "delete from Personne where id=" + identifiant;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void deletePartie(int identifiant)
        {
            con.Open();
            string q = "delete from Partie where id=" + identifiant;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void deleteSalle(int identifiantPartie)
        {
            con.Open();
            string q = "delete from Partie where id_partie=" + identifiantPartie;
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public MonstreDto getMonstre(int id)
        {
            con.Open();
            string commande = "select * from Monstre where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, con);
            SqlDataReader reader = cmd1.ExecuteReader();
            MonstreDto monstre = new MonstreDto();
            while (reader.Read())

            {
                monstre.Id = (int)reader.GetValue(0);
                monstre.Nom = (string)reader.GetValue(1);
                monstre.Pv = (int)reader.GetValue(2);
                monstre.Force = (int)reader.GetValue(3);
                monstre.Dexterite = (int)reader.GetValue(4);
                monstre.Drop_xp = (int)reader.GetValue(5);
                monstre.Drop_argent = (int)reader.GetValue(6);
                monstre.Proba_drop_argent = (int)reader.GetValue(7);
            }

            Console.WriteLine(monstre.Id);
            Console.ReadLine();

            con.Close();
            return (monstre);

        }


    }
}
