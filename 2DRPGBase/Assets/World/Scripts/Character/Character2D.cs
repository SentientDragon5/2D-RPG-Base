using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Character2D : MonoBehaviour
    {
        [Header("Movement")]
        public float speed = 4f;
        Vector2 deltaPos = Vector2.zero;
        Vector2 direction = Vector2.down;

        [Header("Constraint")]
        public LayerMask collideLayers;
        float skinWidth = 0.01f;
        private Vector2[] contraintDirections = new Vector2[4] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

        private Animator anim;
        private CircleCollider2D cCollider;


        void Awake()
        {
            anim = GetComponent<Animator>();
            cCollider = GetComponent<CircleCollider2D>();
        }

        public void Move(Vector2 input)
        {
            deltaPos = Vector2.zero;
            Vector3 pos = transform.position;
            deltaPos.x += input.x;
            deltaPos.y += input.y;

            deltaPos = Constrained(deltaPos);

            if (deltaPos.magnitude > 1)
                deltaPos.Normalize();
            SetAnimator();

            deltaPos *= speed * Time.deltaTime;
            pos += new Vector3(deltaPos.x, deltaPos.y, 0);

            transform.position = pos;
        }

        void SetAnimator()
        {
            if (deltaPos.magnitude > 0.1f)
            {
                direction = deltaPos;
            }

            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);
            anim.SetFloat("speed", deltaPos.magnitude);
        }
        public Vector2 Constrained(Vector2 deltaPos)
        {
            LayerMask mask = collideLayers;
            Vector3 collisionOffset = new Vector3(cCollider.offset.x, cCollider.offset.y, 0f);
            float constraintDistance = cCollider.radius + skinWidth;

            for (int i = 0; i < 4; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + collisionOffset, contraintDirections[i], constraintDistance, mask);
                Debug.DrawRay(transform.position + collisionOffset, new Vector3(contraintDirections[i].x, contraintDirections[i].y, 0) * constraintDistance, (hit.collider != null) ? Color.blue : Color.red);
                if (hit.collider != null)
                {
                    if (i == 0)//right
                    {
                        deltaPos = new Vector2(Mathf.Clamp(deltaPos.x, -1, 0), deltaPos.y);
                    }
                    if (i == 1)//left
                    {
                        deltaPos = new Vector2(Mathf.Clamp(deltaPos.x, 0, 1), deltaPos.y);
                    }
                    if (i == 2)//up
                    {
                        deltaPos = new Vector2(deltaPos.x, Mathf.Clamp(deltaPos.y, -1, 0));
                    }
                    if (i == 3)//down
                    {
                        deltaPos = new Vector2(deltaPos.x, Mathf.Clamp(deltaPos.y, 0, 1));
                    }
                }
            }

            return deltaPos;
        }
    }

}
