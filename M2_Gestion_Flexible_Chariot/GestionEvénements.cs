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
            string IDHistoriqueLot = "";
            List<string> listeLots = new List<string>();

            do
            {

                Console.Write("Veuillez saisir l'ID du lot pour afficher les événements : ");
                IDHistoriqueLot = Console.ReadLine();
                Console.Write("\n");

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "SELECT LOT_ID FROM lot ";

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listeLots.Add(reader.GetString(reader.GetOrdinal("LOT_ID")));
                            }
                        }

                        if (listeLots.Contains(IDHistoriqueLot))
                        {

                            cmd.CommandText = $"SELECT EVE_Libelle, EVE_Date, LOT_ID FROM evenement WHERE LOT_ID = {IDHistoriqueLot} ";
                            string colonnes = "Libellé {0,-15} Date de création {0,-15} ID du lot\n";
                            Console.Write(string.Format(colonnes, "", "", ""));

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                int compteur = 0;
                                Console.Write("\n");
                                while (reader.Read())
                                {
                                    Console.Write(string.Format("{0,-24}", reader["EVE_Libelle"]));
                                    Console.Write(string.Format("{0,-33}", reader["EVE_Date"]));
                                    Console.Write(string.Format("{0,0}", reader["LOT_ID"]));

                                    compteur++;
                                }
                                Console.Write("\n");
                                Console.WriteLine("\n{0} événement(s) affiché(s).", compteur);
                                GestionMenuPrincipale.EntrerSaisieUtilisateur();
                            }
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("L'ID du lot n'existe pas, veuillez réessayer.\n");
                            Console.ResetColor();
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
            } while (!listeLots.Contains(IDHistoriqueLot));
        }
    }
}