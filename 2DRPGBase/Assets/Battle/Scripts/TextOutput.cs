using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOutput : MonoBehaviour
{
    public TextMeshProUGUI currentText;

    public void Print(string text)
    {
        currentText.text = text;
    }

    //Check for my Dialouge reader scripts, they have a way to read out text through a coroutine.
}
