using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Interactions;

namespace Character
{
    [RequireComponent(typeof(Character2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Animator))]
    public class UserCharacter : MonoBehaviour
    {
        private Character2D character;
        private Interactor interactor;

        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<Character2D>();
            interactor = GetComponent<Interactor>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed, interacting");
                interactor.Interact();
            }
            character.Move(input);
        }
    }

}
