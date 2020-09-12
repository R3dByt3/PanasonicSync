using NUnit.Framework;
using System.IO;

namespace PanasonicSync.GUI.Extensions.Tests
{
    [TestFixture()]
    public class FileInfoExtensionsTests
    {
        double _percentage;

        [Test()]
        public void CopyToTest()
        {
            FileInfo fileInfo1 = new FileInfo(@"D:\Coding\PanasonicSync\PanasonicSync.GUITests\bin\Debug\test.mp4");
            FileInfo fileInfo2 = new FileInfo(@"D:\Coding\PanasonicSync\PanasonicSync.GUITests\bin\Debug\test2.mp4");

            fileInfo1.CopyTo(fileInfo2, Callback);
            Assert.AreEqual(100, _percentage);
        }

        private void Callback(double obj)
        {
            _percentage = obj;
        }

        private void Callback(int obj)
        {
            _percentage = obj;
        }
    }
}