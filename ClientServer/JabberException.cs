using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabber.ClientServer
{
    [Serializable]
    public class JabberException : ApplicationException
    {
        public JabberException() { }
        public JabberException(string message) : base(message) { }
        public JabberException(string message, Exception inner) 
            : base($"{message}{Environment.NewLine}Reason:{Environment.NewLine}{inner.Message}", inner) { }

        protected JabberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
