using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    Vector2 deltaPos = Vector2.zero;

    [Header("Constraint")]
    public float constraintDistance = 0.3f;
    public LayerMask collideLayers;
    public Vector3 collisionOffset;
    private Vector2[] contraintDirections = new Vector2[4] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

    //Animation
    private Animator Anim;
    Vector2 direction = Vector2.down;

    

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        deltaPos = Vector2.zero;
        Vector3 pos = transform.position;
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        deltaPos.x += input.x;
        deltaPos.y += input.y;

        LayerMask mask = collideLayers;

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

        if (deltaPos.magnitude > 1)
        {
            deltaPos.Normalize();
        }
        if (deltaPos.magnitude > 0.1f)
        {
            direction = deltaPos;
        }

        Anim.SetFloat("x", direction.x);
        Anim.SetFloat("y", direction.y);
        Anim.SetFloat("speed", deltaPos.magnitude);

        deltaPos *= speed * Time.deltaTime;
        pos += new Vector3(deltaPos.x, deltaPos.y, 0);

        transform.position = pos;
    }
}
