using conversionTest;
using System.IO;
using System.Linq;

// Asset location
string assetFolder = @"C:\Drive\test\assets\";
string convertedFolder = @"C:\Drive\test\assets\conv\";

string[] ifcFiles = Directory.GetFiles(assetFolder, "*.ifc");

foreach (string ifcFile in ifcFiles)
{
    
    string glbfileName = ifcFile[(ifcFile.LastIndexOf('\\') + 1)..ifcFile.LastIndexOf('.')];


    for (int i = 1; i < 7; i++)
    {
        //for (int j = 1; j < 20; j++)
        {
            double deflectionAngle = 100;
            double deflectionTolerance = i * 0.5;
            string saveFileName = $"{convertedFolder}{glbfileName}-da{deflectionAngle}-dt{deflectionTolerance}.glb";
            using (Stream fileStream = new FileStream(ifcFile, FileMode.Open, FileAccess.Read))
            {
                byte[] convertedFile = Ifc2GlbConverterBase.ConvertToGlb(fileStream, deflectionAngle, deflectionTolerance);
                using var convertedFileStream = new FileStream(saveFileName, FileMode.Create);
                convertedFileStream?.Write(convertedFile);
            }
            Console.WriteLine(saveFileName + " saved");
        }
    }    
}

