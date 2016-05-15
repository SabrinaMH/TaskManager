using System;

namespace TaskManager.Domain.Common
{
    public class Identity
    {
        public string Value { get; protected set; }

        public static implicit operator string(Identity id)
        {
            return id.Value;
        }
    }
}