using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M2_Gestion_Flexible_Chariot
{
    public class GestionMenuPrincipale
    {
        /// <summary>
        /// Permet d'afficher le menu principale.
        /// </summary>
        public static void AffichageMenuPrincipale()
        {
            Console.Clear();
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t     *** Menu principale ***                    ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n1. Gestion de recettes");
            Console.WriteLine("2. Gestion de lots");
            Console.WriteLine("3. Historique de lots\n");
            Console.WriteLine("4. Quitter le programme");
            Console.WriteLine("__________________________________________________");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu principale.
        /// </summary>
        public static void ChoixMenuPrincipale()
        {
            string choixMenuPrincipale = "";
            do
            {
                Console.Write("Votre choix : ");
                choixMenuPrincipale = Console.ReadLine();

                switch (choixMenuPrincipale)
                {
                    case "1":
                        GestionRecettes.AffichageMenuRecettes();
                        GestionRecettes.ChoixMenuRecettes();
                        break;

                    case "2":
                        GestionLots.AffichageMenuLots();
                        GestionLots.ChoixMenuLots();
                        break;

                    case "3":
                        GestionLots.AffichageLots();
                        GestionEvénements.AfficherHistoriqueLot();
                        AffichageMenuPrincipale();
                        break;

                    case "4":
                        Console.WriteLine();
                        break;

                    default:
                        ErreurSaisieMenu();
                        break;
                }
            } while (choixMenuPrincipale != "4");
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void ErreurSaisieMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nVous n'avez pas saisi un choix correcte.\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Attend la saisi d'une touche auprès de l'utlisateur.
        /// </summary>
        public static void EntrerSaisieUtilisateur()
        {
            Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
