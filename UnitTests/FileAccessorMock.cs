using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jabber.Server;
using Jabber.ClientServer;

namespace UnitTests
{
    class FileAccessorMock : IFilesAccessor
    {
        public string LineAddedToFile;
        public byte[] BytesToReturn = new byte[] { 2, 3, 4, 5, 6 };

        public void AddLineToFile(string line)
        {
            LineAddedToFile=line;
        }

        public byte[] GetBytes()
        {
            return BytesToReturn;
        }
    }
}
