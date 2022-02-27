using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisual : MonoBehaviour
{
    public CharacterStats stats;
    public Image characterRen;
    public Image hpVisual;
    public GameObject HitVFX;

    private void Start()
    {
        characterRen.sprite = stats.sprite;
    }
    private void Update()
    {
        if(stats.hp <= 0)
        {
            characterRen.color = Color.grey;
        }
        else
        {
            characterRen.color = Color.white;
        }
        hpVisual.fillAmount = ((float)stats.hp / stats.maxHp);
    }

    public void OnHit()
    {
        if(HitVFX!= null)
        {
            Instantiate(HitVFX, transform);
        }
    }
}
