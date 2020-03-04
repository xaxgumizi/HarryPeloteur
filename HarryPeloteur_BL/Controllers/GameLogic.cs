using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HarryPeloteur_DAL;


namespace HarryPeloteur_BL.Controllers
{
    public class GameLogic
    {
        Random rnd = new Random();
        //dbController db = new dbController();
        DebugTools dt = new DebugTools();
        HarryPeloteur_DAL.DBController db = new DBController();

        // Ordre des instructions
        // Récupérer les infos depuis BDD
        // Appeler fonction qui fait traitement en passant les infos
        // La fonction modifie les infos puis les update dans la BDD
        // Obligé car on ne connait pas le nouvel ID dans la BDD avant l'insertion
        // On redemande les infos depuis la BDD

        public string HandleAvancer(HarryPeloteur_DAL.GameInformationDTO gameInfos, string[] parameters)
        {
            string resultText = "";
            dt.dbg("Call to avancer");
            if (parameters.Count() < 2)
            {
                return "Pas assez d'arguments";
            }
            int direction = -1;
            switch (parameters[1])
            {
                case "haut":
                    direction = 0;
                    break;
                case "droite":
                    direction = 1;
                    break;
                case "bas":
                    direction = 2;
                    break;
                case "gauche":
                    direction = 3;
                    break;
                case "random":
                    direction = 4;
                    break;
                default:
                    return "Erreur de paramètre";
            }

            HarryPeloteur_DAL.SalleDTO currentRoom = FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle).found;
            // Si on est face à un monstre non combattu on ne peut pas simplement partir
            if (currentRoom.TypeContenu == 2 && currentRoom.Etat == 0)
            {
                dt.dbg("Mode combat déplacement impossible");
                return "Mode combat déplacement impossible";
            }

            // Si il faut tirer une direction au hasard, pour une Fuite par exemple
            if (direction == 4)
            {
                dt.dbg("Salle aléatoire");
                //var candidates = currentRoom.Portes.Where(x => x == 1).ToArray();
                // On cherche toutes les Portes de la salle
                List<int> candidates = new List<int>();
                foreach (var item in currentRoom.Portes.Select((value, i) => (value, i)))
                {
                    if (item.value == 1)
                    {
                        candidates.Add(item.i);
                    }
                }
                // On en sélectionne une au hasard
                var selected = rnd.Next(0, candidates.Count);

                // On assigne la nouvelle direction par rapport à la porte choisie
                direction = candidates[selected];
                dt.dbg("On va vers " + direction.ToString());
            }

            // Si il y a une porte là ou on veut aller
            if (currentRoom.Portes[direction] == 1)
            {
                int[] currentcoordinates = { 0, 0 };
                currentcoordinates[0] = currentRoom.Coordonnees[0];
                currentcoordinates[1] = currentRoom.Coordonnees[1];


                switch (direction)
                {
                    case 0: // On bouge vers le haut
                        currentcoordinates[1] += 1;
                        break;
                    case 1: //droite
                        currentcoordinates[0] += 1;
                        break;
                    case 2: // bas
                        currentcoordinates[1] -= 1;
                        break;
                    case 3: //gauche
                        currentcoordinates[0] -= 1;
                        break;
                }

                HarryPeloteur_DAL.SalleDTO existingroom = FindRoomByCoordinates(gameInfos.Rooms, currentcoordinates); // Cherche une salle au nouvel emplacement
                if (existingroom != null) // Si il existe déjà une salle à cet emplacement
                {
                    dt.dbg("La salle existe pour le déplacement");
                    resultText = "Vous connaissez cette salle";
                    gameInfos.Character.SalleActuelle = existingroom.Id; // Alors on déplace juste le personnage dedans
                }
                else // Sinon on doit générer une nouvelle salle
                {
                    dt.dbg("On créé une salle pour le déplacement");
                    resultText = "Vous découvrez une nouvelle salle";
                    // La porte opposée de celle d'où l'on vient
                    int[] oppposingDirections = { 2, 3, 0, 1 };

                    HarryPeloteur_DAL.SalleDTO newRoom = GenerateNewRoom();

                    newRoom.IdPartie = gameInfos.Game.Id; // On assigne l'Id de la partie et ses nouvelles coordonnées
                    newRoom.Coordonnees = currentcoordinates;
                    // On place la porte pour retourner de là où on vient
                    newRoom.Portes[oppposingDirections[direction]] = 1;

                    // On insère dans la BDD la nouvelle salle
                    db.InsertSalle(newRoom);

                    // On récupère de nouveau les salles pour avoir l'Id de la salle que l'on vient d'insérer
                    gameInfos.Rooms = db.GetSalles(gameInfos.Game.Id);
                    // On considère que les résultats sont triés par Id croissant, donc on prend la dernière salle
                    int? newRoomId = gameInfos.Rooms.Last().Id;

                    // On déplace le personnage dans la nouvelle salle
                    gameInfos.Character.SalleActuelle = newRoomId;
                }
                resultText = "";
                db.UpdatePersonne(gameInfos.Character); // On met à jour dans la BDD le personnage
                return resultText;
            }
            else // Sinon on ne peut pas aller là
            {
                dt.dbg("Pas de porte dans la direction choisie");
                return "Pas de porte dans la direction choisie";
            }
        }

