using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileChecker : MonoBehaviour
{
    private Tile currentTile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            if (currentTile == null)
            {
                currentTile = collision.GetComponent<Tile>();

                if (currentTile.isMovable)
                {
                    currentTile.isStartingPathTile = true;
                }
            }
            else
            {
                currentTile.isStartingPathTile = false;
                currentTile = collision.GetComponent<Tile>();

                if (currentTile.isMovable)
                {
                    currentTile.isStartingPathTile = true;
                }
            }
        }
    }
}
