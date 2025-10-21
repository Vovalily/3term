using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DapperRepository : IRepository<Player>
    {

        private string connectionString;

        public DapperRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
       
        private void UseScript(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    db.Execute(script);
                }
            }
        }

        public void Create(Player player)
        {
            string script = "INSERT INTO Players (Name, Level, Score, Rank, RegistrationDate) VALUES(" + player.Name + player.Level + player.Score + player.Score + player.RegistrationDate +")";
            UseScript(script);
        }

        public IEnumerable<Player> ReadAll()
        {
            List<Player> players;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                players = db.Query<Player>("SELECT * FROM Players ").ToList();

            }
            return players;

        }


        public Player ReadById(int id)
        {
            Player players;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                players = db.Query<Player>("SELECT * FROM Players WHERE Id = " + id).FirstOrDefault();

            }
            return players;
        }
            

        public void Update(Player player)
        {
            string script = "UPDATE Players SET Name, Level, Score, Rank, RegistrationDate  = '" + 
                player.Name + player.Level + player.Score + player.Score + player.RegistrationDate +
                "' WHERE Id = " + player.Id;
            UseScript(script);
        }

        public void Delete(Player player)
        {
            string script = "DELETE FROM Players WHERE Id = " + player.Id;
            UseScript(script);
        }
    }
}
