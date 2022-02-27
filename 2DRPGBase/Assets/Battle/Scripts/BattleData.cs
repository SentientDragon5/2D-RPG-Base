using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Battle/Battle")]
public class BattleData : ScriptableObject
{
    //bg?
    public List<CharacterStats> teamA = new List<CharacterStats>();
    public List<CharacterStats> teamB = new List<CharacterStats>();

}
