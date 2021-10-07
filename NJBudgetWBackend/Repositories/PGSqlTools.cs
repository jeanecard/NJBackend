using Microsoft.Extensions.Configuration;
using NJBudgetWBackend.Commun;
using System;

namespace NJBudgetWBackend.Repositories
{
    public static class PGSqlTools
    {
        private const String _SERVER_KEY = "elephantSQL:server";
        private const String _DATABASE_KEY = "elephantSQL:database";
        private const String _USER_KEY = "elephantSQL:user";
        private const String _PWD_KEY = "elephantSQL:password";
        public static String GetCxString(IConfiguration configuration)
        {
            if (configuration == null)
            {
                return String.Empty;
            }
            String retour = String.Empty;
            retour = String.Format(Constant.ELEPHANTSQL_CONNEXION_STRING,
                               configuration[_SERVER_KEY],
                               configuration[_DATABASE_KEY],
                               configuration[_USER_KEY],
                               configuration[_PWD_KEY],
                               "5432");
            return retour;
        }
    }
}
