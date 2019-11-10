namespace M2_Gestion_Flexible_Chariot
{
    /// <summary>
    /// Classe qui contient le programme principal.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Point d'entrée du programme.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool connexionAutorisée = GestionBaseDeDonnée.ConnectToDB("gestion_flexible_chariot", "root", "");
          //bool connexionAutorisée = GestionBaseDeDonnée.ConnectToDB("villsyl_bourrob_gestion_flexible_chariot", "villsyl", "EST2019");
          //
            if (connexionAutorisée)
            {
                GestionMenuPrincipale.AfficherMenuPrincipale();
                GestionMenuPrincipale.ChoisirMenuPrincipale();
                GestionBaseDeDonnée.DisconnectFromDB();
            }
        }
    }
}


