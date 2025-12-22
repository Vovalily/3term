using DataAccessLayer;
using Model;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class SimpleConfigModule : NinjectModule
    {
        private readonly bool _useDapper;

        public SimpleConfigModule(bool useDapper = false)
        {
            _useDapper = useDapper;
        }

        public override void Load()
        {
            Bind<DataContext>().ToSelf().InThreadScope();
            Bind<BattleService>().ToSelf().InSingletonScope();

            if (_useDapper)
            {
                Bind<IRepository<Player>>().To<DapperRepository>().InSingletonScope();
            }
            else
            {
                Bind<IRepository<Player>>().To<EntityRepository>().InSingletonScope();
            }
        }
    }
}
