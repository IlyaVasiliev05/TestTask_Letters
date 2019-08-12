using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetData : ScriptableObject
{
    public List<LetterData> lettersList;

    public LetterData GetLetterByID(int id)
    {
        return lettersList[id];
    }

}

[System.Serializable]
public class LetterData
{
    public char letter;
    public AudioClip ac;
}