        public dynamic FindRoomById(List<HarryPeloteur_DAL.SalleDTO> Rooms, int? Id)
        {
            dt.dbg("Looking for room id: " + Id.ToString());
            HarryPeloteur_DAL.SalleDTO found = null;
            int index = 0;
            foreach (var room in Rooms.Select((value, i) => (value, i)))
            {
                if (room.value.Id == Id)
                {
                    found = room.value;
                    index = room.i;
                    break;
                }
            }
            return new { found, index };
        }

        public HarryPeloteur_DAL.SalleDTO FindRoomByCoordinates(List<HarryPeloteur_DAL.SalleDTO> Rooms, int[] coordinates)
        {
            dt.dbg("Call to find room by coordinates");
            HarryPeloteur_DAL.SalleDTO found = null;

            foreach (HarryPeloteur_DAL.SalleDTO room in Rooms)
            {
                if (room.Coordonnees[0] == coordinates[0] && room.Coordonnees[1] == coordinates[1])
                {
                    found = room;
                    break;
                }
            }

            return found;
        }

        /* Génère une nouvelle salle avec des attributs aléatoires
         */
        public HarryPeloteur_DAL.SalleDTO GenerateNewRoom()
        {
            var randomContentTypeGenerator = new LoadedDie(new int[] { 50, 25, 25 }); // 50% rien, 25% objet, 25% monstre
            var contentType = randomContentTypeGenerator.Next();

            var contentId = 0;
            switch (contentType)
            {
                case 0: // La salle est vide
                    contentId = 0;
                    break;
                case 1: // Il y a un objet
                    var randomObject = new LoadedDie(new int[] { 20, 10, 10, 10, 10, 5, 20, 10, 5 }); // regagne 25% de la vie, regagne 50% de la vie, augmente 10% de Force, augmente de 10% la Fuite, augmente de 10% la dexterité, augmente de 10% les PV, 10PO, 50PO, 100PO
                    contentId = randomObject.Next();
                    break;
                case 2: // Il y a un monstre
                    var randomMonster = new LoadedDie(new int[] { 10, 10, 10, 10, 20, 20, 9, 9, 2 }); // voir table des monstres
                    contentId = randomMonster.Next();
                    break;
            }

            var room = new HarryPeloteur_DAL.SalleDTO()
            {
                Id = 0,
                IdPartie = 0,
                Coordonnees = new int[] { 0, 0 },
                IdContenu = contentId,
                TypeContenu = contentType,
                Portes = new int[] { // Chaque porte a 50% de chance d'être présente
                    rnd.Next(0,2),
                    rnd.Next(0,2),
                    rnd.Next(0,2),
                    rnd.Next(0,2) },
                Etat = 0
            };

            return room;
        }

        public string HandleCombattre(HarryPeloteur_DAL.GameInformationDTO gameInfos, string[] paramaters)
        {
            HarryPeloteur_DAL.SalleDTO currentRoom = FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle).found;
            string resultText = "";
            if (currentRoom.TypeContenu != 2)
            {
                dt.dbg("Impossible de combattre dans une salle sans monstres");
                return "Impossible de combattre dans une salle sans monstres";
            }

            HarryPeloteur_DAL.MonstreDTO currentMonster = db.GetMonstre(currentRoom.IdContenu);

