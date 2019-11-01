using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
   public class GestionLots
    {
        /// <summary>
        /// Permet d'afficher le menu des lots.
        /// </summary>
       public static void AffichageMenuLots()
        {
            Console.Clear();
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t        *** Menu lots ***                       ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n1. Création de lots");
            Console.WriteLine("2. Effacement de lots");
            Console.WriteLine("3. Edition de lots");
            Console.WriteLine("\n4. Revenir au menu principale");
            Console.WriteLine("__________________________________________________");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu lots.
        /// </summary>
        public static void ChoixMenuLots()
        {
            char choixMenuLots = ' ';

            Console.Write("Votre choix : ");
            choixMenuLots = char.Parse(Console.ReadLine());

            switch (choixMenuLots)
            {
                case '1':
                    CréationLots();
                    AffichageMenuLots();
                    ChoixMenuLots();
                    break;
                case '2':
                    AffichageLots();
                    EffacerLots();
                    AffichageMenuLots();
                    ChoixMenuLots();
                    break;
                case '3':
                    break;
                case '4':
                    GestionMenuPrincipale.AffichageMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuLots();
                    break;
            }

        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void ErreurSaisieMenuLots()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuLots();
            ChoixMenuLots();
        }



        /// <summary>
        /// Permet de créer un lot.
        /// </summary>
        public static void CréationLots()
        {
            int nbreLots = 1;
            int nbreAjout = 0;
            int qtePièceRéalisée = 0;
            int qtePièceAProduire = 0;
            int IDRecette = 0;
            int IDStatut = 0;
            char choixCréationLots = ' ';

            DateTime dateTime = DateTime.Now;

            do
            {
                Console.Clear();

                Console.WriteLine("               Lot n° " + nbreLots);
                Console.Write("\nQuelle est la quantité de pièce à produire ? : ");
                qtePièceAProduire = int.Parse(Console.ReadLine());
                GestionRecettes.AffichageRecettes();
                Console.Write("Quelle est l'ID de la recette a associer à ce lot ? : ");
                IDRecette = int.Parse(Console.ReadLine());
                Program.AffichageStatut();
                Console.Write("\n\nQuelle est l'ID du statut a associer à ce lot ? : ");
                IDStatut = int.Parse(Console.ReadLine());


                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO lot (LOT_QtePieceRealisee, LOT_QtePieceAProduire, LOT_DateCreation, REC_ID, STA_ID) VALUES (@pièceRéalisée, @pièceProduire, @date, @RECID, @STAID);";

                        cmd.Parameters.AddWithValue("@pièceRéalisée", qtePièceRéalisée);
                        cmd.Parameters.AddWithValue("@pièceProduire", qtePièceAProduire);
                        cmd.Parameters.AddWithValue("@date", dateTime);
                        cmd.Parameters.AddWithValue("@RECID", IDRecette);
                        cmd.Parameters.AddWithValue("@STAID", IDStatut);

                        Console.Write("\nVoulez-vous créer un nouveau lot ? (O/N) ");

                        choixCréationLots = char.Parse(Console.ReadLine().ToUpper());

                        nbreAjout += cmd.ExecuteNonQuery();
                        nbreLots++;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixCréationLots != 'N');

            Console.WriteLine("\nNombre de lots ajoutés : {0}", nbreAjout);
            Console.Write("\nVeuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'afficher la liste des lots.
        /// </summary>
        public static void AffichageLots()
        {
            Console.Clear();

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM lot";
                    Console.Write("\nID\t");
                    Console.Write("Quantité de lots réalisées\t\t".PadLeft(6));
                    Console.Write(" Quantité de lots à produire".PadLeft(12));
                    Console.Write(" Date de création".PadLeft(12));
                    Console.Write(" L'ID de la recette".PadLeft(12));
                    Console.Write(" Statut du lot".PadLeft(12));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0}\t\t {1}\t {2}\t\t {3}\t\t {4}\t\t {5}", reader["LOT_ID"], reader["LOT_QtePieceRealisee"], reader["LOT_QtePieceAProduire"], reader["LOT_DateCreation"], reader["REC_ID"], reader["STA_ID"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} lots affichés.\n", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                Console.ReadKey();
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Permet d'effacer un lot.
        /// </summary>
        public static void EffacerLots()
        {
            char choixEffacerLot = ' ';
            int nbreEffacés = 0;

            do
            {
                Console.Write("Veuillez saisir l'ID du lot à effacer : ");
                int id = int.Parse(Console.ReadLine());

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM lot WHERE LOT_ID = @id;";
                        cmd.Parameters.AddWithValue("@id", id);

                        Console.Write("\nVoulez-vouz effacer un autre lot (O/N) ? : ");

                        choixEffacerLot = char.Parse(Console.ReadLine().ToUpper());
                        nbreEffacés += cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixEffacerLot != 'N');

            Console.WriteLine("\nNombre de lots effacés : {0}\n", nbreEffacés);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

    }
}
