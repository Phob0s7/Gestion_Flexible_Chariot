using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    public class GestionEvénements
    {
        /*
        /// <summary>
        /// Permet d'afficher le menu événements.
        /// </summary>
        public static void AffichageMenuEvénements()
        {
            Console.Clear();
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t      *** Menu événements ***                     ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("1. Affichage des événements");
            Console.WriteLine("\n2. Revenir au menu principale");
            Console.WriteLine("__________________________________________________");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu recettes.
        /// </summary>
        public static void ChoixMenuEvénements()
        {
            char choixMenuEvénements = ' ';

            Console.Write("Votre choix : ");
            choixMenuEvénements = char.Parse(Console.ReadLine());

            switch (choixMenuEvénements)
            {
                case '1':
                    AffichageMenuEvénements();
                    ChoixMenuEvénements();
                    break;
                case '2':
                    MenuPrincipale.AffichageMenuPrincipale();
                    break;
                default:
                    ErreurSaisieMenuEvénements();
                    break;
            }

        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void ErreurSaisieMenuEvénements()
        {
            Console.Write("\nErreur de saisie, veuillez appuyer sur une touche pour recommencer la saisie... ");
            Console.WriteLine(Console.ReadKey());
            Console.Clear();
            AffichageMenuEvénements();
            ChoixMenuEvénements();
        }
        */

        /// <summary>
        /// Permet d'afficher la liste des événements.
        /// </summary>
        /// 

        /// <summary>
        /// Affiche l'historique d'un lot choisi.
        /// </summary>

        public static void AfficherHistoriqueLot()
        {
            Console.Write("Veuillez saisir l'ID du lot pour afficher les événements : ");
            string IDHistoriqueLot = Console.ReadLine();
            Console.WriteLine("\n");

            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM evenement WHERE LOT_ID = {IDHistoriqueLot} ";
                    Console.Write("ID\t");
                    Console.Write("Libellé\t");
                    Console.Write("Date de création\t");
                    Console.Write("l'ID du lot\t");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine("\n{0} {1} {2} {3}", reader["EVE_ID"], reader["EVE_Libelle"], reader["EVE_Date"], reader["LOT_ID"]);
                            compteur++;
                        }

                        Console.WriteLine("\n{0} événements affichés.\n", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                GestionMenuPrincipale.EntrerSaisieUtilisateur();
                Console.Write("\n\n");
            }
            Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}