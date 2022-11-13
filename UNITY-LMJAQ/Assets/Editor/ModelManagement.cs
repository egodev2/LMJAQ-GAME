using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Text;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using Microsoft.CSharp;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor.Events;


public class ModelManagement : MonoBehaviour
{

    private GameObject currentCanvas;
    private static string sceneFolderPath = "Assets" + Path.AltDirectorySeparatorChar + "Scenes" + Path.AltDirectorySeparatorChar;
    private static string scriptsFolderPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Scripts" + Path.AltDirectorySeparatorChar;

    private string[] preDefinedUIList = { "Botão", "Imagem" };

    // private string buttonPrefabPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "DefaultButton.prefab";
    private string buttonPrefabPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "DefaultButton";
    private string imagePrefabPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "DefaultImage";
    private string directorPrefabPath = "Assets" + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar + "_DirectorController";

    private int gameWindowWidth;
    private int gameWindowHeight;

    private GameObject buttonPrefab;
    private GameObject imagePrefab;
    private ModelToUnity modelToUnity;
    private Dictionary<GameObject, string> onClickCallbackReferences;

    private ComponenteManagement componenteManagement;
    private GameObject directorObject;

    private delegate void AoClicarDelegate();
    private AoClicarDelegate m_AoClickListener;


    AoClicarManagement aoClicarManagement;

    public ModelManagement(ModelToUnity modelToUnity)
    {
        Debug.Log("path: " + buttonPrefabPath);
        buttonPrefab = Resources.Load<GameObject>(buttonPrefabPath);
        imagePrefab = Resources.Load<GameObject>(imagePrefabPath);
        this.modelToUnity = modelToUnity;
        onClickCallbackReferences = new Dictionary<GameObject, string>();
        componenteManagement = new ComponenteManagement();
    }

    public void SetCanvasReference(GameObject canvasObject)
    {
        currentCanvas = canvasObject;
    }

    public GameObject BuildUIObject(ModelData objeto, string UIType)
    {

        Debug.Log("Uitype : " + UIType);

        switch (UIType)
        {
            case "Botao":
                return ConfigureButtonUI(objeto);
            case "Imagem":
                return ConfigureImageUI(objeto);
            case "Texto":
                return ConfigureTextUI(objeto);
            default:
                Debug.Log("Erro: UI não reconhecida; objeto não foi criado.");
                break;
        }

        return null;
    }

    private GameObject ConfigureButtonUI(ModelData objeto)
    {

        GameObject buttonObject = Instantiate(Resources.Load("DefaultButton", typeof(GameObject))) as GameObject;
        buttonObject.transform.SetParent(currentCanvas.transform);

        // Image Component
        ConfigureImageComponent(buttonObject, objeto);

        // SetRectComponent();
        SetAnchorsAndPivots(buttonObject);
        SetRectTransform(buttonObject, objeto);

        // SetObject Name
        SetObjectName(buttonObject, objeto.Class.Name);

        // SetText
        SetButtonTextContent(buttonObject, objeto);

        return buttonObject;

    }

    private GameObject ConfigureImageUI(ModelData objeto)
    {

        GameObject imageObject = Instantiate(Resources.Load("DefaultImage", typeof(GameObject))) as GameObject;
        imageObject.transform.SetParent(currentCanvas.transform);

        Debug.Log("Sim, estamos criando iamgem");

        // Add Image
        ConfigureImageComponent(imageObject, objeto);

        // Configure Rect Transform
        SetAnchorsAndPivots(imageObject);
        SetRectTransform(imageObject, objeto);

        // SetObject Name
        SetObjectName(imageObject, objeto.Class.Name);

        return imageObject;
    }

    private GameObject ConfigureTextUI(ModelData objeto)
    {
        GameObject textObject = Instantiate(Resources.Load("DefaultText", typeof(GameObject))) as GameObject;
        textObject.transform.SetParent(currentCanvas.transform);

        // Set Text Content
        SetTextUITextContent(textObject, objeto);

        // S
        SetAnchorsAndPivots(textObject);
        SetRectTransform(textObject, objeto);

        // SetObjectName
        SetObjectName(textObject, objeto.Class.Name);

        return textObject;
    }

    private void SetTextUITextContent(GameObject textObject, ModelData objeto)
    {
        if (objeto.Class.Properties.PropertiesKeys.Contains("Conteudo"))
        {
            int conteudoIndex = objeto.Class.Properties.PropertiesKeys.IndexOf("Conteudo");
            textObject.GetComponent<Text>().text = objeto.Class.References.RecursoReferences[conteudoIndex];
        }
        else
        {
            textObject.GetComponent<Text>().text = textObject.name;
        }
    }

