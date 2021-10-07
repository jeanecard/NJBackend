using Dapper;
using Microsoft.Extensions.Configuration;
using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Commun;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Repositories.Interface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private IConfiguration _configuration = null;

        private GroupRepository()
        {
        }

        public GroupRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<GroupRawDB> GetCompteHeader(Guid idGroup)
        {
            string sql = "SELECT * FROM public.\"GROUP\" WHERE \"Id\" = :id::uuid";
            using var connection = new NpgsqlConnection(PGSqlTools.GetCxString(_configuration));
            using var groupsTask = connection.QueryAsync<GroupRawDB>(sql, new { id = idGroup.ToString() });
            await groupsTask;
            if (groupsTask.IsCompletedSuccessfully)
            {
                return groupsTask.Result?.FirstOrDefault();
            }
            else
            {
                throw new Exception("Cette faute envers moi était la denrière Obiwan.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GroupRawDB>> GetGroupsByAppartenanceAsync(Guid id)
        {
            string sql = "SELECT * FROM public.\"GROUP\" WHERE \"AppartenanceId\" = :appartenanceId::uuid";
           
            using var connection = new NpgsqlConnection(PGSqlTools.GetCxString(_configuration));
            using var groupsTask = connection.QueryAsync<GroupRawDB>(sql, new { appartenanceId = id.ToString() });
            await groupsTask;
            if(groupsTask.IsCompletedSuccessfully)
            {
                return groupsTask.Result?.ToList();
            }
            else
            {
                throw new Exception("Mais luke, tu l'a, déja fait.");
            }
        }

        public async Task<IEnumerable<GroupRawDB>> GetGroupsAsync()
        {
            string sql = "SELECT * FROM public.\"GROUP\"";

            using var connection = new NpgsqlConnection(PGSqlTools.GetCxString(_configuration));
            using var groupsTask = connection.QueryAsync<GroupRawDB>(sql);
            await groupsTask;
            if (groupsTask.IsCompletedSuccessfully)
            {
                return groupsTask.Result?.ToList();
            }
            else
            {
                throw new Exception("Excuses acceptées commandeur");
            }
        }

    }
}
