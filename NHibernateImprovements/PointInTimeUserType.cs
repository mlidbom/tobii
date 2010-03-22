using System;
using NHibernate;

namespace NHibernateImprovements
{
    public class PointInTimeUserType : ImmutableSingleColumnUserType<PointInTime, Int64>
    {
        public PointInTimeUserType() : base(NHibernateUtil.Int64)
        {}

        protected override long PersistedValueOf(PointInTime value) { return value.TotalMicroseconds; }
        protected override PointInTime DominValueOf(long getValue) { return PointInTime.FromMicroseconds(getValue); }
    }
}