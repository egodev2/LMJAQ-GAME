using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ModelMenuTool : EditorWindow
{

    public Scene CurrrentOpenScene;
    public List<Scene> SceneList;

    public GameObject EmptyPrefab;

    // private string sceneFolderPath = "Assets/Scenes/" ;



    private string modelPath;
    private string modelScriptPath;

    private ModelToUnity modelToUnity;
    private ModelManagement modelManagement;

    private static GameObject currentCanvas;

    private string lastCommonDirectoryForGameScripts = "Game"; // From eclipse


    // EditorWindow modelMenuTool;

    [MenuItem("Window/ModelMenuTool")]
    public static void ShowWindow()
    {
         EditorWindow.GetWindow(typeof(ModelMenuTool));
    }

    void OnGUI()
    {
        // The actual window code goes here
        if (GUILayout.Button("Set path of Model folder"))
        {
            // modelPath = EditorUtility.OpenFolderPanel("Model folder", "", "");
            modelPath = "F:\\eclispe-workspace\\runtime-New_configuration\\novoexemplo\\src-gen\\models";
            Debug.Log(modelPath);
        }
        if(GUILayout.Button("Set path for Scripts folder"))
        {
            // scriptPath = EditorUtility.OpenFolderPanel("Model folder", "", "");
            modelScriptPath = "F:\\eclispe-workspace\\runtime-New_configuration\\novoexemplo\\src-gen\\Scripts";
            Debug.Log(modelScriptPath);
        }
        if(GUILayout.Button("Load CHARP game files"))
        {
            // Get current Unity scripts path
            string unityScriptsFolderPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Scripts" + Path.AltDirectorySeparatorChar;

            // Create Folder for modelscript in Unity
            string modelScritpsPath = unityScriptsFolderPath + "ModelScripts" + Path.AltDirectorySeparatorChar;
            Directory.CreateDirectory(modelScritpsPath);

            // Dump all CSharp scripts created from folder
            //  string scriptsPath = EditorUtility.OpenFolderPanel("CSharpScripts folder", "", "");

            string[] folders = Directory.GetDirectories(modelScriptPath + Path.AltDirectorySeparatorChar + lastCommonDirectoryForGameScripts);

            char[] separator = { '/', '\\' };

            Debug.Log("Path : " + modelScriptPath);

            foreach(string folder in folders)
            {
                Debug.Log("PASTA: " + folder);
                string[] filesFromFolder = Directory.GetFiles(folder);
                foreach (string file in filesFromFolder)
                {
                    // Split each part of the path for dir and the file
                    string[] substrings = file.Split(separator);
                    Debug.Log("Arquivo ->" + file);

                    string endPathForScripts = ""; 

                    // Get all Different directories
                    for (int i = substrings.Length-2; i > 0; i--)
                    {
                        if (substrings[i].Equals(lastCommonDirectoryForGameScripts))
                        {
                            break;
                        }
                        endPathForScripts += substrings[i] + "\\";
                    }
                    //Arquivo ->F:\eclispe-workspace\runtime-New_configuration\novoexemplo\src-gen\Scripts/Game\Comportamentos\AoClicar.cs
                    // DESTPATH F:/Projetos/Unity/PackageTest/PackageTest/Assets/Scripts/ModelScripts/Comportamentos\AoClicar.cs
                    //          F:\Projetos\Unity\PackageTest\PackageTest\Assets\Scripts\ModelScripts\Comportamentos\aa.cs

                    // Create Directory and copy File
                    Directory.CreateDirectory(modelScritpsPath + endPathForScripts);

                    // Add File at the end
                    endPathForScripts += substrings[substrings.Length - 1];

                    //Debug.Log("filepath" + file);
                    Debug.Log("endPathForScripts" + endPathForScripts);

                    string destpath = Path.Combine(modelScritpsPath,endPathForScripts);
                    //destpath = destpath.Replace('/', '\\');
                    string sourceFile = file.Replace('/', '\\');
                 
                    Debug.Log("DESTPATH: " + destpath);
                    Debug.Log("filepath" + sourceFile);


                    File.Copy(sourceFile, destpath, true);
                }
            }
        }
        else if (GUILayout.Button("Load DataObject CSHARP"))
        {

            string scriptsFolderPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Scripts" + Path.AltDirectorySeparatorChar;

            //Create Directory
            string modelScriptPath = scriptsFolderPath + "ModelData" + Path.AltDirectorySeparatorChar;
            Directory.CreateDirectory(modelScriptPath);


            modelPath = EditorUtility.OpenFolderPanel("Model folder", "", "");
            Debug.Log("scriptFolderPath: " + scriptsFolderPath);
            Debug.Log("mdoelPath: " + modelPath);

            string[] files = Directory.GetFiles(modelPath);
            char[] separator = { '/', '\\'};

            foreach (string file in files)
            {
                string[] substrings = file.Split(separator);


                Debug.Log("Arquivo ->" + file);
                File.Copy(file,modelScriptPath + substrings[substrings.Length-1],true);
            }

            files = Directory.GetFiles(modelScriptPath);

            // List<ModelData> modelDataList = new List<ModelData>();

            Debug.Log(modelPath);
        }
        else if (GUILayout.Button("Carregar Modelo por Arquivo"))
        {
            string path = EditorUtility.OpenFilePanel("Load JSON File", "", "");

            if (path.Length != 0)
            {

                Debug.Log(path);
            }
        }
        else if(GUILayout.Button("Load UNITY GAME from C#DATA"))
        {
            string[] folders = Directory.GetDirectories(modelPath);

            // Hashes for Data information
            modelToUnity = new ModelToUnity();
            modelToUnity.InitializeHash();


            modelManagement = new ModelManagement(modelToUnity);

            // Load Data in each file of each folder
            foreach (string folder in folders)
            {

                Debug.Log("Folder: " + folder);
                string[] files = Directory.GetFiles(folder);

                foreach (string file in files)
                {
                    // Store Model data
                    string modelInJSON = File.ReadAllText(file);

                    // Create a ModeDataObject with desrialization
                    ModelData modelDataObject = JsonConvert.DeserializeObject<ModelData>(modelInJSON);

                    Debug.Log("name after deserialization: " + modelDataObject.Class.Name);

                    // Hash: Add ClassName (string) -> modelData (ModelData)
                    modelToUnity.AddModelDataToHash(modelDataObject.Class.Name, CreateNewModelData(modelInJSON));

                    // Hash: Add ClassName (string -> Type (String)
                    modelToUnity.AddTypeToHash(modelDataObject.Class.Name, modelDataObject.Class.Type);
                }
            }


            foreach(string Key in modelToUnity.NameReferencesToModelData.Keys)
            {
                Debug.Log("KeyBB : " + Key);
            }

            // Create All objects from ModelData
            modelManagement.BuildGameFromModels(modelToUnity);

            // Create File to load dictioanry later
            modelManagement.BuildReferenceTypeDictionaryFile();
        }
        else if(GUILayout.Button("ResourceLoadTest"))
        {
            EmptyPrefab = Instantiate(Resources.Load("DefaultButton", typeof(GameObject))) as GameObject;
        }
    }

    private ModelData CreateNewModelData(string model)
    {
        return JsonConvert.DeserializeObject<ModelData>(model);
    }
}

