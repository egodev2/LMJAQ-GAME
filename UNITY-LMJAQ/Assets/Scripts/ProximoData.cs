using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximoData 
{
   public string Conteudo { get; set; }
   public AdventureState AdventureStateReference { get; set; }

   public void AddConteudo(Dictionary<string, ModelData> modelDataDict, string analyzedStateCode)
   {
        int i = 0;
        foreach (string key in modelDataDict[analyzedStateCode].Class.Properties.PropertiesKeys)
        {
            if(key.Equals("conteudo"))
            {
                Conteudo = modelDataDict[analyzedStateCode].Class.Properties.PropertiesValues[i];
            }
        }
        i++;
   }
}
