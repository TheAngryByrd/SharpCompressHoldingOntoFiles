using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using SharpCompress.Archive;
using SharpCompress.Archive.Tar;
using SharpCompress.Common;

namespace SharpCompressHoldingOntoFiles
{
    [TestClass]
    public class UnitTest1
    {
        public string ThisLocation
        {
            get { return Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString(); }
        }

        public string TheItems
        {
            get
            {
                return Path.Combine(ThisLocation, "Items");
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var temp = Path.Combine(ThisLocation, Path.GetRandomFileName());
            CopyFiles(TheItems, temp);

            var newTarball = Path.Combine(ThisLocation, "MyTarBall.tgz");
            using (var archive = TarArchive.Create())
            {
                archive.AddAllFromDirectory(temp);
                archive.SaveTo(newTarball, CompressionType.GZip);
            }

            //An exception of type 'System.IO.IOException' occurred in mscorlib.dll but was not handled in user code
            //Additional information: The process cannot access the file 'New Text Document.txt' because it is being 
            //used by another process.
            Directory.Delete(temp, true);
            
        }

        private void CopyFiles(string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath));
        }
    }
}
