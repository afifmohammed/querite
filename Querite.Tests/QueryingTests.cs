using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Querite.Tests
{
    [TestFixture]
    public class QueryingTests
    {
        public class Phone
        {
            public Phone()
            {
                Contacts = new EnumerableQuery<string>(new []{"Afif Mohammed", "Shaun Marsh", "Ravi Kishan", "Frank Cannata"});
            }

            public IQueryable<string> Contacts { get; private set; }
        }

        public class MatchingNamesQuery : AbstractQuery<IEnumerable<string>, Phone>
        {
            public string ContactContains { get; set; }
            protected override IEnumerable<string> OnApply(Phone source)
            {
                Thread.Sleep(1100);
                var results = source.Contacts.Where(c => c.Contains(ContactContains)).OrderBy(c => c);
                Count = results.Count();
                return results.Skip(0).Take(3);
            }
        }

        [Test]
        public void CanQueryWithStatistics()
        {
            var myPhone = new Phone();
            int? count = null;
            TimeSpan? timetaken = null;
            IEnumerable<string> names;
            using(var container = new QueriteContainer())
            {
                names = container.Query(myPhone)
                            .Statistics(x => count = x.Count)
                            .Statistics(x => timetaken = x.ExecutionSpan)
                            .Execute(new MatchingNamesQuery { ContactContains = "n"}).ToList();
            }
            
            Assert.That(names, Has.Count.EqualTo(3));
            Assert.That(timetaken.Value.TotalSeconds, Is.GreaterThan(1));
            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void CanQueryWithCustomizations()
        {
            var myPhone = new Phone();
            TimeSpan? timetaken = null;

            using(var container = new QueriteContainer())
            {
                var matchinesNamesQuery = new MatchingNamesQuery { ContactContains = "n"};
                var query = container.Query(myPhone);

                query.Customize(x => x.CacheItFor = 3.Minutes())
                     .Statistics(x => timetaken = x.ExecutionSpan)
                     .Execute(matchinesNamesQuery);    

                query.Customize(x => x.CanBeStaleSince = 3.MinutesFrom(DateTime.Now))
                     .Statistics(x => timetaken = x.ExecutionSpan)
                     .Execute(matchinesNamesQuery);    
            }
            
            Assert.That(timetaken.Value.TotalSeconds, Is.LessThan(1));
        }
    }
}