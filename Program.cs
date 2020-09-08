using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PdfMerger
{
    class Program
    {
        static string inputFolder;
        static string inputFilePrefix;
        static string maxInputFileNumberTxt;
        static string outputFilePathNname;

        static void Main(string[] args)
        {
            inputFolder = ConfigurationManager.AppSettings["inputFolderName"];
            inputFilePrefix = ConfigurationManager.AppSettings["inputFilePrefix"];
            maxInputFileNumberTxt = ConfigurationManager.AppSettings["maxInputFileNumber"];
            outputFilePathNname = ConfigurationManager.AppSettings["outputFilePathNname"];
            int maxInputFileNumber = 1;
            int.TryParse(maxInputFileNumberTxt, out maxInputFileNumber);

            TraverseInputFolder();
        }

        static void TraverseInputFolder()
        {
            //if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            DirectoryInfo dirInfo = new DirectoryInfo(inputFolder);
            var inputFileNames = dirInfo.GetFiles().Select(fileInfo => fileInfo.FullName).ToArray();
            const char separator = '_';
            string inputFilePrefix = null;
            var fileNumbers = inputFileNames.Select(fileName =>
            {
                var sepPos = fileName.LastIndexOf(separator);
                if (sepPos > -1)
                {
                    inputFilePrefix = fileName.Substring(0, sepPos);
                    var extnPos = fileName.LastIndexOf('.');
                    return int.Parse(fileName.Substring(sepPos + 1, extnPos - sepPos - 1));
                }
                else
                    return -1;
            });
            Array.Sort(fileNumbers.Where(num => num != -1).ToArray());
            inputFileNames = fileNumbers.Select(num => inputFilePrefix + separator + num + ".pdf").ToArray();
            if (inputFileNames.Length > 1)
                SimplePdfOps.CombineMultiplePDFs(inputFileNames, outputFilePathNname);
        }
    }
}
