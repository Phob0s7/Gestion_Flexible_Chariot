using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    public class GestionRecettes
    {
        static string nomRecette = "";

        /// <summary>
        /// Permet d'afficher le menu recettes.
        /// </summary>
        public static void AffichageMenuRecettes()
        {
            Console.Clear();
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t      *** Recettes ***                          ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n1. Création de recettes");
            Console.WriteLine("2. Affichage de recettes");
            Console.WriteLine("3. Edition de recettes");
            Console.WriteLine("4. Effacement de recettes");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\t        *** Pas ***                             ");
            Console.WriteLine("__________________________________________________");
            Console.WriteLine("\n5. Afficher tous les pas d'une recette");
            //Console.WriteLine("6. Edition de pas");
            //Console.WriteLine("7. Effacement de pas");
            Console.WriteLine("\n6. Revenir au menu principale");
            Console.WriteLine("__________________________________________________");
        }

        /// <summary>
        /// Permet de saisir le choix de l'utilisateur en fonction du menu recettes.
        /// </summary>
        public static void ChoixMenuRecettes()
        {
            string choixMenuRecettes = "";
            bool saisieInvalide = false;

            do
            {
                saisieInvalide = false;

                Console.Write("Votre choix : ");
                choixMenuRecettes = Console.ReadLine();

                switch (choixMenuRecettes)
                {
                    case "1":
                        CréationRecettes();
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;
                    case "2":
                        AffichageRecettes();
                        GestionMenuPrincipale.EntrerSaisieUtilisateur();
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;
                    case "3":
                        AffichageRecettes();
                        ModifierRecette();
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;
                    case "4":
                        AffichageRecettes();
                        EffacerRecettes();
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;
                    case "5":
                        GestionRecettes.AffichageRecettes();
                        GestionPas.AfficherPas();
                        AffichageMenuRecettes();
                        ChoixMenuRecettes();
                        break;
                    //case "6":
                    //GestionPas.AfficherPas();
                    //IDPas = GestionPas.ChoisirPasAModifier();
                    //GestionPas.ModifierPas(IDPas);
                    //break;
                    //case "7":
                    //GestionPas.EffacerPas();
                    //break;

                    case "6":
                        GestionMenuPrincipale.AffichageMenuPrincipale();
                        break;
                    default:
                        saisieInvalide = true;
                        GestionMenuPrincipale.ErreurSaisieMenu();
                        break;
                }
            } while (saisieInvalide);
        }

        /// <summary>
        /// Permet de créer une recette.
        /// </summary>
        public static void CréationRecettes()
        {
            string choixAjouterRecette = "";
            int nombreRecette = 1;
            int nbreAjout = 0;
            string IDRecette = "";

            do
            {
                Console.Clear();
                Console.WriteLine("\t    Recette n° " + nombreRecette);
                Console.WriteLine("");
                nomRecette = GestionPas.VérifierSaisieNom("Veuillez saisir le nom de la recette : ");
                Console.WriteLine("");
                DateTime dateTime = DateTime.Now;

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO recette (REC_Nom, REC_DateCreation) VALUES (@nomRecette, @dateCréation);";

                        cmd.Parameters.AddWithValue("@nomRecette", nomRecette);
                        cmd.Parameters.AddWithValue("@dateCréation", dateTime);
                        nbreAjout += cmd.ExecuteNonQuery();
                        nombreRecette++;
                    }

                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "SELECT MAX(REC_ID) FROM RECETTE";

                        IDRecette = cmd.ExecuteScalar().ToString();
                    }


                    GestionPas.CréationPas(ref IDRecette);

                    choixAjouterRecette = GestionPas.ErreurSaisirChoix("\nVoulez-vouz ajouter une autre recette (O/N) ? : ", choixAjouterRecette);
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }

            } while (choixAjouterRecette != "N");

            Console.Write("\nNombre de recette ajoutés : {0}\n", nbreAjout);
            GestionMenuPrincipale.EntrerSaisieUtilisateur();
        }

        /// <summary>
        /// Permet d'afficher la liste des recettes.
        /// </summary>
        public static void AffichageRecettes()
        {
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM recette ";
                    string colonnes = "\nID {0,-10} Nom {0,-10} Date de création";
                    Console.WriteLine(string.Format(colonnes, "", "", ""));
                    Console.WriteLine("");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            Console.Write(string.Format("{0,-14}", reader["REC_ID"]));
                            Console.Write(string.Format("{0,-15}", reader["REC_Nom"]));
                            Console.Write(string.Format("{0,0}", reader["REC_DateCreation"]));
                            Console.WriteLine("");

                            compteur++;
                        }

                        Console.WriteLine("\n{0} recettes affichées.", compteur);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Write("\nAttention il y a eu le problème suivant : ");
                Console.Write(ex.Message);
                Console.Write("\n\n");
            }
        }

        /// <summary>
        /// Demande à l'utilisateur l'ID d'une recette
        /// </summary>
        public static string SaisirIDRecette()
        {
            string saisieUtilisateur = "";
            bool saisieValide = false;
            int résultat = 0;

            do
            {
                Console.Write("\nVeuillez saisir l'ID de la recette pour voir les pas : ");
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out résultat))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVeuillez saisir une valeur correcte (nombre(s)).");
                    Console.ResetColor();
                }

                else
                {
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return saisieUtilisateur;
        }

        /// <summary>
        /// Demande à l'utilisateur l'ID d'une recette
        /// </summary>
        public static string VérifierSaisieNombre(string messageSaisie)
        {
            string saisieUtilisateur = "";
            bool saisieValide = false;
            int résultat = 0;

            do
            {
                Console.Write(messageSaisie);
                saisieUtilisateur = Console.ReadLine();

                if (!int.TryParse(saisieUtilisateur, out résultat))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nVeuillez saisir une valeur correcte (nombre(s)).");
                    Console.ResetColor();
                }

                else
                {
                    saisieValide = true;
                }

            } while (saisieValide == false);

            return saisieUtilisateur;
        }
        /// <summary>
        /// Modifie la strucuture d'une recette
        /// </summary>
        public static void ModifierRecette()
        {
            string choixModifierRecette = "";
            int nbreModifié = 0;
            string nomRecette = "";
            bool saisieInvalide = false;
            List<string> listeIDRecettes = new List<string>();

            do
            {
                nomRecette = GestionPas.VérifierSaisieNom("\nVeuillez saisir le nouveau nom de la recette : ");

                do
                {
                    saisieInvalide = false;

                    Console.Write("Veuillez saisir l'ID de la recette à modifier : ");
                    string id = Console.ReadLine();

                    try
                    {
                        using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                        {
                            listeIDRecettes = StockerIDRecette();

                            if (listeIDRecettes.Contains(id) && nomRecette != "")
                            {
                                cmd.CommandText = $"UPDATE recette SET REC_Nom = @nom WHERE REC_ID = @id;";
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@nom", nomRecette);

                                choixModifierRecette = GestionPas.ErreurSaisirChoix("Voulez-vouz modifier une autre recette (O/N) ? : ", choixModifierRecette);
                                nbreModifié += cmd.ExecuteNonQuery();
                            }

                            else
                            {
                                saisieInvalide = true;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nL'ID de la recette n'existe pas, veuillez réessayer.");
                                Console.ResetColor();
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
                } while (saisieInvalide);

            } while (choixModifierRecette != "N");

            Console.WriteLine("\nNombre de recettes modifiés : {0}\n", nbreModifié);
            Console.Write("Veuillez appuyer sur une touche pour continuer... ");
            Console.ReadKey();
        }

        /// <summary>
        /// Vérifie si l'ID de la recette existe
        /// </summary>
        /// <returns>la liste des ID de la recette</returns>
        public static List<string> StockerIDRecette()
        {
            List<string> listeIDRecette = new List<string>();

            using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT REC_ID FROM recette ";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listeIDRecette.Add(reader.GetString(reader.GetOrdinal("REC_ID")));
                    }
                }
            }
            return listeIDRecette;
        }

        /// <summary>
        /// Permet d'effacer une recette.
        /// </summary>
        public static void EffacerRecettes()
        {
            string choixEffacerRecette = "";
            int nbreEffacés = 0;
            string saisieUtilisateur = "";
            List<string> listeIDRecettes = new List<string>();
            bool saisieInvalide = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nPour effacer une recette, il faut impérativement supprimer tous les pas qui lui sont associés.\n");
            Console.ResetColor();
            GestionMenuPrincipale.EntrerSaisieUtilisateur();
            Console.WriteLine("");

            do
            {
                saisieInvalide = false;

                saisieUtilisateur = VérifierSaisieNombre("\n\nVeuillez saisir l'ID de la recette à effacer : ");
                Console.WriteLine("");
                listeIDRecettes = StockerIDRecette();

                GestionPas.EffacerTousPasRecette(saisieUtilisateur);

                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        if (listeIDRecettes.Contains(saisieUtilisateur))
                        {


                            cmd.CommandText = "DELETE FROM recette WHERE REC_ID = @id;";
                            cmd.Parameters.AddWithValue("@id", saisieUtilisateur);

                            choixEffacerRecette = GestionPas.ErreurSaisirChoix("\nVoulez-vouz effacer une autre recette (O/N) ? : ", choixEffacerRecette);
                            nbreEffacés += cmd.ExecuteNonQuery();
                        }

                        else
                        {
                            saisieInvalide = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("L'ID de la recette n'existe pas, veuillez réessayer.\n");
                            Console.ResetColor();
                        }
                    }

                }
                catch (MySqlException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nAttention vous ne pouvez pas supprimer une recette associée à un lot.\n");
                    Console.ResetColor();
                    Console.Write("\nVeuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    Console.Write("\n\n");
                }
            } while (choixEffacerRecette != "N" || saisieInvalide == true);

            Console.Write("\nNombre de recettes effacés : {0}\n", nbreEffacés);
            GestionMenuPrincipale.EntrerSaisieUtilisateur();
        }
    }
}
