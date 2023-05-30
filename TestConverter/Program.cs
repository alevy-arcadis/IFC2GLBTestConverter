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


    //for (int i = 1; i < 11; i++)
    {
        //for (int j = 1; j < 20; j++)
        {
            double deflectionAngle = 0.5; // or use i or j to vary
            double deflectionTolerance = 2; // or use i or j to vary
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

// get stats
// save to .csv
// column 1 - filename
// column 2 - deflection tolerance
// column 3 - file size
string[] convertedFiles = Directory.GetFiles(convertedFolder, "*.glb");
using (var statsFile = new StreamWriter(convertedFolder + "stats.csv"))
{
    foreach (string convertedFile in convertedFiles)
    {
        string glbfileName = convertedFile[(convertedFile.LastIndexOf('\\') + 1)..convertedFile.LastIndexOf('.')];
        FileInfo fi = new(convertedFile);
        long filesize = fi.Length;
        string deflectionTolerance = "1000";
        if (glbfileName.Contains("dt"))
        {
            deflectionTolerance = convertedFile[(convertedFile.LastIndexOf("dt") + 2)..convertedFile.LastIndexOf('.')];
        }
        statsFile.WriteLine(glbfileName + "," + deflectionTolerance + "," + filesize.ToString());
    }
}
