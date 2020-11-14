using NetStandard.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace NetStandard.IO.Files
{
    public class FileManager : IFileManager
    {
        private readonly ILogger _logger;

        public FileManager(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateFileLogger();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public void WriteByteArrayToFile(Byte[] objectToWrite, string filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            try
            {
                _logger.Debug("Start writing file \"" + filename + "\"");
                File.WriteAllBytes(filename, objectToWrite);
                _logger.Debug("Successfull written file \"" + filename + "\""); //ToDo: Checken
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Saving file failed; Path = " + filename);
            }
        }

        public byte[] ReadByteArrayFromFile(string filename)
        {
            try
            {
                byte[] returnValue;
                _logger.Debug("Start reading file \"" + filename + "\"");
                returnValue = File.ReadAllBytes(filename);
                _logger.Debug("Successfull read file \"" + filename + "\""); //ToDo: Checken
                return returnValue;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Reading file failed; Path = " + filename);
                throw new FileNotFoundException();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public void WriteObjectToJSON<ObjectType>(ObjectType @object, string filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            try
            {
                _logger.Debug("Start writing file \"" + filename + "\"");
                File.WriteAllText(filename, JsonConvert.SerializeObject(@object));
                _logger.Debug("Successfull written file \"" + filename + "\""); //ToDo: Checken
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Saving file failed; Path = " + filename);
            }
        }

        public ObjectType ReadObjectFromJSON<ObjectType>(string filename)
        {
            try
            {
                ObjectType returnValue;
                _logger.Debug("Start reading file \"" + filename + "\"");
                returnValue = JsonConvert.DeserializeObject<ObjectType>(System.IO.File.ReadAllText(filename));
                _logger.Debug("Successfull read file \"" + filename + "\""); //ToDo: Checken
                return returnValue;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Reading file failed; Path = " + filename);
                throw new FileNotFoundException();
            }
        }

        public List<string> GetAllFilesInFolder(string rootFolder, string offType = "", bool recursivly = false)
        {
            if (!Directory.Exists(rootFolder)) throw new DirectoryNotFoundException();
            List<string> returnValue = new List<string>();
            returnValue.AddRange(Directory.GetFiles(rootFolder));
            if (recursivly)
            {
                foreach (string subFolder in Directory.GetDirectories(rootFolder))
                {
                    returnValue.AddRange(GetAllFilesInFolder(subFolder, offType, recursivly));
                }
            }
            if (!string.IsNullOrEmpty(offType)) returnValue.RemoveAll(type => !type.EndsWith(offType));
            return returnValue;
        }


        public byte[] CreateChecksum(string filePath)
        {
            using (var hashAlgorithm = new SHA512Managed())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024 * 1024))
                {
                    fileStream.Position = 0;
                    long bytesToHash = 1024 * 1024;

                    var buf = new byte[4 * 1024];
                    while (bytesToHash > 0)
                    {
                        var bytesRead = fileStream.Read(buf, 0, (int)Math.Min(bytesToHash, buf.Length));
                        hashAlgorithm.TransformBlock(buf, 0, bytesRead, null, 0);
                        bytesToHash -= bytesRead;
                        if (bytesRead == 0)
                            throw new InvalidOperationException("Unexpected end of stream");
                    }
                    hashAlgorithm.TransformFinalBlock(buf, 0, 0);
                    var hash = hashAlgorithm.Hash;
                    return hash;
                }
            }
        }
    }
}
