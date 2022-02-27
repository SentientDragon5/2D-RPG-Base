using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character.Interactions 
{
    public class EnterWorldInteractable : Interactable
    {
        public string scene = "Island0";
        public override void Interact(Interactor interactor)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}