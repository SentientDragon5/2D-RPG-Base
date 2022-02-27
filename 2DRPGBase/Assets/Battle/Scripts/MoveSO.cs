using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Move", menuName = "Battle/Move")]
public class MoveSO : ScriptableObject
{
    public int power = 10;
    public UnityEvent Action;
}
