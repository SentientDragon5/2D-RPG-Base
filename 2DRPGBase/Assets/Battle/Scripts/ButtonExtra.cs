using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExtra : MonoBehaviour
{
    public int value;
    public void SetAction()
    {
        GetComponentInParent<TurnBattleManager>().SetAction(value);
    }
    public void SetTarget()
    {
        GetComponentInParent<TurnBattleManager>().SetTarget(value);
    }
}
