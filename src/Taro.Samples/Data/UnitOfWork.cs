using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Data
{
    public class UnitOfWork : UnitOfWorkBase
    {
        private List<object> _records = new List<object>();

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
