using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool isObstacle;
    [SerializeField] private bool hasWorms;
    [SerializeField] private Schell schell;
    [SerializeField] private Sprite destriedSprite;
    [SerializeField] private GameObject objectOnTile;

    [HideInInspector] public bool isMovable;
    [HideInInspector] public bool isStartingPathTile;
    [HideInInspector] public bool isDestroied;
    private SpriteRenderer spriteRenderer;
    private AudioManager _audioManager;
    private InfoBook infoBook;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        infoBook = FindObjectOfType<Movement>().infoBook;
    }

    public void UnlockTile()
    {
        spriteRenderer.color = Color.white;

        if (isObstacle)
        {
            isMovable = false;
        }
        else
        {
            isMovable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasWorms)
        {
            if (other.name == "ScanScore")
            {
                if (_audioManager==null)
                {
                    _audioManager = FindObjectOfType<AudioManager>();
                }
                _audioManager.Play("EatingWorm");
                FindObjectOfType<Movement>().AddMovment();
                infoBook.CollectWorm();
            }
        }

        if (schell != null)
        {
            if (other.name == "ScanScore")
            {
                schell.AddCapturedSchellScore();
                schell.UnlockNextLvl();
            }
        }

        if (other.name == "ScanScore")
        {
            isMovable = false;
            isDestroied = true;
            spriteRenderer.sprite = destriedSprite;

            if (objectOnTile != null)
            {
                Destroy(objectOnTile);
            }
        }
    }

    public void LockTile()
    {
        spriteRenderer.color = Color.gray;

        isMovable = false;
    }
}
