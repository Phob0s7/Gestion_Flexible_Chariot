using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient la gestion du menu principale.
    /// </summary>
    public class GestionMenuPrincipale
    {
        /// <summary>
        /// Permet d'afficher le menu principale.
        /// </summary>
        public static void AfficherMenuPrincipale()
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
        public static void ChoisirMenuPrincipale()
        {
            string choisirMenuPrincipale = "";

            do
            {
                Console.Write("Votre choix : ");
                choisirMenuPrincipale = Console.ReadLine();

                switch (choisirMenuPrincipale)
                {
                    case "1":
                        GestionRecettes.AfficherMenuRecettes();
                        GestionRecettes.ChoisirMenuRecettes();
                        break;

                    case "2":
                        GestionLots.AfficherMenuLots();
                        GestionLots.ChoisirMenuLots();
                        break;

                    case "3":
                        GestionLots.AfficherLots();
                        GestionEvénements.AfficherHistoriqueLot();
                        AfficherMenuPrincipale();
                        break;

                    case "4":
                        Console.WriteLine();
                        break;

                    default:
                        AfficherErreurSaisieMenu();
                        break;
                }
            } while (choisirMenuPrincipale != "4");
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void AfficherErreurSaisieMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nVous n'avez pas saisi un choix correcte.\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Attend la saisie d'une touche auprès de l'utilisateur.
        /// </summary>
        public static void AttenteSaisieUtilisateur()
        {
            Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
