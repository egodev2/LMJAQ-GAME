using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponenteManagement : MonoBehaviour
{
    public ComponenteManagement()
    {

    }

    public void AddComponente(string nomeDaComponente, GameObject gameObject, ModelData objeto)
    {

        switch (nomeDaComponente)
        {
            case "Gatilho":
                AddGatilho(gameObject,objeto);
                break;
            case "Comportamento":
                AddComportamento(gameObject,objeto);
                break;
            default:
                Debug.Log("Erro: não sei - Componente");
                break;
        }
    }

    public void AddGatilho(GameObject gameObject, ModelData objeto)
    {

        // Determine if its simple or complex?
        //int proximaTelaIndex = objeto.Class.Properties.PropertiesKeys.IndexOf("AoClicar");
        //textObject.GetComponent<Text>().text = objeto.Class.References.RecursoReferences[conteudoIndex];


    }

    public void AddComportamento(GameObject gameObject, ModelData objeto)
    {

    }
}
