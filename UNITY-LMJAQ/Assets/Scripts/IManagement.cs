using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManagement
{
    public void InitializeWriter(string name);
    public void WriteLineOfFile(string content);
    public void CloseWriter();
}
