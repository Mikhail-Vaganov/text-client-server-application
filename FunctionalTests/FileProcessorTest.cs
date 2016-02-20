using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Jabber.Server;
using System.Text;
namespace FunctionalTEsts
{
    [TestClass]
    public class FileProcessorTest
    {
        [TestMethod]
        public void TestSortingLinesInFLigFile()
        {
            string[] randomStrings = new string[] { "Zerg", "ACC", "привет", "ABC", "111"};
            string[] sortedStrings = new string[] { "111", "ABC", "ACC", "Zerg", "привет"};

            FileProcessor fileProcessor = FileProcessor.Instance;
            foreach (string str in randomStrings)
                fileProcessor.AddLineToFile(str);

            byte[] bytes = fileProcessor.GetBytes();
            string[] stringsFromFile = (Encoding.UTF8.GetString(bytes)).Replace(Environment.NewLine,",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(sortedStrings.Length, stringsFromFile.Length);
            for(int i=0;i<stringsFromFile.Length;i++)
                Assert.AreEqual(stringsFromFile[i], sortedStrings[i]);                
        }
    }
}
