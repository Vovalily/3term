using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EntityRepository : IRepository<Player>
    {
        private readonly DataContext _context;

        public EntityRepository(DataContext context)
        {
            _context = context;
        }

        public void Create (Player obj)
        {
            _context.Set<Player>().Add(obj);
            _context.SaveChanges();
        }

        public IEnumerable<Player> ReadAll() 
        {
            return new List<Player>(_context.Set<Player>());
        }
        public Player ReadById(int id) 
        { 
            return _context.Players.Where(o => o.Id == id).FirstOrDefault();
        }
        public void Update(Player obj) 
        { 
            Player player = _context.Players.Where(o => o.Id == obj.Id).FirstOrDefault();
            player.Name = obj.Name;
            player.Level = obj.Level;
            player.Rank = obj.Rank;
            player.Score = obj.Score;
            player.RegistrationDate = obj.RegistrationDate;
            _context.SaveChanges();

        }
        public void Delete(Player obj) 
        {
            var player = _context.Players.Find(obj.Id);
            if (player != null)
            {
                _context.Players.Remove(player);
                _context.SaveChanges();
            }
            ;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}