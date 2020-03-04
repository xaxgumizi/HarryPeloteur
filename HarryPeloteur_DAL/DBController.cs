using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class DBController
    {
        static void Main(string[] args)
        {

        }
        public string contenu;

        private string conString = "Data Source=isimadba.database.windows.net;Initial Catalog=IsimaDatabase;User ID=isimadba;Password=tvilum?00;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection con;
        Random rnd = new Random();
        public DBController()
        {
            con = new SqlConnection(conString);
            //this.con.Open();
        }
        public void VarDump(object obj)
        {
            System.Diagnostics.Debug.WriteLine("{0,-18} {1}", "Name", "Value");
            string ln = @"-----------------------------------------------------------------";
            System.Diagnostics.Debug.WriteLine(ln);

            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("{0,-18} {1}",
                          props[i].Name, props[i].GetValue(obj, null));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);  
                }
            }
            System.Diagnostics.Debug.WriteLine("");
        }
        public string ArrayToString(int[] arr)
        {
            return string.Join(" ", arr.Select(n => Convert.ToString(n)).ToArray());
        }
        public int[] StringToArray(string str)
        {
            return str.Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
        }
        public int InsertSalle(SalleDTO room)
        {
            this.con.Open();
            string coordText = ArrayToString(room.Coordonnees);
            string portesText = ArrayToString(room.Portes);

            string q = "insert into salle(coordonnees,id_contenu,type_contenu,portes,etat,id_partie) values('" + coordText + "'," + room.IdContenu + "," + room.TypeContenu + ",'" + portesText + "'," + room.Etat + "," + room.IdPartie + ") SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(q, this.con);

            var newId = Convert.ToInt32(cmd.ExecuteScalar());

            this.con.Close();


            return newId;
        }
        public void UpdateSalle(SalleDTO room)
        {
            this.con.Open();
            string coordText = ArrayToString(room.Coordonnees);
            string portesText = ArrayToString(room.Portes);

            string q = "update salle set coordonnees ='" + coordText + "' ,id_contenu=" + room.IdContenu + " ,type_contenu=" + room.TypeContenu + " ,portes='" + portesText + "' ,etat=" + room.Etat + ", id_partie=" + room.IdPartie + " where Id=" + room.Id;
            SqlCommand cmd = new SqlCommand(q, this.con);
            
            cmd.ExecuteNonQuery();

            this.con.Close();

        }

        public bool UpdatePersonne(PersonneDTO p)
        {
            this.con.Open();
            string q = "update personne set Nom ='" + p.Nom +"' ,Pv=" + p.Pv + " ,Force=" + p.Force + " ,Dexterite=" + p.Dexterite + " ,fuite=" + p.Fuite + " ,xp=" + p.Xp + ", po=" + p.Po + ", salle_actuelle="+ p.SalleActuelle + " where Id=" + p.Id;
            SqlCommand cmd = new SqlCommand(q, this.con);
            cmd.ExecuteNonQuery();
            
            this.con.Close();

            return true;
        }

        public SalleDTO GetSalle(int? id)
        {
            this.con.Open();
            string commande = "select * from salle where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            SalleDTO room = new SalleDTO();

            while (reader.Read())
            {
                room.Id = (int?)reader.GetValue(0);

                string coordText = (string)reader.GetValue(1);
                room.Coordonnees = StringToArray(coordText);

                room.IdContenu = (int?)reader.GetValue(2);

                room.TypeContenu = (int?)reader.GetValue(3);

                string portesText = (string)reader.GetValue(4);
                room.Portes = StringToArray(portesText);

                room.Etat = (int?)reader.GetValue(5);

                room.IdPartie = (int?)reader.GetValue(6);
            }

            this.con.Close();

            return room;
        }

        public List<SalleDTO> GetSalles(int? gameId)
        {
            this.con.Open();
            string commande = "select * from salle where id_partie=" + gameId;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            List<SalleDTO> roomList = new List<SalleDTO>();

            while (reader.Read())
            {
                SalleDTO room = new SalleDTO();

                room.Id = (int?)reader.GetValue(0);

                string coordText = (string)reader.GetValue(1);
                room.Coordonnees = StringToArray(coordText);

                room.IdContenu = (int?)reader.GetValue(2);

                room.TypeContenu = (int?)reader.GetValue(3);

                string portesText = (string)reader.GetValue(4);
                room.Portes = StringToArray(portesText);

                room.Etat = (int?)reader.GetValue(5);

                room.IdPartie = (int?)reader.GetValue(6);

                roomList.Add(room);
            }

            this.con.Close();
            return roomList;
        }

        public PersonneDTO GetPersonne(int? id)
        {
            this.con.Open();
            string commande = "select * from personne where Id=" + id;
            System.Diagnostics.Debug.WriteLine("Execution de la requête: " + commande);
            SqlCommand cmd1 = new SqlCommand(commande, this.con);
            SqlDataReader reader = cmd1.ExecuteReader();

            PersonneDTO personne = new PersonneDTO();
            while (reader.Read())
            {
                personne.Id = (int?)reader.GetValue(0);
                personne.Nom = (string)reader.GetValue(1);
                personne.Pv = (int?)reader.GetValue(2);
                personne.Force = (int?)reader.GetValue(3);
                personne.Dexterite = (int?)reader.GetValue(4);
                personne.Fuite = (int?)reader.GetValue(5);
                personne.Xp = (int?)reader.GetValue(6);
                personne.Po = (int?)reader.GetValue(7);
                personne.SalleActuelle = (int?)reader.GetValue(8);
            }

            this.con.Close();

            return personne;
        }

        public PartieDTO GetPartie(int? id)
        {
            this.con.Open();
            string commande = "select * from partie where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            PartieDTO game = new PartieDTO();
            while (reader.Read())
            {
                game.Id = (int?)reader.GetValue(0);
                game.IdPersonnage = (int?)reader.GetValue(1);
                game.Difficulte = (int?)reader.GetValue(2);
            }

            this.con.Close();
            return game;
        }

        public List<PartieDTO> GetParties()
        {
            this.con.Open();
            string commande = "select * from partie";
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            List<PartieDTO> gameList = new List<PartieDTO>();

            while (reader.Read())
            {
                PartieDTO game = new PartieDTO();
                game.Id = (int?)reader.GetValue(0);
                game.IdPersonnage = (int?)reader.GetValue(1);
                game.Difficulte = (int?)reader.GetValue(2);

                gameList.Add(game);
            }

            this.con.Close();
            return gameList;
        }
        public string GetTexte(int? type)
        {
            this.con.Open();
            string commande = "select * from texte where type=" + type;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            List<string> texts = new List<string> { };
            while (reader.Read())
            {
                texts.Add((string)reader.GetValue(2));
            }

            int r = rnd.Next(texts.Count);
            string text = texts[r];

            this.con.Close();
            return text;
        }

        public ObjetDTO GetObjet(int? id)
        {
            this.con.Open();
            string commande = "select * from objet where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            ObjetDTO objet = new ObjetDTO();
            while(reader.Read())
            {
                objet.Id = (int?)reader.GetValue(0);
                objet.Nom = (string)reader.GetValue(1);
                objet.Description = (string)reader.GetValue(2);
                objet.ProprieteCible = (string)reader.GetValue(3);
                objet.Montant = (int?)reader.GetValue(4);
            }

            this.con.Close();
            return objet;
        }

        public MonstreDTO GetMonstre(int? id)
        {
            this.con.Open();
            string commande = "select * from monstre where Id=" + id;
            SqlCommand cmd1 = new SqlCommand(commande, this.con);

            SqlDataReader reader = cmd1.ExecuteReader();

            MonstreDTO monster = new MonstreDTO();
            while (reader.Read())
            {
                monster.Id = (int?)reader.GetValue(0);
                monster.Nom = (string)reader.GetValue(1);
                monster.Pv = (int?)reader.GetValue(2);
                monster.Force = (int?)reader.GetValue(3);
                monster.Dexterite = (int?)reader.GetValue(4);
                monster.DropXp = (int?)reader.GetValue(5);
                monster.ProbaDropArgent = (float?)Convert.ToDouble(reader.GetValue(6));
            }

            this.con.Close();
            return monster;
        }

        public GameInformationDTO GetGameInfos(int? id)
        {
            GameInformationDTO gameInfos = new GameInformationDTO();
            gameInfos.Game = this.GetPartie(id);
            gameInfos.Character = this.GetPersonne(gameInfos.Game.IdPersonnage);
            gameInfos.Rooms = this.GetSalles(gameInfos.Game.Id);
            return gameInfos;
        }

        public int InsertPersonne(PersonneDTO perso)
        {
            this.con.Open();
            string q = "insert into personne(nom,pv,force,dexterite,fuite,xp,po,salle_actuelle) values('" + perso.Nom + "'," + perso.Pv + "," + perso.Force + "," + perso.Dexterite + "," + perso.Fuite + "," + perso.Xp + "," + perso.Po + "," + perso.SalleActuelle + ") SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(q, this.con);

            var newId = Convert.ToInt32(cmd.ExecuteScalar());

            this.con.Close();

            return newId;
        }

        public int InsertPartie(PartieDTO partie)
        {
            this.con.Open();
            string q = "insert into partie(id_personnage,difficulte) values(" + partie.IdPersonnage + "," + partie.Difficulte + ") SELECT SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand(q, this.con);

            var newId = Convert.ToInt32(cmd.ExecuteScalar());

            this.con.Close();

            return newId;
        }

    }
}
