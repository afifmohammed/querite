using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Querite;

namespace Tests
{
    public interface IDataSource
    {
        IQueryable<T> Query<T>();
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Defaulted { get; set; }
    }
    
    public class BadCreditCustomersQuery : AbstractQuery<Customer, IDataSource>
    {
        public override IEnumerable<Customer> Query(IDataSource source)
        {
            var matching = source.Query<Customer>().Where(x => x.Defaulted);
            Total = matching.Count();
            return matching.Take(10);
        }
    }

    [TestFixture]
    public class QueryUsage
    {
        private IQuery<IDataSource> DataSourceQuery { get; set; }
        public void DriveByDemo()
        {
            int count;
            var customers = DataSourceQuery.Count(out count).Execute(new BadCreditCustomersQuery());
        }
    }
}
