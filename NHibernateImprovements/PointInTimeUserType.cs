using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace NHibernateImprovements
{
    public abstract class ImmutableSingleColumnUserType<TDomain, TPersisted> : IUserType
    {
        private readonly NullableType _type;
        protected ImmutableSingleColumnUserType(NullableType type) { _type = type; }

        public new bool Equals(object x, object y)
        {
            if(ReferenceEquals(x, y))
            {
                return true;
            }

            if(x == null  || y == null )
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x) { return ((PointInTime)x).GetHashCode(); }

        public object DeepCopy(object value) { return value; }
        public object Replace(object original, object target, object owner) { return original; }
        public object Assemble(object cached, object owner) { return cached; }
        public object Disassemble(object value) { return value; }
        public SqlType[] SqlTypes { get { return new[]{ _type.SqlType}; } }
        public Type ReturnedType { get { return typeof(TDomain); } }
        public bool IsMutable { get { return false; } }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {            
            if(value == null)
            {
                _type.Set(cmd, DBNull.Value, index);
            }
            _type.Set(cmd, PersistedValueOf((TDomain) value), index);
        }        


        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var value = _type.NullSafeGet(rs, names[0]);
            if(value == null)
            {
                return null;
            }
            return DominValueOf((TPersisted)value);
        }

        protected abstract TPersisted PersistedValueOf(TDomain value);
        protected abstract TDomain DominValueOf(TPersisted getValue);
    }

    public class PointInTimeUserType : ImmutableSingleColumnUserType<PointInTime, Int64>
    {
        public PointInTimeUserType() : base(NHibernateUtil.Int64)
        {}

        protected override long PersistedValueOf(PointInTime value) { return value.TotalMicroseconds; }
        protected override PointInTime DominValueOf(long getValue) { return PointInTime.FromMicroseconds(getValue); }
    }
}