using System;
using System.Collections.Generic;
using System.Text;

namespace CLex
{
    internal class Return : Exception
    {
        public object Value { get; }

        public Return(object value)
        {
            this.Value = value;
        }
    }
}
