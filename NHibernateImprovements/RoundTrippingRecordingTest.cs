using System.Transactions;
using NHibernate.ByteCode.LinFu;
using NUnit.Framework;
using Void.Data.ORM.NHibernate;

namespace NHibernateImprovements
{
    [TestFixture]
    public class RoundTrippingRecordingTest
    {
        static RoundTrippingRecordingTest() { InMemoryNHibernatePersistanceSession<ProxyFactoryFactory>.RegisterAssembly(typeof(Recording).Assembly); }

        [Test]
        public void ShouldSaveAndReloadWithIdenticalValues()
        {
            var expected = new Recording(PointInTime.FromMicroseconds(10), PointInTime.FromMicroseconds(20));

            using (var repo = new ProjectRepository())
            {
                using (var transaction = new TransactionScope())
                {
                    repo.Recordings.SaveOrUpdate(expected);
                    transaction.Complete();
                }

                repo.Session.Clear();

                Recording actual;
                using (var transaction = new TransactionScope())
                {
                    actual = repo.Recordings.TryGet(expected.Id);
                }
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Start, Is.EqualTo(expected.Start));
                Assert.That(actual.End, Is.EqualTo(expected.End));
            }
        }
    }
}