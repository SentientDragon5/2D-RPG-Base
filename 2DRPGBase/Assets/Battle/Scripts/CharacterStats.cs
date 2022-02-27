using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This holds the permanent stats of the character. only edit when leveling up. or editing current HP at end of fight.
/// </summary>
[CreateAssetMenu(fileName = "Character Stats", menuName = "Battle/Character")]
public class CharacterStats : ScriptableObject
{
    public Sprite sprite;
    public int maxHp = 12;
    public int hp = 12;

    public int baseDMG = 2;
    public List<MoveSO> moves = new List<MoveSO>();

    public BattleAI ai;
}
