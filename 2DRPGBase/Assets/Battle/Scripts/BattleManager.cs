using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleData data;

    public BattleData myData;
    private void Awake()
    {
        if(data == null)
        {
            data = myData;
        }
    }
}
