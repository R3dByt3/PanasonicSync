namespace NetStandard.IO.Compression
{
    public interface ICompressor
    {
        byte[] Compress<ObjectType>(ObjectType ObjToCompress);
        string DeCompress(byte[] compressed);
        ObjectType DeCompress<ObjectType>(byte[] compressed);
        void ExtractGZIP(string gzArchiveName, string destFolder);
        void ExtractZipFile(string archiveFilenameIn, string outFolder);
        void ExtractZipFile(string archiveFilenameIn, string outFolder, string password);
        void PackGZIP(string sourceFolder, string destFile);
        void CompressFolder(string folderName, string outPathname, string password = null);
        void CompressFile(string filename, string outPathname, string password = null);
    }
}