using System;
using System.IO;

namespace NetStandard.IO.Files
{
    public interface IBaseFile
    {
        MemoryStream ContentStream { get; set; }
        DateTime CreationDate { get; set; }
        string Extension { get; set; }
        string FullPath { get; set; }
        DateTime LastEdited { get; set; }
        string Name { get; set; }
        string NameWithExtension { get; set; }
        string Path { get; set; }
        long Size { get; set; }

        byte[] GetContent();
        void SetContent(byte[] value);
        void LoadByteContent();
        void LoadContentStream();
        void Load(string pathToFile);
    }
}