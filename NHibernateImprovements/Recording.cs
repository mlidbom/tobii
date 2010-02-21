using System;
using System.Collections.Generic;
using Void;

namespace NHibernateImprovements
{
    public class Recording : PersistentEntity<Recording>
    {
        //for nhibernate
        protected Recording(){}
        public Recording(PointInTime start, PointInTime end)
        {
            Start = start;
            End = end;
        }
        public virtual PointInTime Start { get; private set; }
        public virtual PointInTime End { get; private set; }
        //public ISet<Fixation> Fixations { get; private set; }    
    }
}