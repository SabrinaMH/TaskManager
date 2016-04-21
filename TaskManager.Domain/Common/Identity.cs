using System;

namespace TaskManager.Domain.Common
{
    public class Identity
    {
        public Guid Value { get; protected set; }

        public static implicit operator string(Identity id)
        {
            return id.Value.ToString();
        }

        public static implicit operator Guid(Identity id)
        {
            return id.Value;
        }
    }
}