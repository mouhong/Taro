using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Data
{
    public class UnitOfWork : CommitableBase
    {
        private List<object> _records = new List<object>();

        public UnitOfWork()
            : base(Taro.Config.TaroEnvironment.Instance.EventDispatcher)
        {
        }

        public IQueryable<T> Query<T>()
        {
            return _records.OfType<T>().AsQueryable();
        }

        public void Save(object entity)
        {
            _records.Add(entity);
        }

        protected override void DoCommit()
        {
            Console.WriteLine("DB: Commit");
        }
    }
}
