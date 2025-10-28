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

        public void Create(Player player)
        {
            string script = @"INSERT INTO Players (Name, Level, Score, Rank, RegistrationDate) 
                             VALUES (@Name, @Level, @Score, @Rank, @RegistrationDate)";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute(script, new
                {
                    Name = player.Name,
                    Level = player.Level,
                    Score = player.Score,
                    Rank = player.Rank,
                    RegistrationDate = player.RegistrationDate
                });
            }
        }

        public IEnumerable<Player> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Player>("SELECT * FROM Players").ToList();
            }
        }


        public Player ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Player>("SELECT * FROM Players WHERE Id = @Id", new { Id = id })
                       .FirstOrDefault();
            }
        }


        public void Update(Player player)
        {
            string script = @"UPDATE Players 
                             SET Name = @Name, 
                                 Level = @Level, 
                                 Score = @Score, 
                                 Rank = @Rank, 
                                 RegistrationDate = @RegistrationDate 
                             WHERE Id = @Id";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute(script, new
                {
                    Id = player.Id,
                    Name = player.Name,
                    Level = player.Level,
                    Score = player.Score,
                    Rank = player.Rank,
                    RegistrationDate = player.RegistrationDate
                });
            }
        }

        public void Delete(Player player)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("DELETE FROM Players WHERE Id = @Id", new { Id = player.Id });
            }
        }
    }
}
