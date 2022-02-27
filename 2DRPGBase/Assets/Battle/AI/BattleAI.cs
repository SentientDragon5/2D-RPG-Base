using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Behavior", menuName = "Battle/AI")]
public class BattleAI : ScriptableObject
{
    public List<float> actionRates;
    public List<float> moveRates;

    public int Action()
    {
        float rand01 = Random.Range(0f, 1f);
        float sum = 0;
        foreach(float rate in actionRates)
        {
            sum += rate;
        }
        float rand0sum = sum * rand01;
        float adding = 0;
        for (int i = 0; i < actionRates.Count; i++)
        {
            adding += actionRates[i];
            if(adding > rand0sum)//if the current sum is greater than your random, do action, else wait for it to add up right
            {
                return i;
            }
        }
        return 0;
    }

    public int RandTarget(int max)
    {
        return Random.Range(0, max);
    }
}
