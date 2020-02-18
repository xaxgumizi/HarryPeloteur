using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HarryPeloteur_BL.Controllers
{
    public class GameLogic
    {
        Random rnd = new Random();
        dbController db = new dbController();
        DebugTools dt = new DebugTools();

        // Ordre des instructions
        // Récupérer les infos depuis BDD
        // Appeler fonction qui fait traitement en passant les infos
        // La fonction modifie les infos puis les update dans la BDD
        // Obligé car on ne connait pas le nouvel ID dans la BDD avant l'insertion
        // On redemande les infos depuis la BDD

        public void HandleAvancer(Models.GameInformationDTO gameInfos, string[] parameters)
        {
            dt.dbg("Call to avancer");
            if(parameters.Count() < 2)
            {
                return;
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
                    return;
            }

            Models.SalleDTO currentRoom = FindRoomById(gameInfos.rooms, gameInfos.character.salle_actuelle).found;
            // Si on est face à un monstre non combattu on ne peut pas simplement partir
            if (currentRoom.type_contenu == 2 && currentRoom.etat == 0)
            {
                dt.dbg("Mode combat déplacement impossible");
                return;
            }

            // Si il faut tirer une direction au hasard, pour une fuite par exemple
            if (direction == 4)
            {
                dt.dbg("Salle aléatoire");
                //var candidates = currentRoom.portes.Where(x => x == 1).ToArray();
                // On cherche toutes les portes de la salle
                List<int> candidates = new List<int>();
                foreach (var item in currentRoom.portes.Select((value, i) => (value, i)))
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
            if (currentRoom.portes[direction] == 1)
            {
                int[] currentcoordinates = { 0, 0 };
                currentcoordinates[0] = currentRoom.coordonnees[0];
                currentcoordinates[1] = currentRoom.coordonnees[1];


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

                Models.SalleDTO existingroom = FindRoomByCoordinates(gameInfos.rooms, currentcoordinates); // Cherche une salle au nouvel emplacement
                if (existingroom != null) // Si il existe déjà une salle à cet emplacement
                {
                    dt.dbg("La salle existe pour le déplacement");
                    gameInfos.character.salle_actuelle = existingroom.id; // Alors on déplace juste le personnage dedans
                }
                else // Sinon on doit générer une nouvelle salle
                {
                    dt.dbg("On créé une salle pour le déplacement");
                    // La porte opposée de celle d'où l'on vient
                    int[] oppposingDirections = { 2, 3, 0, 1 };

                    Models.SalleDTO newroom = GenerateNewRoom();

                    newroom.id_partie = gameInfos.game.id; // On assigne l'id de la partie et ses nouvelles coordonnées
                    newroom.coordonnees = currentcoordinates;
                    // On place la porte pour retourner de là où on vient
                    newroom.portes[oppposingDirections[direction]] = 1;

                    // On insère dans la BDD la nouvelle salle
                    db.insertRoom(newroom);

                    // On récupère de nouveau les salles pour avoir l'id de la salle que l'on vient d'insérer
                    gameInfos.rooms = db.getRoom(gameInfos.game.id);
                    // On considère que les résultats sont triés par id croissant, donc on prend la dernière salle
                    int newRoomId = gameInfos.rooms.Last().id;

                    // On déplace le personnage dans la nouvelle salle
                    gameInfos.character.salle_actuelle = newRoomId;
                }

                db.updateCharacter(gameInfos.character); // On met à jour dans la BDD le personnage
            }
            else // Sinon on ne peut pas aller là
            {
                dt.dbg("Pas de porte dans la direction choisie");
                return;
            }
        }

        public dynamic FindRoomById(List<Models.SalleDTO> rooms, int id)
        {
            Models.SalleDTO found = null;
            int index = 0;
            foreach (var room in rooms.Select((value, i) => (value, i)))
            {
                if (room.value.id == id)
                {
                    found = room.value;
                    index = room.i;
                    break;
                }
            }
            return new { found, index };
        }

        public Models.SalleDTO FindRoomByCoordinates(List<Models.SalleDTO> rooms, int[] coordinates)
        {
            dt.dbg("Call to find room by coordinates");
            Models.SalleDTO found = null;

            //dt.PrintArray(coordinates);

            foreach (Models.SalleDTO room in rooms)
            {
                //dt.PrintArray(room.coordonnees);
                if (room.coordonnees[0] == coordinates[0] && room.coordonnees[1] == coordinates[1])
                {
                    found = room;
                    break;
                }
            }

            return found;
        }
        
        /* Génère une nouvelle salle avec des attributs aléatoires
         */
        public Models.SalleDTO GenerateNewRoom()
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
                    var randomObject = new LoadedDie(new int[] { 20, 10, 10, 10, 10, 5, 20, 10, 5 }); // regagne 25% de la vie, regagne 50% de la vie, augmente 10% de force, augmente de 10% la fuite, augmente de 10% la dexterité, augmente de 10% les PV, 10PO, 50PO, 100PO
                    contentId = randomObject.Next();
                    break;
                case 2: // Il y a un monstre
                    var randomMonster = new LoadedDie(new int[] { 10, 10, 10, 10, 20, 20, 9, 9, 2}); // voir table des monstres
                    contentId = randomMonster.Next();
                    break;
            }

            var room = new Models.SalleDTO()
            {
                id = 0,
                id_partie = 0,
                coordonnees = new int[] { 0, 0 },
                id_contenu = contentId,
                type_contenu = contentType,
                portes = new int[] { // Chaque porte a 50% de chance d'être présente
                    rnd.Next(0,2),
                    rnd.Next(0,2),
                    rnd.Next(0,2),
                    rnd.Next(0,2) },
                etat = 0
            };

            return room;
        }

        public void HandleCombattre(Models.GameInformationDTO gameInfos, string[] paramaters)
        {
            Models.SalleDTO currentRoom = FindRoomById(gameInfos.rooms, gameInfos.character.salle_actuelle).found;

            if(currentRoom.type_contenu != 2)
            {
                dt.dbg("Impossible de combattre dans une salle sans monstres");
                return;
            }

            Models.MonstreDTO currentMonster = db.getMonster(currentRoom.id_contenu);

            // On détermine les chances de toucher en fonction de la dextérité
            //double playerHitChance = 0.5 * Math.Pow((double)gameInfos.character.dexterite / (double)currentMonster.Dexterite, 2);
            double playerHitChance = Math.Min(0.9, 0.5 + 0.5 * (((double)gameInfos.character.dexterite - (double)currentMonster.Dexterite) / (double)currentMonster.Dexterite));
            double monsterHitChance = Math.Min(0.9, 0.5 + 0.5 * (((double)currentMonster.Dexterite - (double)gameInfos.character.dexterite) / (double)gameInfos.character.dexterite));
            // ajouter une fonction pour faire ça
            // diviser par deux l'augmentation de la chance ?
            // mettre un plafond de 90%
            dt.VarDump(currentMonster);
            dt.dbg(gameInfos.character.dexterite.ToString());
            dt.dbg(currentMonster.Dexterite.ToString());
            dt.dbg((gameInfos.character.dexterite / currentMonster.Dexterite).ToString());
            dt.dbg("Chances de toucher du joueur " + playerHitChance.ToString());
            dt.dbg("Chances de toucher du monstre " + monsterHitChance.ToString());

            // Tant que le joueur et le monstre sont en vie on combat
            // C'est un combat à mort
            while (gameInfos.character.pv > 0 && currentMonster.Pv > 0)
            {
                // Le joueur commence
                // Mettre la personne qui ouvre le combat en aléatoire ?
                double draw = rnd.NextDouble();
                // Le joueur touche
                if(draw < playerHitChance)
                {
                    dt.dbg("Le joueur a touché le monstre");
                    currentMonster.Pv -= gameInfos.character.force;
                }

                // Au tour du monstre
                draw = rnd.NextDouble();
                // Le monstre touche
                if(draw < monsterHitChance)
                {
                    dt.dbg("Le monstre a touché le joueur");
                    gameInfos.character.pv -= currentMonster.Force;
                }
                
            }

            if(gameInfos.character.pv <= 0)
            {
                dt.dbg("Le joueur est mort tué par un " + currentMonster.Nom);
            }
            else
            {
                dt.dbg("Le joueur a touché un " + currentMonster.Nom);
            }

            // On met à jour la vie du joueur
            db.updateCharacter(gameInfos.character);
        }

        public void HandleFuir(Models.GameInformationDTO gameInfos, string[] parameters)
        {
            var roomSearch = FindRoomById(gameInfos.rooms, gameInfos.character.salle_actuelle);
            Models.SalleDTO currentRoom = roomSearch.found;
            int currentRoomID = roomSearch.index;

            // Si la salle actuelle ne contient pas de monstre on annule l'action
            if (currentRoom.type_contenu != 2)
            {
                dt.dbg("Impossible de fuir une salle sans monstres");
                return;
            }

            // On obtient le monstre dans la salle actuelle
            var currentMonster = db.getMonster(currentRoom.id_contenu);

            // Calcule la chance de s'échapper selon les caractéristiques du joueur et du monstre avec un minimum de 20%
            var escapeChance = Math.Max(0.20, (gameInfos.character.fuite - currentMonster.Dexterite)/gameInfos.character.fuite);
            
            while(gameInfos.character.pv > 0)
            {
                // Si on n'a pas réussi à s'échapper
                if (rnd.NextDouble() > escapeChance)
                {
                    dt.dbg("Le joueur n'as pas réussi à s'échapper");
                    // Alors le joueur prend des dégats
                    gameInfos.character.pv -= currentMonster.Force;
                }
                else
                {
                    dt.dbg("Le joueur a réussi à s'échapper");
                    // Sinon on sort vers une salle aléatoire

                    // modifier l'état de la salle actuelle vers un état intermédiaire pour passer le check dans le handleavancer
                    gameInfos.rooms[currentRoomID].etat = 2;
                    HandleAvancer(gameInfos, new string[] { "", "random" });

                    // Met à jour les données du personnage après l'avoir déplacé
                    //gameInfos = db.getGameInfos(gameInfos.game.id);
                    gameInfos.character = db.getCharacter(gameInfos.character.id);
                    //dt.VarDump(gameInfos.character);

                    break;
                }
            }
            // Met à jour la vie du personnage
            db.updateCharacter(gameInfos.character);
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