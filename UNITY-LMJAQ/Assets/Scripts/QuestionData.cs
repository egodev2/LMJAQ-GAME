using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionData 
{
    public string Formato { get; set; }

    public string Enunciado { get; set; }

    public string Resposta { get; set; }
    
    public void SetFormato(string format)
    {
        Formato = format;
    }
}
