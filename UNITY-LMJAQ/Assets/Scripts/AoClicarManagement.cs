using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AoClicarManagement : IManagement
{

    string path;
    string fileName;

    FileStream fs;

    StreamWriter sr;

    public void InitializeWriter(string fileName)
    {
        this.fileName = fileName;

        path = Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "AoClicarRun" + Path.AltDirectorySeparatorChar, fileName + ".txt");
        Directory.CreateDirectory(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "AoClicarRun");

        fs = new FileStream(path, FileMode.OpenOrCreate);
        sr = new StreamWriter(fs);
    }
    public void WriteLineOfFile(string content)
    {
        sr.WriteLine(content);
    }
    public void CloseWriter()
    {
        sr.Close();
        fs.Close();
    }
}
