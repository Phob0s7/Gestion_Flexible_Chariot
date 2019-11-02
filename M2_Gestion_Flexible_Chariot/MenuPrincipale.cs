using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M2_Gestion_Flexible_Chariot
{
    public class MenuPrincipale
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
            Console.WriteLine("3. Gestion des événements\n");
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
                        GestionEvénements.AffichageMenuEvénements();
                        GestionEvénements.ChoixMenuEvénements();
                        break;

                    case "4":
                        Console.WriteLine();
                        break;

                    default:
                        ErreurSaisieMenuPrincipale();
                        break;
                }
            } while (choixMenuPrincipale != "4");
        }

        /// <summary>
        /// Permet d'écrire quelque chose par défaut lors d'une erreur.
        /// </summary>
        public static void ErreurSaisieMenuPrincipale()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nVous n'avez pas saisi un caractère correcte.\n");
            Console.ResetColor();
            Console.Write("\nVeuillez réessayer en appuyant sur une touche...");
            Console.ReadKey();
            Console.Clear();
            AffichageMenuPrincipale();
            ChoixMenuPrincipale();
        }
    }
}