            // On détermine les chances de toucher en fonction de la dextérité
            double playerHitChance = Math.Min(0.9, 0.5 + 0.5 * (((double)gameInfos.Character.Dexterite - (double)currentMonster.Dexterite) / (double)currentMonster.Dexterite));
            double monsterHitChance = Math.Min(0.9, 0.5 + 0.5 * (((double)currentMonster.Dexterite - (double)gameInfos.Character.Dexterite) / (double)gameInfos.Character.Dexterite));
            // ajouter une fonction pour faire ça
            // diviser par deux l'augmentation de la chance ?
            dt.dbg(gameInfos.Character.Dexterite.ToString());
            dt.dbg(currentMonster.Dexterite.ToString());
            dt.dbg((gameInfos.Character.Dexterite / currentMonster.Dexterite).ToString());
            dt.dbg("Chances de toucher du joueur " + playerHitChance.ToString());
            dt.dbg("Chances de toucher du monstre " + monsterHitChance.ToString());

            // Tant que le joueur et le monstre sont en vie on combat
            // C'est un combat à mort
            while (gameInfos.Character.Pv > 0 && currentMonster.Pv > 0)
            {
                // Le joueur commence
                // Mettre la personne qui ouvre le combat en aléatoire ?
                double draw = rnd.NextDouble();
                // Le joueur touche
                if (draw < playerHitChance)
                {
                    dt.dbg("Le joueur a touché le monstre");
                    currentMonster.Pv -= gameInfos.Character.Force;
                }

                // Au tour du monstre
                draw = rnd.NextDouble();
                // Le monstre touche
                if (draw < monsterHitChance)
                {
                    dt.dbg("Le monstre a touché le joueur");
                    gameInfos.Character.Pv -= currentMonster.Force;
                }

            }

            if (gameInfos.Character.Pv <= 0)
            {
                dt.dbg("Le joueur est mort tué par un " + currentMonster.Nom);
                resultText = "Vous êtes mort tué par un " + currentMonster.Nom;
            }
            else
            {
                dt.dbg("Le joueur a tué un " + currentMonster.Nom);
                resultText = "Vous avez tué un " + currentMonster.Nom;
            }

