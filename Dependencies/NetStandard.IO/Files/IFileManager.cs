using System.Collections.Generic;

namespace NetStandard.IO.Files
{
    public interface IFileManager
    {
        byte[] CreateChecksum(string filePath);
        List<string> GetAllFilesInFolder(string rootFolder, string offType = "", bool recursivly = false);
        byte[] ReadByteArrayFromFile(string filename);
        ObjectType ReadObjectFromJSON<ObjectType>(string filename);
        void WriteByteArrayToFile(byte[] objectToWrite, string filename);
        void WriteObjectToJSON<ObjectType>(ObjectType @object, string filename);
    }
}