    private void SetButtonTextContent(GameObject buttonObject, ModelData objeto)
    {
        Transform textContent = buttonObject.transform.GetChild(0);

        int resourceTextContentIndex = -1;


        if (objeto.Class.References.RecursoReferences != null)
        {
            foreach (string resourceName in objeto.Class.References.RecursoReferences)
            {
                Debug.Log("resourceName : " + modelToUnity.NameReferencesToType[resourceName]);
                // First Case: There is a Texto classReference but actually no Texto file.
                // Second Case: See if the resourceClassRef is a Texto Type
                if (modelToUnity.NameReferencesToType[resourceName] != null && modelToUnity.NameReferencesToType[resourceName].Equals("Texto"))
                {
                    resourceTextContentIndex = objeto.Class.References.RecursoReferences.IndexOf(modelToUnity.NameReferencesToType[resourceName].ToString());
                    break;
                }
            }
        }

        if (resourceTextContentIndex >= 0)
        {
            textContent.gameObject.GetComponent<Text>().text = objeto.Class.References.RecursoReferences[resourceTextContentIndex];
        }
        else
        {
            textContent.gameObject.GetComponent<Text>().text = buttonObject.name;
        }

        // textContent.gameObject.GetComponent<Text>().text = 
    }

    private void AddTextComponent(GameObject gameObject, ModelData objeto)
    {
        int conteudoIndex = objeto.Class.Properties.PropertiesKeys.IndexOf("conteudo");

        Debug.Log("objeto: " + objeto.Class.Name);

        gameObject.AddComponent<Text>();
        Text textUI = gameObject.GetComponent<Text>();

        if (conteudoIndex <= 0)
        {
            textUI.text = objeto.Class.Properties.PropertiesValues[conteudoIndex].ToString();
        }
        else
        {
            textUI.text = "Text Placeholder ";
        }
    }

    private void ConfigureImageComponent(GameObject gameObject, ModelData objeto)
    {
        // TO DO: Set With FIle
    }

