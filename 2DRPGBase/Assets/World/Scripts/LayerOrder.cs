using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerOrder : MonoBehaviour
{
    public int[] sortingOrders = new int[2] { 1, 3 };
    /* sorting layers:
     * 0 - tilemap
     * 1 - collidables perspectivly Under player, position wise Over the player
     * 2 - Player
     * 3 - collidables perspectivly Over player, position wise Under the player
     * 4 - UI and effects
     */
    public float offsety = 0;
    private SpriteRenderer Sprite;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Sprite)
        {
            if (player.transform.position.y < transform.position.y + offsety)
            {
                Sprite.sortingOrder = sortingOrders[0];
            }
            else
            {
                Sprite.sortingOrder = sortingOrders[1];
            }
        }
    }
}
