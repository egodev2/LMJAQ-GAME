using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureState
{
    char []letters;
    char []numbers;

    public Dictionary <string, ProximoData> PossibleTargetStates { get; set; }

    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }

    public AdventureState(string code, bool isInitial, bool isFinal)
    {
        // Initialize Dict
        PossibleTargetStates = new Dictionary<string, ProximoData>();

        letters = new char[3];
        numbers = new char[3];

        // Add Code
        letters[0] = code[0];
        letters[1] = code[1];
        letters[2] = code[2];
        numbers[0] = code[3];
        numbers[1] = code[4];
        numbers[2] = code[5];

        // SEt Inital/Final
        IsInitial = isInitial;
        IsFinal = isFinal;
    }

    public void AddADventureStateTransition(string code, AdventureState adventure)
    {
        ProximoData proximoData = new ProximoData();
        proximoData.AdventureStateReference = adventure;

        Debug.Log("Added: " + code);
        PossibleTargetStates.Add(code, proximoData);
    }

    public string GetFullCode()
    {
        return GetStringCode() + GetIntCode();
    }

    public string GetStringCode()
    {
        return letters[0].ToString() + letters[1].ToString() + letters[2].ToString();
    }

    public string GetIntCode()
    {
        return numbers[0].ToString() + numbers[1].ToString() + numbers[2].ToString();
    }
}
