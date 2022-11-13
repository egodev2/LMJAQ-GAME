using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ModelToUnity 
{
   public Dictionary<string, ModelData> NameReferencesToModelData 
   {    
        get
        {
            return nameReferencesToModelData;
        }
        set
        {
           NameReferencesToModelData = nameReferencesToModelData;
        }
    }

   public  Hashtable NameReferencesToType
    {
        get
        {
            return nameReferencesToType;
        }
        set
        {
            NameReferencesToType = nameReferencesToType;
        }
    }

   private  Dictionary<string,ModelData> nameReferencesToModelData;
   private  Hashtable nameReferencesToType;

   // Initialize both HASHES
   public void InitializeHash()
   {
        nameReferencesToModelData = new Dictionary<string, ModelData>();
        nameReferencesToType = new Hashtable();
   }

   // 
   public void AddModelDataToHash(string keyNameReference, ModelData valeuModelData)
   {
        nameReferencesToModelData.Add(keyNameReference,valeuModelData);
   }

   public void AddTypeToHash(string keyNameReference, string valueType)
   {
        nameReferencesToType.Add(keyNameReference,valueType);
   }
}
