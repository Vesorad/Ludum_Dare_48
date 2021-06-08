using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCheckTilesBox : MonoBehaviour
{
    private Tile currentTile;
    private CheckBox brickAllowed;

    private void Awake()
    {
        brickAllowed = GetComponentInParent<CheckBox>();
        Physics2D.IgnoreCollision(brickAllowed.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            currentTile = collision.GetComponent<Tile>();

            if (currentTile.isStartingPathTile)
            {
                brickAllowed.isStartingBrick = true;
            }
            else
            {
                brickAllowed.isStartingBrick = false;
            }
        }
    }
}
