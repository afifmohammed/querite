using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ninject;
using querite;

namespace Tests
{
    public class DataSource
    {
        public DataSource()
        {
            Query = new List<string>();
        }

        public List<string> Query { get; set; }
    }
    
    public class LongStringsQuery : AbstractQuery<IEnumerable<string>, DataSource>
    {
        public int Length { get; set; }

        public override IEnumerable<string> Apply(DataSource source)
        {
            var matching = source.Query.Where(x => x.Length > Length).AsQueryable();
            SetCount(matching.Count());
            return matching.Take(10);
        }
    }

    [TestFixture]
    public class QueryUsage
    {
        [Test]
        public void DriveByDemo()
        {
            int count = 0;
            IEnumerable<string> matches;
            using(var k = new StandardKernel())
            {
                k.Load<Querite>();
                k.Bind<DataSource>().ToSelf().InSingletonScope();

                k.Get<DataSource>().Query.AddRange(new [] {"ab", "long", "verylong", "12"});
                matches = k.Get<IQuery<DataSource>>().Count(x => count = x).Execute(new LongStringsQuery {Length = 3});
            }

            Assert.AreEqual(2, count);
            Assert.IsNotNull(matches);
            Assert.IsTrue(matches.Count() == 2);
            Assert.IsTrue(matches.Contains("long"));
            Assert.IsTrue(matches.Contains("verylong"));
        }
    }
}