            // On met à jour la vie du joueur
            db.UpdatePersonne(gameInfos.Character);
            return resultText;
        }

        public string HandleFuir(HarryPeloteur_DAL.GameInformationDTO gameInfos, string[] parameters)
        {
            var roomSearch = FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle);
            HarryPeloteur_DAL.SalleDTO currentRoom = roomSearch.found;
            int currentRoomID = roomSearch.index;
            string resultText = "";
            // Si la salle actuelle ne contient pas de monstre on annule l'action
            if (currentRoom.TypeContenu != 2)
            {
                dt.dbg("Impossible de fuir une salle sans monstres");
                return "Impossible de fuir une salle sans monstres";
            }

            // On obtient le monstre dans la salle actuelle
            HarryPeloteur_DAL.MonstreDTO currentMonster = db.GetMonstre(currentRoom.IdContenu);

            // Calcule la chance de s'échapper selon les caractéristiques du joueur et du monstre avec un minimum de 20%
            double escapeChance = (double)(gameInfos.Character.Fuite - currentMonster.Dexterite);
            escapeChance = escapeChance/(double)(gameInfos.Character.Fuite);
            escapeChance = Math.Max(0.20, escapeChance);

            while (gameInfos.Character.Pv > 0)
            {
                // Si on n'a pas réussi à s'échapper
                if (rnd.NextDouble() > escapeChance)
                {
                    dt.dbg("Le joueur n'as pas réussi à s'échapper");
                    // Alors le joueur prend des dégats
                    gameInfos.Character.Pv -= currentMonster.Force;
                }
                else
                {
                    dt.dbg("Le joueur a réussi à s'échapper");
                    // Sinon on sort vers une salle aléatoire
                    resultText = "Vous avez réussi à vous échapper";
                    // modifier l'état de la salle actuelle vers un état intermédiaire pour passer le check dans le handleavancer
                    gameInfos.Rooms[currentRoomID].Etat = 2;
                    resultText += HandleAvancer(gameInfos, new string[] { "", "random" });

                    // Met à jour les données du personnage après l'avoir déplacé
                    //gameInfos = db.getGameInfos(gameInfos.Game.Id);
                    gameInfos.Character = db.GetPersonne(gameInfos.Character.Id);

                    break;
                }
            }
            // Met à jour la vie du personnage
            db.UpdatePersonne(gameInfos.Character);
            return resultText;
        }

        public string HandleRamasser(HarryPeloteur_DAL.GameInformationDTO gameInfos, string[] parameters)
        {
            HarryPeloteur_DAL.SalleDTO currentRoom = FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle).found;

            if (currentRoom.TypeContenu != 1)
            {
                dt.dbg("Pas d'objet à ramasser dans la salle !");
                return "Pas d'objet à ramasser dans la salle !";
            }

            HarryPeloteur_DAL.ObjetDTO currentObject = db.GetObjet(currentRoom.IdContenu);

            switch (currentObject.ProprieteCible)
            {
                case "pv":
                    gameInfos.Character.Pv += currentObject.Montant;
                    break;
                case "force":
                    gameInfos.Character.Force += currentObject.Montant;
                    break;
                case "fuite":
                    gameInfos.Character.Fuite += currentObject.Montant;
                    break;
                case "dexterite":
                    gameInfos.Character.Dexterite += currentObject.Montant;
                    break;
                case "po":
                    gameInfos.Character.Po += currentObject.Montant;
                    break;
            }
            string returnText = (currentObject.Montant).ToString();
            if (currentObject.Montant < 0)
            {
                returnText = returnText + " " + currentObject.ProprieteCible;
            }
            if (currentObject.Montant > 0)
            {
                returnText = "+" + returnText + " " + currentObject.ProprieteCible;
            }

            db.UpdatePersonne(gameInfos.Character);
            return returnText;
        }

        public int? GenerateNewGame(string nomPerso, int difficultePartie)
        {
            HarryPeloteur_DAL.PersonneDTO perso = new HarryPeloteur_DAL.PersonneDTO()
            {
                Id = 0,
                SalleActuelle = 0,
                Nom = nomPerso,
                Pv = 10,
                Force = 10,
                Fuite = 10,
                Dexterite = 10,
                Xp = 10,
                Po = 10
            };
            perso.Id = db.InsertPersonne(perso);

            HarryPeloteur_DAL.PartieDTO partie = new HarryPeloteur_DAL.PartieDTO()
            {
                Id = 0,
                IdPersonnage = perso.Id,
                Difficulte = difficultePartie
            };
            partie.Id = db.InsertPartie(partie);
            HarryPeloteur_DAL.SalleDTO salle = new HarryPeloteur_DAL.SalleDTO()
            {
                Id = 0,
                IdPartie = partie.Id,
                Coordonnees = new int[] { 0, 0 },
                IdContenu = 0,
                TypeContenu = 0,
                Portes = new int[] { 1, 1, 1, 1 },
                Etat = 0
            };
            
            salle.Id = db.InsertSalle(salle);
            perso.SalleActuelle = salle.Id;
            db.UpdatePersonne(perso);

            return partie.Id;
        }

        public dynamic GenerateDisplayText(HarryPeloteur_DAL.GameInformationDTO gameInfos)
        {
            HarryPeloteur_DAL.SalleDTO currentRoom = FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle).found;
            var endText = "";
            int champ = 0; // champ de texte traité 0:intro; 1:maintext; 2:outro;
            int type = 0;
            //type 
            //0 : intro;
            //1 : vide;
            //2 : objet;
            //3 : monstre;
            //4 : outro;
            while (champ < 3)
            {
                switch (champ)
                {
                    case 0:
                        type = 0;
                        break;
                    case 1:
                        type = 1;
                        switch (currentRoom.TypeContenu)
                        {//on filtre par contenu de a salle pour demander le texte correspondant
                            case 1: //objet
                                type = 2;
                                break;
                            case 2: //monstre
                                type = 3;
                                break;
                            default: //rien
                                break;
                        }
                        break;
                    case 2:
                        type = 4;
                        break;
                }
                endText += db.GetTexte(type);
                champ++;
            }
            //actions possibles: 
            //mouvement 10-devant; 11-gauche; 12-droite; 13-derriere;
            //combat 20-combattre; 21-fuir;
            //objet 30-utiliser objet
            Dictionary<int, string> codeRetour = new Dictionary<int, string>();
            codeRetour.Add(10, "avancer haut");
            codeRetour.Add(11, "avancer droite");
            codeRetour.Add(12, "avancer bas");
            codeRetour.Add(13, "avancer gauche");
            codeRetour.Add(20, "combattre");
            codeRetour.Add(21, "fuir");
            codeRetour.Add(30, "ramasser");

            var actionsPossibles = new List<string> { };
            switch (currentRoom.Etat)
            {
                case 0: //salle non terminée
                    switch (currentRoom.TypeContenu)
                    {
                        case 0: //rien
                            for (int i = 0; i < 4; i++)
                            {
                                if (currentRoom.Portes[i] == 1)
                                {
                                    actionsPossibles.Add(codeRetour[i + 10]);
                                }
                            }
                            break;
                        case 1: //objet
                            actionsPossibles.Add(codeRetour[30]);
                            for (int i = 0; i < 4; i++)
                            {
                                if (currentRoom.Portes[i] == 1)
                                {
                                    actionsPossibles.Add(codeRetour[i + 10]);
                                }
                            }
                            break;
                        case 2:
                            actionsPossibles.Add(codeRetour[20]);
                            actionsPossibles.Add(codeRetour[20]);
                            break;
                    }
                    break;
                case 1: //salle terminée
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentRoom.Portes[i] == 1)
                        {
                            actionsPossibles.Add(codeRetour[i + 10]);
                        }
                    }
                    break;
            }

            return new { endText, actionsPossibles };
        }


    }

    public class LoadedDie
    {
        // Initializes a new loaded die.  Probs
        // is an array of numbers indicating the relative
        // probability of each choice relative to all the
        // others.  For example, if probs is [3,4,2], then
        // the chances are 3/9, 4/9, and 2/9, since the probabilities
        // add up to 9.
        public LoadedDie(int probs)
        {
            this.prob = new List<long>();
            this.alias = new List<int>();
            this.total = 0;
            this.n = probs;
            this.even = true;
        }

        Random random = new Random();

        List<long> prob;
        List<int> alias;
        long total;
        int n;
        bool even;

        public LoadedDie(IEnumerable<int> probs)
        {
            // Raise an error if nil
            if (probs == null) throw new ArgumentNullException("probs");
            this.prob = new List<long>();
            this.alias = new List<int>();
            this.total = 0;
            this.even = false;
            var small = new List<int>();
            var large = new List<int>();
            var tmpprobs = new List<long>();
            foreach (var p in probs)
            {
                tmpprobs.Add(p);
            }
            this.n = tmpprobs.Count;
            // Get the max and min choice and calculate total
            long mx = -1, mn = -1;
            foreach (var p in tmpprobs)
            {
                if (p < 0) throw new ArgumentException("probs contains a negative probability.");
                mx = (mx < 0 || p > mx) ? p : mx;
                mn = (mn < 0 || p < mn) ? p : mn;
                this.total += p;
            }
            // We use a shortcut if all probabilities are equal
            if (mx == mn)
            {
                this.even = true;
                return;
            }
            // Clone the probabilities and scale them by
            // the number of probabilities
            for (var i = 0; i < tmpprobs.Count; i++)
            {
                tmpprobs[i] *= this.n;
                this.alias.Add(0);
                this.prob.Add(0);
            }
            // Use Michael Vose's alias method
            for (var i = 0; i < tmpprobs.Count; i++)
            {
                if (tmpprobs[i] < this.total)
                    small.Add(i); // Smaller than probability sum
                else
                    large.Add(i); // Probability sum or greater
            }
            // Calculate probabilities and aliases
            while (small.Count > 0 && large.Count > 0)
            {
                var l = small[small.Count - 1]; small.RemoveAt(small.Count - 1);
                var g = large[large.Count - 1]; large.RemoveAt(large.Count - 1);
                this.prob[l] = tmpprobs[l];
                this.alias[l] = g;
                var newprob = (tmpprobs[g] + tmpprobs[l]) - this.total;
                tmpprobs[g] = newprob;
                if (newprob < this.total)
                    small.Add(g);
                else
                    large.Add(g);
            }
            foreach (var g in large)
                this.prob[g] = this.total;
            foreach (var l in small)
                this.prob[l] = this.total;
        }

        // Returns the number of choices.
        public int Count
        {
            get
            {
                return this.n;
            }
        }
        // Chooses a choice at random, ranging from 0 to the number of choices
        // minus 1.
        public int Next()
        {
            var i = random.Next(this.n);
            return (this.even || random.Next((int)this.total) < this.prob[i]) ? i : this.alias[i];
        }

    }
}