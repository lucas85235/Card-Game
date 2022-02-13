using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryData", menuName = "ScriptableObjects/HistoryData", order = 0)]
public class HistoryData : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private string playerName = "";
    [SerializeField] private CharacterSelected character;

    public void SetPlayerName(string newName) => playerName = newName;
    public string GetPlayerName() => playerName;

    public void SetCharacter(string characterName)
    {
        character = (CharacterSelected) Enum.Parse(typeof(CharacterSelected), characterName, true);
    }

    public CharacterSelected GetCharacter()
    {
        return character;
    }
}

public enum CharacterSelected
{
    Men,
    Woman,
}
