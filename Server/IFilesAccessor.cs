using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabber.Server
{
    public interface IFilesAccessor
    {
        void AddLineToFile(string line);
        byte[] GetBytes();
    }
}
