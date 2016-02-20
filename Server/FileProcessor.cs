using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("FunctionalTests")]

namespace Jabber.Server
{

    class FileProcessor:IFilesAccessor
    {
        
        static string filePath;
        object stringsLocker = new object();

        static FileProcessor instance;
        public static FileProcessor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileProcessor();
                }
                return instance;
            }
        }

        private FileProcessor()
        {
            try
            {
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vaganov", DateTime.Now.ToLocalTime().ToString("dd_MM_yy-HH_mm_mm"));
                Directory.CreateDirectory(folderPath);
                filePath = Path.Combine(folderPath, "log.txt");
                File.WriteAllText(filePath, "");
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddLineToFile(string line)
        {    
            lock(stringsLocker)
            {
                try
                {
                    List<string> sortedStrings = File.ReadAllLines(filePath).ToList();
                    line = line.Replace(Environment.NewLine, "");
                    sortedStrings.Add(line);
                    sortedStrings.Sort();
                    File.WriteAllLines(filePath, sortedStrings);
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public byte[] GetBytes()
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return bytes;
        }
    }
}
