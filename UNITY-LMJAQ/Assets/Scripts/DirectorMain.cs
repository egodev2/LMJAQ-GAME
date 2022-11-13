using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DirectorMain : MonoBehaviour
{

    private GameObject AdventureController { get; set; }

    private Dictionary<GameObject, string> onClickCallbackReferences;

    private Dictionary<string, string> classReferenceToNameDictionary;

    int currentScore;

    string dataPath;

    private GameObject ac;
    // private GameObject advController;
    private delegate void AoClicarDelegate();
    private AoClicarDelegate m_AoClickListener;
    // Start is called before the first frame update
    private void Awake()
    {
        currentScore = 0;
        dataPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        DontDestroyOnLoad(this);
        ac = GameObject.Find("_AoClicar");
        DontDestroyOnLoad(ac);
        // advController = GameObject.Find("_AdventureControl");
        // advController = GameObject.Find("_AdventureControl");

        AdventureController = Instantiate(Resources.Load("_AdventureController", typeof(GameObject))) as GameObject;
        AdventureController.name = "_AdventureControl";
        DontDestroyOnLoad(AdventureController);

        onClickCallbackReferences = InitializeDictionary();
        classReferenceToNameDictionary = InitializeNameReferenceToTypeDicitonary();
        SceneManager.activeSceneChanged += sceneRuntimeInitialization;

        // Initialize Controller responsible for any ADventure Gameplay
    }

    void Start()
    {
        // GetProximoData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void sceneRuntimeInitialization(Scene current, Scene next)
    {

        Dictionary<string, Button> dictButtonAcess = new Dictionary<string, Button>();

        Debug.Log("HELLO!: " + next.name);
        // SET UP Buttons

        // Find Every object that has a button component.
        Button[] foundButtonObjects = FindObjectsOfType<Button>();

        //
        Type t = typeof(AoClicar);

        GameObject canvasObject = GameObject.Find("Canvas");
            
            // gameObject.transform.Find("Canvas").gameObject;

        Button[] buttonComponents = canvasObject.GetComponentsInChildren<Button>();

        foreach(Button button in buttonComponents)
        {
            dictButtonAcess.Add(button.gameObject.name, button);
        }


        string currentSceneName = next.name;

        // Get Scene file and read each object-methodReference (1 line each)
        string[] lines = File.ReadAllLines(Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "AoClicarRun" + Path.AltDirectorySeparatorChar, currentSceneName + ".txt"));

        foreach (string line in lines)
        {


            // Get Values from line "ObjectName"-"eventName"
            string[] paramsForObject = line.Split('-');
            string objectName = paramsForObject[0];
            string onClickReferenceName = paramsForObject[1];

            Debug.Log("objectName: " + objectName);
            Debug.Log("objectReferenceName: " + onClickReferenceName);
            Debug.Log("gameObject");

            System.Reflection.MethodInfo method = ac.GetComponent<AoClicar>().GetType().GetMethod(onClickReferenceName);

            //Add the listener;
            Action<string> param = ac.GetComponent<AoClicar>().Call;

            Button button = dictButtonAcess[objectName]; 

            // Lambda Exprressions
            button.onClick.AddListener(() => ac.GetComponent<AoClicar>().Call(onClickReferenceName));
        }



        // For each obejct that has buttonComponents
        /*
        foreach (Button button in buttonComponents)
        {
            // Get Button's Obejct reference
            GameObject gameObject = button.transform.gameObject;

            // Get Scene name
            string currentSceneName = next.name;
            
            // Get Scene file and read each object-methodReference (1 line each)
            string[] lines = File.ReadAllLines(Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "AoClicarRun" + Path.AltDirectorySeparatorChar, currentSceneName + ".txt"));

            foreach(string line in lines)
            {
                // Get Values from line "ObjectName"-"eventName"
                string[] paramsForObject = line.Split('-');
                string objectName = paramsForObject[0];
                string onClickReferenceName = paramsForObject[1];

                Debug.Log("objectName: " + objectName);
                Debug.Log("objectReferenceName: " + onClickReferenceName);
                Debug.Log("gameObject");

                System.Reflection.MethodInfo method = ac.GetComponent<AoClicar>().GetType().GetMethod(onClickReferenceName);

                //Add the listener;
                Action<string> param = ac.GetComponent<AoClicar>().Call;

                // Lambda Exprressions
                button.onClick.AddListener(() => ac.GetComponent<AoClicar>().Call(onClickReferenceName));
            }
        }
        */
        

        /*
        foreach (Button button in foundButtonObjects)
        {
            Debug.Log("HELLO!: " + button.gameObject.name);
            // Pass the object for dictionary to get method reference

            if (onClickCallbackReferences.ContainsKey(button.gameObject))
            {
                string gatilhoReference = onClickCallbackReferences[button.gameObject];
                Debug.Log("gatilhoReference: " + gatilhoReference);


                System.Reflection.MethodInfo method = ac.GetComponent<AoClicar>().GetType().GetMethod(gatilhoReference);
                Debug.Log("method: " + method.ToString());


                //Add the listener;
                Action<string> param = ac.GetComponent<AoClicar>().Call;

                // Lambda Exprressions
                button.onClick.AddListener(() => ac.GetComponent<AoClicar>().Call(gatilhoReference));
            }
        }
        */
    }



    public Dictionary<GameObject, string> InitializeDictionary()
    {
        string[] dentryLines = File.ReadAllLines(dataPath + "OnClick.txt");



        Dictionary<GameObject, string> dict = new Dictionary<GameObject, string>();

        foreach (string line in dentryLines)
        {
            string[] parameters = line.Split(',');
            Debug.Log("[0]? " + parameters[0] + "[1]?" + parameters);
            dict.Add(GameObject.Find(parameters[0]), parameters[1]);
        }

        return dict;
    }

    public void GetProximoData()
    {

        // Dictionary<String, List<String>> dict = new Dictionary<string, List<string>>();

        string proximoEstadosPath = Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar, "ProximoStates") + Path.AltDirectorySeparatorChar;
        proximoEstadosPath.Replace('/', '\\');

        string[] allFiles = Directory.GetFiles(proximoEstadosPath);
        List<string> files = new List<string>();

        // Unity generate meta files: need to filter.
        for (int i = 0; i < allFiles.Length; i = i + 2)
        {
            Debug.Log("pathToBeIncldued: " + allFiles[i]);
            files.Add(allFiles[i]);
        }

        foreach (string filePath in files)
        {
            Debug.Log("FilePath" + filePath);
            string[] fileParts = filePath.Split('\\');
            string filepath = fileParts[fileParts.Length - 1];

            // Code is stored at 0
            string[] fileCode = filePath.Split('.');

            // Create new list to store states of file
            List<string> proximoCodes = new List<string>();

            // For each state (line) read in files
            foreach (string line in File.ReadLines(filePath))
            {
                Debug.Log("LineContent: " + line);
                proximoCodes.Add(line);
            }
        }

        // dict.Add(fileCode[0], proximoCodes);

        /*
        foreach (string code in dict.Keys)
        {
            Debug.Log("|" + code + "|");
            List<string> proximoCodes = dict[code];
            foreach (string proximoCode in proximoCodes)
            {
                Debug.Log(proximoCode);
            }
        }
        */


        // return dict;
    }

    private Dictionary<string, string> InitializeNameReferenceToTypeDicitonary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        string dictPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar;
        dictPath.Replace('/','\\');
        Debug.Log("ARQUIVO: " + dictPath + "referenceToType");
        TextAsset textAsset = Resources.Load("referenceToType") as TextAsset;

        Debug.Log("text: " + textAsset);

        int r = 0;
        string key = "!";
        string value = "!";

        using (StringReader stringReader = new StringReader(textAsset.text))
        {
            while(true)
            {
                string line = stringReader.ReadLine();
                Debug.Log("line: " + line);
                if(line == null)
                {
                    break;
                }
                else
                {
                    if(r % 2 == 0)
                    {
                        key = line;
                    }
                    else
                    {
                        value = line;
                        dict.Add(key ,value);
                    }
                    r++;
                }
            }
        }

        foreach(string keytest in dict.Keys)
        {
            Debug.Log("Teste: " + keytest);
        }


        return dict;
    }


}

