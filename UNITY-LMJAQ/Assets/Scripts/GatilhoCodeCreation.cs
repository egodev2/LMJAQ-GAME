using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GatilhoCodeCreation
{

    public static void WriteCSharpFile(StreamWriter sw, string className)
    {
        WritePackages(sw, className);
        WriteClass(sw, className);
    }

    private static void WritePackages(StreamWriter sw, string className)
    {
        // References
        sw.WriteLine("using System.Collections;");
        sw.WriteLine("using System.Collections.Generic;");
        sw.WriteLine("using UnityEngine;");

        // Space
        sw.WriteLine("");
    }
    private static void WriteClass(StreamWriter sw, string className)
    {
        sw.WriteLine("public class " + className +  " : MonoBehaviour");
        sw.WriteLine("{");
        WriteMethods(sw,className);
        sw.WriteLine("}");
    }

    private static void WriteMethods(StreamWriter sw, string className)
    {
        sw.WriteLine("      public void " + className + "method()"); // 2 TABS
        sw.WriteLine("      {");
        sw.WriteLine("              // PLACEHOLDER"); // 4 TABS
        sw.WriteLine("      }");
    }
}
