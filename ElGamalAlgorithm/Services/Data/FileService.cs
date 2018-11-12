using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.Services.Data
{
    public class FileService : IFileService
    {
        public void SaveToFile(byte[] bytesToSave, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(bytesToSave);
                }
            }
        }
    }
}
