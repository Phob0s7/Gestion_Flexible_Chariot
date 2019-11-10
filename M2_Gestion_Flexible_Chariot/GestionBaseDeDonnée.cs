using MySql.Data.MySqlClient;
using System;


namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient la gestion de la base de données.
    /// </summary>
    static class GestionBaseDeDonnée
    {
        static public MySqlConnection connexion;

        /// <summary>
        /// Etablit une connexion à la base de données.
        /// </summary>
        /// <param name="databaseName">Nom de la base de données</param>
        /// <param name="userName">Nom d'utilisateur</param>
        /// <param name="password">Mot de passe</param>
        static public bool ConnectToDB(string databaseName, string userName, string password)
        {
            bool connexionAutorisée = true;

            string connectionString = $"server=localhost;database={databaseName};Uid={userName};Pwd={password};";
            //string connectionString = $"server=172.16.100.9;database={databaseName};Uid={userName};Pwd={password};";

            connexion = new MySqlConnection(connectionString);

            try
            {
                connexion.Open();
            }
            catch (MySqlException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Problème lors de la connexion à la base de donnée : ");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.WriteLine("");
                connexionAutorisée = false;
            }

            return connexionAutorisée;
        }

        /// <summary>
        /// Donne un accès à l'objet de connexion MySql.
        /// </summary>
        /// <returns>la connexion MySQL</returns>
        static public MySqlConnection GetMySqlConnection()
        {
            return connexion;
        }

        /// <summary>
        /// Ferme la connexion à la base de données.
        /// </summary>
        static public void DisconnectFromDB()
        {
            if (connexion != null)
                connexion.Close();
        }
    }
}
