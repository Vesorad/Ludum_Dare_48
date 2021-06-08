using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [HideInInspector] public Tile currentTile;
  /*  [HideInInspector]*/ public bool isStartingBrick;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private bool checkGrid;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Tile")
        {
            currentTile = other.GetComponent<Tile>();
        }
    }
}
