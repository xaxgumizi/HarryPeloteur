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

        public string ArrayToString(int[] arr)
        {
            return string.Join(" ", arr.Select(n => Convert.ToString(n)).ToArray());
        }
        public int[] StringToArray(string str)
        {
            return str.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
        }
        public int InsertRoom(SalleDTO salle)
        {
            string coordText = ArrayToString(salle.Coordonnees);
            string portesText = ArrayToString(salle.Portes);

            string q = "insert into salle(coordonnees,id_contenu,type_contenu,portes,etat,id_partie) values('" + coordText + "'," + salle.IdContenu + "," + salle.TypeContenu + ",'" + portesText + "'," + salle.Etat + "," + salle.IdPartie + ") SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(q, this.con);

            var newId = Convert.ToInt32(cmd.ExecuteScalar());

            this.con.Close();

            return newId;
        }
        public void UpdateRoom(SalleDTO salle)
        {
            string coordText = ArrayToString(salle.Coordonnees);
            string portesText = ArrayToString(salle.Portes);

            string q = "update salle set coordonnees ='" + coordText + "' ,id_contenu=" + salle.IdContenu + " ,type_contenu=" + salle.TypeContenu + " ,portes='" + portesText + "' ,etat=" + salle.Etat + ", id_partie=" + salle.IdPartie + " where Id=" + salle.Id;
            SqlCommand cmd = new SqlCommand(q, this.con);
            
            cmd.ExecuteNonQuery();

            this.con.Close();

        }

        public bool UpdatePersonne(PersonneDTO p)
        {
            string q = "update personne set Nom =" + p.Nom +" ,Pv=" + p.Pv + " ,Force=" + p.Force + " ,Dexterite=" + p.Dexterite + " ,fuite=" + p.Fuite + " ,xp=" + p.Xp + ", po=" + p.Po + " where Id=" + p.Id;
            SqlCommand cmd = new SqlCommand(q, this.con);
            
            cmd.ExecuteNonQuery();
            
            this.con.Close();

            return true;
        }

        public SalleDTO GetSalle(int id)
        {
            string commande = "select * from salle where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            SalleDTO salle = new SalleDTO();

            while (reader.Read())
            {
                salle.Id = (int)reader.GetValue(0);

                string coordText = (string)reader.GetValue(1);
                salle.Coordonnees = StringToArray(coordText);

                salle.IdContenu = (int)reader.GetValue(2);

                salle.TypeContenu = (int)reader.GetValue(3);

                string portesText = (string)reader.GetValue(4); // par exemple si j'ai 1000 tabDoors[0]=0 , tabDoors[1]=0 ,tabDoors[2]=0 ,tabDoors[3]=1 , 
                salle.Portes = StringToArray(portesText);

                salle.Etat = (int)reader.GetValue(5);

                salle.IdPartie = (int)reader.GetValue(6);
            }

            this.con.Close();

            return (salle);
        }

        public List<SalleDTO> GetSalles(int gameId)
        {
            return new List<SalleDTO>();
        }

        public PersonneDTO GetPersonne(int id)
        {
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

            this.con.Close();

            return personne;
        }

        public PartieDTO GetPartie(int id)
        {
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

            this.con.Close();
            return (partie);
        }

        public List<PartieDTO> GetParties()
        {
            return new List<PartieDTO>();
        }
        public string GetTexte(int type)
        {
            string commande = "select * from texte where Id=" + type;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();
            
            while(reader.Read())
            {
                string contenu = (string)reader.GetValue(2);
            }

            this.con.Close();
            return contenu;
        }

        public ObjetDTO GetObjet(int id)
        {
            string commande = "select * from objet where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            ObjetDTO objet = new ObjetDTO();
            while(reader.Read())
            {
                objet.Id = (int)reader.GetValue(0);
                objet.Nom = (string)reader.GetValue(1);
                objet.Description = (string)reader.GetValue(2);
                objet.ProprieteCible = (string)reader.GetValue(3);
                objet.Montant = (int)reader.GetValue(4);
            }

            return objet;
        }

        public MonstreDTO GetMonstre(int id)
        {
            return new MonstreDTO();
        }

        public GameInformationDTO GetGameInfos(int id)
        {
            return new GameInformationDTO();
        }
    }
}
