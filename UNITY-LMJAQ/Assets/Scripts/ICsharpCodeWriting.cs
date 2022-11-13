using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ICsharpCodeWriting 
{
    public void WritePackages(StreamWriter sw);
    public void WriteClass(StreamWriter sw);
}
