using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBrick : MonoBehaviour
{
    public bool canGo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            if (collision.GetComponent<Tile>().isMovable)
            {
                canGo = true;
            }
        }
    }
}
