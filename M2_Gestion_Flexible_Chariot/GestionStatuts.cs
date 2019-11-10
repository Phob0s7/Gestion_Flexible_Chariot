using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient la gestion des statuts d'un lot.
    /// </summary>
    class GestionStatuts
    {
        /// <summary>
        /// Obtient l'ID du statut "En attente de production".
        /// </summary>
        /// <returns>l'ID du statut.</returns>
        public static int ObtenirIDStatut()
        {
            int IDStatut = 0;
            try
            {
                using (MySqlCommand cmd = GestionBaseDeDonnée.GetMySqlConnection().CreateCommand())
                {
                    cmd.CommandText = $"SELECT STA_ID FROM statut WHERE STA_ID = {1}";

                    IDStatut = (int)cmd.ExecuteScalar();
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

            return IDStatut;
        }
    }
}
