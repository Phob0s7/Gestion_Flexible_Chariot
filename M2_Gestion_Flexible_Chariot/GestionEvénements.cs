using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient la gestion des événements.
    /// </summary>
    public class GestionEvénements
    {
        /// <summary>
        /// Affiche les événements en fonction du lot saisi.
        /// </summary>
        public static void AfficherHistoriqueLot()
        {
            string IDHistoriqueLot = "";
            List<string> listeLots = new List<string>();

            do
            {
                try
                {
                    using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                    {
                        cmd.CommandText = "SELECT LOT_ID FROM lot ";

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listeLots.Add(reader.GetString(reader.GetOrdinal("LOT_ID"))); // Permet d'ajouter à une liste tous les ID de la table lot.
                            }
                        }

                        if (listeLots.Count != 0)
                        {
                            Console.Write("\nVeuillez saisir l'ID du lot pour afficher ses événements : ");
                            IDHistoriqueLot = Console.ReadLine();
                            Console.Write("\n");

                            if (listeLots.Contains(IDHistoriqueLot))
                            {
                                cmd.CommandText = $"SELECT EVE_Libelle, EVE_Date, LOT_ID FROM evenement WHERE LOT_ID = {IDHistoriqueLot}";
                                string colonnes = "Libellé {0,-31} Date de création\n";
                                Console.Write(string.Format(colonnes, "", "", ""));

                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    int compteur = 0;
                                    Console.Write("\n");
                                    while (reader.Read())
                                    {
                                        Console.Write(string.Format("{0,-40}", reader["EVE_Libelle"]));
                                        Console.Write(string.Format("{0,-28}", reader["EVE_Date"]));
                                        Console.WriteLine("");

                                        compteur++;
                                    }
                                    Console.Write("\n");
                                    Console.WriteLine("{0} événement(s) affiché(s).", compteur);
                                    GestionMenuPrincipale.AttenteSaisieUtilisateur();
                                }
                            }

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("L'ID du lot n'existe pas, veuillez réessayer.");
                                Console.ResetColor();

                            }
                        }

                        else
                        {
                            Console.Clear();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.Write("\nAttention il y a eu le problème suivant : ");
                    Console.Write(ex.Message);
                    GestionMenuPrincipale.AttenteSaisieUtilisateur();
                    Console.Write("\n\n");
                }

            } while (!listeLots.Contains(IDHistoriqueLot) && listeLots.Count != 0);
        }
    }
}