    private void SetRectTransform(GameObject gameObject, ModelData objeto)
    {
        Vector3 vector3 = new Vector3(0.0f, 0.0f);
        float width = 0.0f;
        float height = 0.0f;

        // If the object has Width and Height
        if (objeto.Class.Properties.PropertiesKeys.Contains("W"))
        {
            // Get Index of objeto model that contains both height and width
            int Windex = objeto.Class.Properties.PropertiesKeys.IndexOf("W");
            int Hindex = objeto.Class.Properties.PropertiesKeys.IndexOf("H");

            // Parse the values
            width = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Windex]);
            height = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Hindex]);

            // Get positions, if any. Otherwise it will be (0,0)
            if (objeto.Class.Properties.PropertiesKeys.Contains("X"))
            {
                int Xindex = objeto.Class.Properties.PropertiesKeys.IndexOf("X");
                int Yindex = objeto.Class.Properties.PropertiesKeys.IndexOf("Y");
                vector3.x = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Xindex]);
                vector3.y = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Yindex]);
            }

            // Set Both size and position.
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }

        if (objeto.Class.Properties.PropertiesKeys.Contains("X"))
        {
            int Xindex = objeto.Class.Properties.PropertiesKeys.IndexOf("X");
            int Yindex = objeto.Class.Properties.PropertiesKeys.IndexOf("Y");
            vector3.x = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Xindex]);
            vector3.y = (float)int.Parse(objeto.Class.Properties.PropertiesValues[Yindex]);
        }

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(vector3.x, vector3.y);
    }

    public void SetTransform()
    {

    }

    public void AddComponentes(ModelData objeto, GameObject newObject)
    {
        Debug.Log("newObejct: " + newObject.name);

        string folderPath = scriptsFolderPath;

        if (objeto.Class.References.ComponenteReferences != null && objeto.Class.References.ComponenteReferences.Count > 0)
        {
            foreach (string comportamento in objeto.Class.References.ComponenteReferences)
            {
                Debug.Log("comportamento::: " + comportamento);
                string type = modelToUnity.NameReferencesToType[comportamento] as string;
                Debug.Log("type::: " + type);
                // Is a Gatilho or Compartamento?
                // comportamento.Equals("AoClicar")
                if (type.Equals("AoClicar"))
                {
                    if(newObject.GetComponent<AoClicar>() == null)
                    {
                        newObject.AddComponent<AoClicar>();
                    }
                    MarkToAddButtonResponse(comportamento, type, newObject);
                }
                else
                {

                }
            }
        }
    }

    public void MarkToAddButtonResponse(string gatilhoReference, string type, GameObject newObject)
    {
        //build mehtod name
        // AoClicar aC = new AoClicar();

        aoClicarManagement.WriteLineOfFile(newObject.name + "-" + gatilhoReference);


        // onClickCallbackReferences.Add(newObject, gatilhoReference);

        //  Delegate.CreateDelegate(typeof(AoClicar), t.GetMethod(gatilhoReference));

        /*
        UnityEditor.Events.UnityEventTools.AddPersistentListener(newObject.GetComponent<Button>().onClick,
            Delegate.CreateDelegate(typeof(AoClicarDelegate), t.GetMethod(gatilhoReference))
            as UnityEngine.Events.UnityAction);

        /*
        newObject.GetComponent<Button>().onClick.AddListener(
            Delegate.CreateDelegate(typeof(AoClicarDelegate), t.GetMethod(gatilhoReference))
            as UnityEngine.Events.UnityAction
            );
        */

    }

    public void BuildGameFromModels(ModelToUnity modelToUnity)
    {

        //
        SetProjectSettings();

        // Scenes
        foreach (DictionaryEntry dentry in modelToUnity.NameReferencesToType)
        {
            string screenCandidate = dentry.Value.ToString();
            if (screenCandidate.Equals("Tela") || screenCandidate.Equals("Questao"))
            {
                Debug.Log("Tela name:" + dentry.Value.ToString());
                if (screenCandidate.Equals("Tela"))
                {
                    Scene newScene = BuildScene(dentry, modelToUnity);
                }
                else 
                {
                    Scene newScene = BuildQuestaoScene(dentry, modelToUnity);
                }
            }
        }

        // DIrector Set-up for runtime coding
        // SaveDataForRuntime();

        // Copiar ESTADOS to resources: 
        CopyJSONStatesToResources();
    }

    public Scene BuildScene(DictionaryEntry sceneEntry, ModelToUnity modelToUnity)
    {
        // Create a Scene and set as the active Scene
        var createdScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        EditorSceneManager.SetActiveScene(createdScene);

        // Change it's name
        string scenename = sceneEntry.Key.ToString();
        createdScene.name = scenename;

        aoClicarManagement = new AoClicarManagement();
        aoClicarManagement.InitializeWriter(scenename);


        // IF -> TELAINICIAL (TO DO UPDATE TOP ESTADOS)
        if (scenename.Equals("TelaInicial"))
        {
            CreateDirectorObject();
            CreateComportamentoObject();
            // CreateAdventureCotnrollerObject();
        }

        //Get Model Data of scene
        currentCanvas = CreateACanvas();

        // Create EventSystem;
        CreateEventSystem();

        ModelData sceneModelData = (ModelData)modelToUnity.NameReferencesToModelData[scenename];

        //create Objeto
        foreach (string objetoName in sceneModelData.Class.References.ObjetoReferences)
        {
            Debug.Log("objetoName :" + objetoName);
            BuildGameObject(objetoName, modelToUnity);
        }

        /*
        foreach (string resourceName in sceneModelData.Class.References.RecursoReferences)
        {
            Debug.Log("recurso name: " + resourceName);
            BuildResourceObject(resourceName, modelToUnity);
        }
        */

        // IF - SCREEN TYPE IS QUESTION THEN 
        // SetAsQuestionScene()

        aoClicarManagement.CloseWriter();
        aoClicarManagement = null;

        EditorSceneManager.SaveScene(createdScene, sceneFolderPath + scenename + ".unity");

        return createdScene;
    }

    public Scene BuildQuestaoScene(DictionaryEntry sceneEntry, ModelToUnity modelToUnity)
    {
        var createdScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        EditorSceneManager.SetActiveScene(createdScene);

        string scenename = sceneEntry.Key.ToString();
        createdScene.name = scenename;

        //Get Model Data of scene
        currentCanvas = CreateACanvas();

        // Create EventSystem;
        CreateEventSystem();

        ModelData sceneModelData = (ModelData) modelToUnity.NameReferencesToModelData[scenename];

        string questionFormat = GetQuestionFormat(sceneModelData);
        string questiontext = GetQuestionText(sceneModelData);

        SetQuestionUIText(questiontext + "?");

        for(int i = 0; i < sceneModelData.Class.References.AlternativaReferences.Count; i++)
        {
            string alternativaReference = sceneModelData.Class.References.AlternativaReferences[i];
    
            int index = modelToUnity.NameReferencesToModelData[alternativaReference].Class.Properties.PropertiesKeys.IndexOf("conteudo");

            GameObject buttonObject = Instantiate(Resources.Load("BotaoAlternativa", typeof(GameObject))) as GameObject;
            buttonObject.transform.Find("Text").GetComponent<Text>().text = modelToUnity.NameReferencesToModelData[alternativaReference].Class.Properties.PropertiesValues[index];

            buttonObject.name = "Alternativa" + i.ToString();
            buttonObject.transform.SetParent(currentCanvas.transform);

            SetRectTransformForQuestion(buttonObject, i);
        }


        // 1024/576
        //ALL:
        // Pivot: 0.5 0.5


        // Alterantiva1 and ALterantiva3:
        // anchor preserts : bottom-left
        //  280 - 209 <84>;
        //

        // Alterantiva2 and Alternativa4:
        // anchor presets: bottom-right
        // -280 209

        EditorSceneManager.SaveScene(createdScene, sceneFolderPath + scenename + ".unity");

        return createdScene;
    }

    private void SetQuestionUIText(string questionText)
    {
        GameObject textObject = Instantiate(Resources.Load("QuestionText", typeof(GameObject))) as GameObject;
        textObject.transform.SetParent(currentCanvas.transform);
        textObject.name = "Enunciado";
        RectTransform reference = textObject.GetComponent<RectTransform>();
        reference.anchorMin = new Vector2(0.5f, 1.0f);
        reference.anchorMax = new Vector2(0.5f, 1.0f);
        reference.pivot = new Vector2(0.5f, 0.5f);
        reference.anchoredPosition = new Vector2(0.0f, -120.0f);
        textObject.GetComponent<Text>().text = questionText;
    }

    private void SetRectTransformForQuestion(GameObject gameObject, int alternativaIndex)
    {
        RectTransform reference = gameObject.GetComponent<RectTransform>();

        reference.anchorMin = new Vector2(0.0f, 0.0f);
        reference.anchorMax = new Vector2(0.0f, 0.0f);
        reference.pivot = new Vector2(0.5f, 0.5f);

        switch (alternativaIndex)
        {
            case 0:
                reference.anchoredPosition = new Vector2 (280.0f, 240.0f);
                break;
            case 1:
                reference.anchorMin = new Vector2(1.0f, 0.0f);
                reference.anchorMax = new Vector2(1.0f, 0.0f);
                reference.anchoredPosition = new Vector2(-280.0f, 240.0f);
                break;
            case 2:
                reference.anchoredPosition = new Vector2(280.0f, 90.0f);
                break;
            case 3:
                reference.anchorMin = new Vector2(1.0f, 0.0f);
                reference.anchorMax = new Vector2(1.0f, 0.0f);
                reference.anchoredPosition = new Vector2(-280.0f, 90.0f);
                break;
        }
    }

    public string GetQuestionFormat(ModelData modelData)
    {
        string value = "multiplaEscolha"; // default
        int index;

        if (modelData.Class.Properties.PropertiesKeys.Contains("FormatoQuiz"))
        {
            index = modelData.Class.Properties.PropertiesKeys.IndexOf("FormatoQuiz");
            value = modelData.Class.Properties.PropertiesValues[index];
        }

        return value;
    }

    public string GetQuestionText(ModelData modelData)
    {
        string value = "Sample Text"; // default
        int index;

        if (modelData.Class.Properties.PropertiesKeys.Contains("Enunciado"))
        {
            index = modelData.Class.Properties.PropertiesKeys.IndexOf("Enunciado");
            value = modelData.Class.Properties.PropertiesValues[index];
        }

        return value;
    }

    public void BuildGameObject(string objetoName, ModelToUnity modelToUnity)
    {

        // Get model  it data
        Debug.Log("objectName: " + objetoName);


        ModelData objeto = (ModelData)modelToUnity.NameReferencesToModelData[objetoName];

        // Search if it's a common user interface object
        int predefinedIndexUI = objeto.Class.Properties.PropertiesKeys.IndexOf("IU");

        if (predefinedIndexUI >= 0)
        {
            // If belongs to UI group, then rect transform.
            string UIname = objeto.Class.Properties.PropertiesValues[predefinedIndexUI];

            GameObject newObject = BuildUIObject(objeto, UIname);

            // Check for Components
            AddComponentes(objeto, newObject);
        }
        else
        {
            GameObject newObject = new GameObject();

            // Set its parent to canvas
            newObject.transform.SetParent(currentCanvas.transform);

            // Set obejct name
            newObject.name = objetoName;

            // Set if it's present o nscreen
            newObject.SetActive(SetestaAtivo(objetoName, modelToUnity));

            Debug.Log("Building " + objetoName);
            // properties
        }
        // Create new empty object
    }

    public void BuildResourceObject(string resourceName, ModelToUnity modelToUnity)
    {
        GameObject newObject = new GameObject();
        newObject.transform.SetParent(currentCanvas.transform);
        ModelData objeto = (ModelData)modelToUnity.NameReferencesToModelData[resourceName];
        newObject.name = resourceName;
        newObject.SetActive(SetestaAtivo(resourceName, modelToUnity));

        string resourceType = modelToUnity.NameReferencesToType[resourceName].ToString();
        Debug.Log("RESOURCE TYPE:" + resourceType);

        switch (resourceType)
        {
            case "Imagem":
                AddImageComponent(newObject, objeto, modelToUnity);
                break;
            case "Texto":
                AddTextComponent(newObject, objeto, modelToUnity);
                break;
            default:
                Debug.Log("Recurso " + resourceType + " não reconhecido.");
                break;
        }

        // SetPositionForObject(newObject, objeto, modelToUnity);
    }

    private static void AddImageComponent(GameObject newObject, ModelData objeto, ModelToUnity modelToUnity)
    {
        int indexW = objeto.Class.Properties.PropertiesKeys.IndexOf("W");
        int indexH = objeto.Class.Properties.PropertiesKeys.IndexOf("H");

        newObject.AddComponent<Image>();
        Image imageUI = newObject.GetComponent<Image>();

        // Opaque White
        imageUI.color = new Color(255, 255, 255, 255);
    }

    private static void AddTextComponent(GameObject newObject, ModelData objeto, ModelToUnity modelToUnity)
    {
        int conteudoIndex = objeto.Class.Properties.PropertiesKeys.IndexOf("conteudo");

        Debug.Log("objeto: " + objeto.Class.Name);

        newObject.AddComponent<Text>();
        Text textUI = newObject.GetComponent<Text>();

        textUI.text = objeto.Class.Properties.PropertiesValues[conteudoIndex].ToString();
    }


    private static void SetAnchorsAndPivots(GameObject newObject)
    {
        RectTransform rt = newObject.GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(0.0f, 0.0f);
        rt.anchorMin = new Vector2(0.0f, 0.0f);
        rt.pivot = new Vector2(0.0f, 0.0f);

    }

    private static bool SetestaAtivo(string objetoName, ModelToUnity modelToUnity)
    {
        ModelData objeto = (ModelData)modelToUnity.NameReferencesToModelData[objetoName];
        bool gameObjectIsActive = true;

        if (objeto.Class.Properties.PropertiesKeys.Contains("estaAtivo"))
        {
            int estaAtivoIndex = objeto.Class.Properties.PropertiesKeys.IndexOf("estaAtivo");
            string valueOfBool = (string)objeto.Class.Properties.PropertiesValues[estaAtivoIndex];
            if (valueOfBool.Equals("false"))
            {
                gameObjectIsActive = false;
            }
        }

        return gameObjectIsActive;
    }

    private static GameObject CreateACanvas()
    {
        GameObject newCanvas = new GameObject();
        newCanvas.name = "Canvas";
        newCanvas.AddComponent<Canvas>();
        Canvas canvas = newCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();
        return newCanvas;
    }

    private static void CreateEventSystem()
    {
        GameObject newES = new GameObject();
        newES.name = "EventSystem";
        newES.AddComponent<EventSystem>();
        newES.AddComponent<StandaloneInputModule>();
    }

    private void SetObjectName(GameObject gameObject, string name)
    {
        gameObject.name = name;
    }

    private void CreateDirectorObject()
    {
        directorObject = Instantiate(Resources.Load("_DirectorController", typeof(GameObject))) as GameObject;
        directorObject.name = "_DirectorController";
    }

    private string SetFolderToSaveDataForRuntime()
    {

        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        Debug.Log("MyPath!!!:" + path);

        try
        {
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
        catch (IOException e)
        {
            Debug.Log(e + ". Caminho é arquivo ou armazenamento cheio.");
        }

        return path;

        // SaveObjectToCallbackReference(path);
    }

    private void SaveObjectToCallbackReference(string folderPath)
    {
        FileStream fs = File.OpenWrite(folderPath + "OnClick.txt");
        //

        using var sr = new StreamWriter(fs);

        foreach (GameObject go in onClickCallbackReferences.Keys)
        {
            sr.WriteLine(go.name + "," + onClickCallbackReferences[go]);
        }

        sr.Close();
        fs.Close();
    }

    private void CreateComportamentoObject()
    {
        GameObject AoClicarObject = Instantiate(Resources.Load("_AoClicar", typeof(GameObject))) as GameObject;
        AoClicarObject.name = "_AoClicar";
    }

    private void CreateAdventureCotnrollerObject()
    {
        GameObject AdventureController = Instantiate(Resources.Load("_AdventureController", typeof(GameObject))) as GameObject;
        AdventureController.name = "_AdventureControl";
    }

    private void SetProjectSettings()
    {
        SetGameResources();
    }

    private void SetGameResources()
    {
        // FullScreenMdoe windowed
        // SET GAME WIDTH
        gameWindowWidth = 1024;
        // SET GAME HEIGHT
        gameWindowHeight = 576;

    }

    private void CopyJSONStatesToResources()
    {
        // need from mdoel Path
        string estadosPath = Path.Combine("F:/eclispe-workspace/runtime-New_configuration/novoexemplo/src-gen/models/Estado");
        char[] separator = { '\\', '/', '.' };

        string[] filesFromFolder = Directory.GetFiles(estadosPath);

        string resourcePath = Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar;

        string proximoStatesPath = "ProximoStates";
        string adventureStatesPath = "AdventureStates";

        Directory.CreateDirectory(Path.Combine(resourcePath,proximoStatesPath));
        Directory.CreateDirectory(Path.Combine(resourcePath,adventureStatesPath));

        foreach (string file in filesFromFolder)
        {
            // Split each part of the path for dir and the file
            string[] substrings = file.Split(separator);
            Debug.Log("Arquivo ->" + file);
            string endPathForFolder = Path.Combine(resourcePath, proximoStatesPath) + Path.AltDirectorySeparatorChar;
            endPathForFolder.Replace('/','\\');       
            string sourceFile = file.Replace('/', '\\');


            //
            File.Copy(sourceFile, Path.Combine(resourcePath, adventureStatesPath) + Path.AltDirectorySeparatorChar +  substrings[substrings.Length-2] + ".json", true);

            string stateModelDataPath = File.ReadAllText(sourceFile);
            ModelData stateModelData = JsonConvert.DeserializeObject<ModelData>(stateModelDataPath);

            if (stateModelData.Class.Properties.PropertiesKeys.Contains("ProximoEstados"))
            {
                int index = stateModelData.Class.Properties.PropertiesKeys.IndexOf("ProximoEstados");

                // Load different next states
                string[] nextStates = stateModelData.Class.Properties.PropertiesValues[index].Split(null);

                //  Open file for Creation  of that state
                FileStream fs = new FileStream(endPathForFolder + GetFullCodeEquivalent(stateModelData.Class.Name) + ".txt", FileMode.OpenOrCreate);
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (string stateName in nextStates)
                    {
                        string stateCode = GetFullCodeEquivalent(stateName);
                        if(stateName.Equals(nextStates[nextStates.Length-1]))
                        {
                            writer.Write(stateCode);
                        }
                        else
                        {
                            writer.Write(stateCode + "-");
                        }
                    }
                }
                fs.Close();
            }
        }
    }

    private void SetAsQuestionScene()
    {

    }

    private string GetFullCodeEquivalent(string code)
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

    public void BuildReferenceTypeDictionaryFile()
    {

        string resourcesFolderPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Resources" + Path.AltDirectorySeparatorChar;
        resourcesFolderPath.Replace( '/', '\\');

        FileStream stream = null;

        try
        {
            stream = new FileStream(resourcesFolderPath + "referenceToType.txt", FileMode.OpenOrCreate);

            using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                Debug.Log("stuff");
                foreach (string classReferenceKey in modelToUnity.NameReferencesToType.Keys)
                {
                    streamWriter.WriteLine(classReferenceKey);
                    streamWriter.WriteLine(modelToUnity.NameReferencesToType[classReferenceKey]);
                }
                streamWriter.Close();
            }
        }
        finally
        {

        }
        /*
        catch (Exception e)
        {
            Debug.Log("Something is wrong when writing file: " + e.Message);
        }
        */
    }
}
