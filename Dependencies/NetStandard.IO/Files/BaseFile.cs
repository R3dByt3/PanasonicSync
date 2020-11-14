using System;
using System.IO;

namespace NetStandard.IO.Files
{
    public class BaseFile : IBaseFile
    {
        private byte[] _content;

        public string Path { get; set; }
        public string Name { get; set; }
        public string NameWithExtension { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public long Size { get; set; }
        public MemoryStream ContentStream { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastEdited { get; set; }

        public BaseFile()
        {

        }

        public byte[] GetContent()
        {
            return _content;
        }

        public void SetContent(byte[] value)
        {
            _content = value;
        }

        public BaseFile(string pathToFile)
        {
            Load(pathToFile);
        }

        public void LoadByteContent()
        {
            SetContent(File.ReadAllBytes(FullPath));
        }

        public void LoadContentStream()
        {
            using (FileStream file = new FileStream("file.bin", FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ContentStream.Write(bytes, 0, (int)file.Length);
            }
        }

        public virtual void Load(string pathToFile)
        {
            FullPath = pathToFile;
            Path = System.IO.Path.GetDirectoryName(pathToFile);
            Name = System.IO.Path.GetFileNameWithoutExtension(pathToFile);
            NameWithExtension = System.IO.Path.GetFileName(pathToFile);
            Extension = System.IO.Path.GetExtension(pathToFile);
            Size = new FileInfo(pathToFile).Length;
            CreationDate = File.GetCreationTime(pathToFile);
            LastEdited = File.GetLastAccessTime(pathToFile);
        }
    }
}
