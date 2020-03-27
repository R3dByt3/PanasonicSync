using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.Extensions
{
    public static class FileInfoExtensions
    {
        public static void CopyTo(this FileInfo file, FileInfo destination, Action<double> progressCallback)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            //bool cancelFlag = false;

            using (FileStream source = file.OpenRead())
            {
                long fileLength = source.Length;
                using (FileStream dest = destination.OpenWrite())
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double percentage = (double)totalBytes * 100.0 / fileLength;

                        dest.Write(buffer, 0, currentBlockSize);

                        //cancelFlag = false;
                        progressCallback(percentage/*, cancelFlag*/);

                        //if (cancelFlag == true)
                        //{
                        //    // Delete dest file here
                        //    break;
                        //}
                    }
                }
            }

        }
    }
}
