using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AdventureControl : MonoBehaviour
{
    // Start is called before the first frame update

    // Example: STA000

    // STRING (ID) -> AventureSTATE 

    public Dictionary<string, AdventureState> StatesDictionary { get; set; }

    public AdventureState CurrentAdventureState { get; set; }

    public Inventory inventoryReference { get; set; }

    List<string> codes;

    Dictionary<string, ModelData> modelDataTemp;

    private enum ReadStates
    {
        First = 1,
        Second = 2,
        Third = 3,
    }

    void Awake()
    {
        // Correct
        RuntimeInitForAdventureController();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RuntimeInitForAdventureController()
    {
        StatesDictionary = new Dictionary<string, AdventureState>();
        codes = new List<string>();
        InitializeAdventureControl();
    }

    // TOMORROW SUNDAY HEEEEEE
    private void InitializeAdventureControl()
    {
        StatesDictionary = BuildStatesDictionary();

        string[][] proximoStatesbyCode = GetProximoData();

        foreach(string keyDict in StatesDictionary.Keys)
        {
            Debug.Log("key: " + keyDict);
        }


        Debug.Log("OH MY GOD???????");
        // proximoStatesByCode
        for(int i = 0; i < proximoStatesbyCode.GetLength(0); i++)
        {
            // Get State reference
            string code = codes[i];
            Debug.Log("M-CODEANAL: " + code);
            AdventureState currentAdventureState = StatesDictionary[code];

            for (int j = 0; j < proximoStatesbyCode[i].Length; j++) 
            {
                Debug.Log("M-proximoStatesbyCode[[ " + i + " ] [ " + j + " ] " + ":" + proximoStatesbyCode[i][j]);

                // Build Object
                ProximoData proximoData = new ProximoData();
                proximoData.AddConteudo(modelDataTemp, proximoStatesbyCode[i][j]);
                proximoData.AdventureStateReference = StatesDictionary[proximoStatesbyCode[i][j]];

                // Add State Dto ProxiDictionary
                currentAdventureState.PossibleTargetStates.Add(proximoStatesbyCode[i][j], proximoData);
            }
        }
        Debug.Log(">---------------END--------------<");



        codes = null;
    }

    private AdventureState CreateNewAdventureState(ModelData modelDataObject, string codeEntry)
    {

        bool estadoInicialData = false;
        bool estadoFinalData = false;

        //
        string estadoName = codeEntry;
       //  Debug.Log("estado name" + estadoName);


        if (modelDataObject.Class.Properties.PropertiesKeys.Contains("EstadoInicial"))
        {
            estadoInicialData = true;
        }
        else if (modelDataObject.Class.Properties.PropertiesKeys.Contains("EstadoFinal"))
        {
            estadoFinalData = true;
        }

        return new AdventureState(estadoName, estadoInicialData, estadoFinalData);
    }

    public string GetFullCodeEquivalent(string code)
    {

        if (code.Length < 4)
        {
            return code[0].ToString() + code[1].ToString() + code[2].ToString() + "000";
        }
        else if (code.Length < 5)
        {
            return code[0].ToString() + code[1].ToString() + code[2].ToString() + "00" + code[3].ToString();
        }
        else if (code.Length < 6)
        {
            return code[0].ToString() + code[1].ToString() + code[2].ToString() + "0" + code[3].ToString() + code[4].ToString();
        }

        return (code[0].ToString() + code[1].ToString() + code[2].ToString() + code[3].ToString() + code[4].ToString() + code[5].ToString());
    }

    private Dictionary<string, AdventureState> BuildStatesDictionary()
    {
        // FOr getting model data
        modelDataTemp = new Dictionary<string, ModelData>();

        // ACtual dictionary to be returned
        Dictionary<string, AdventureState> codeToStateDictionary = new Dictionary<string, AdventureState>();

        string folderStatePath = Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "AdventureStates";

        string[] allFiles = Directory.GetFiles(folderStatePath);
        List<string> files = new List<string>();

        // Unity generate meta files: need to filet.
        for (int i = 0; i < allFiles.Length; i = i + 2)
        {
            files.Add(allFiles[i]);
        }

        // Get ModelData laoded for each state in temporary dictionary.
        foreach (string filePath in files)
        {
            string modelInJSON = File.ReadAllText(filePath);
            ModelData modelData = JsonConvert.DeserializeObject<ModelData>(modelInJSON);
            modelDataTemp.Add(GetFullCodeEquivalent(modelData.Class.Name), modelData);
        }

        // BuildStates - tobe acessed for entire game.
        foreach (string modelDataEntry in modelDataTemp.Keys)
        {
            AdventureState adventureState = CreateNewAdventureState(modelDataTemp[modelDataEntry], modelDataEntry);
            //Debug.Log("BUILDSTATE-MODELDATAENTRY: " + modelDataEntry);
            // Debug.Log("BUILDSTATE-adventureState: " + adventureState.GetFullCode());
            codeToStateDictionary.Add(modelDataEntry, adventureState);
        }

        return codeToStateDictionary;
    }

    public string[][] GetProximoData()
    {

        // Dictionary<String, List<String>> dict = new Dictionary<string, List<string>>();
        // string proximoestadoRelativeFolderPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "DefaultButton";
        string proximoestadoRelativeFolderPath = "ProximoStates" + Path.AltDirectorySeparatorChar;

        List<string> files = GetProximoEstatesPaths();
        int numberOfFiles = files.Count;

        string[][] statesCodeArray = new string[numberOfFiles][];
        int cnt = 0;

        foreach (string filePath in files)
        {
            Debug.Log("FilePath: " + filePath);

            // Get Code
            char[] separators = new char[] { '.', '\\', '/' };
            string [] pathSplit = filePath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string stateCode = pathSplit[pathSplit.Length - 2];

            codes.Add(stateCode);

            // Get ProximoStates
            TextAsset textAsset = Resources.Load(proximoestadoRelativeFolderPath + stateCode) as TextAsset;
            string textValue = textAsset.text.Trim();
            //string textValue = textAsset.text;

            // Store them in the 2D-array
            statesCodeArray[cnt] = (textValue.Split('-'));

            foreach(string x in statesCodeArray[cnt])
            {
                Debug.Log("COL: " + x);
            }

            cnt++;
        }

        return statesCodeArray;
    }

    private List<String> GetProximoEstatesPaths()
    {
        string proximoEstadosPath = Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar, "ProximoStates") + Path.AltDirectorySeparatorChar;
        proximoEstadosPath.Replace('/', '\\');

        string[] allFiles = Directory.GetFiles(proximoEstadosPath);
        List<string> files = new List<string>();

        // Unity generate meta files: need to filet.
        for (int i = 0; i < allFiles.Length; i = i + 2)
        {
            Debug.Log("pathToBeIncldued: " + allFiles[i]);
            files.Add(allFiles[i]);
        }

        return files;
    }

    public Dictionary<String, List<String>> ab()
    {
        return null;
    }
}
