using System;
using LinFu.DynamicProxy;
using NHibernate.ByteCode.LinFu;
using Void.Data.ORM;
using Void.Data.ORM.NHibernate;

namespace NHibernateImprovements
{
    public class ProjectRepository : IDisposable
    {
        public readonly INHibernatePersistanceSession Session;

        public ProjectRepository() : this(new InMemoryNHibernatePersistanceSession<ProxyFactoryFactory>()) { }

        public ProjectRepository(INHibernatePersistanceSession session)
        {
            this.Session = session;
            Recordings = new Repository<Recording, Guid>(session);
        }

        public Repository<Recording, Guid> Recordings { get; set; }
        
        public void Dispose() { Session.Dispose(); }
    }
}