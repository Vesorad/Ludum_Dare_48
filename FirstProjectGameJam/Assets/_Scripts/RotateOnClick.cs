using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnClick : MonoBehaviour
{
    public CheckBox[] childs;

    private bool _movePath;
    private bool _canPut;
    private bool inGrid;
    public bool stay;
    public GameObject parentTransform;
    private int _checkList;
    public int pathIndex;
    private Movement playerMovement;
    private Grid grid;
    private List<Tile> tiles = new List<Tile>();

    private AudioManager _audioManager;

    private void Awake()
    {
        playerMovement = FindObjectOfType<Movement>();
        grid = FindObjectOfType<Grid>();
    }

    private void OnMouseDown()
    {
        gameObject.transform.parent = null;
        _movePath = true;
    }
    private void Update()
    {
        if (!stay)
        {
            if (_movePath)
            {
                ChangeBrickColor();

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
                transform.position = new Vector3(Mathf.RoundToInt(worldPosition.x) + 0.5f,
                    Mathf.RoundToInt(worldPosition.y) + 0.5f,-1.5f);

                if (Input.GetMouseButtonUp(0))
                {
                    _movePath = false;
                    bool isStart = false;
                    int startPointcheck = 0;

                    for (int i = 0; i < childs.Length; i++)
                    {
                        if (childs[i].GetComponent<CheckBox>().isStartingBrick)
                        {
                            isStart = true;
                            startPointcheck++;
                        }
                    }
                    foreach (var VARIABLE in childs)
                    {
                        if (!VARIABLE.currentTile.isMovable || !isStart)
                        {
                            _canPut = false;
                        }
                    }

                    if (!_canPut)
                    {
                        if (_audioManager==null)
                        {
                            _audioManager = FindObjectOfType<AudioManager>();
                        }
                        _audioManager.Play("Invalid Path");
                        ResetPath(false);
                        
                    }
                    else
                    {
                        
                        if (_audioManager==null)
                        {
                            _audioManager = FindObjectOfType<AudioManager>();
                        }
                        _audioManager.Play("PathSnapToGrid");
                        stay = true;
                        AddBricksToPath();
                        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                        if (playerMovement.lastAddedPath != null)
                        {
                            foreach (var item in playerMovement.lastAddedPath.childs)
                            {
                                item.spriteRenderer.color = new Color(1, 1, 1, 1);
                            }
                        }
                        playerMovement.lastAddedPath = this;

                        playerMovement.OnOffTilesLocators(false);
                        grid.ResetTilesStartPoint();

                        foreach (var VARIABLE in childs)
                        {
                            if (startPointcheck == 1)
                            {
                                if (VARIABLE.GetComponent<SeterStartPointForPath>() && !VARIABLE.isStartingBrick)
                                {
                                    VARIABLE.GetComponent<SeterStartPointForPath>().OnOfSetters(true);
                                }
                            }
                            else if (startPointcheck > 1)
                            {
                                if (VARIABLE.GetComponent<SeterStartPointForPath>())
                                {
                                    VARIABLE.GetComponent<SeterStartPointForPath>().OnOfSetters(true);
                                }
                            }

                            if (VARIABLE.GetComponent<BrickChecker>() && VARIABLE.GetComponent<BrickChecker>().nearCheckBox != null)
                            {
                                DeleteOneOfAlternativeWays(VARIABLE.GetComponent<BrickChecker>().nearCheckBox);
                            }

                            if (childs[childs.Length - 1].isStartingBrick && childs[0].isStartingBrick)
                            {
                                if (VARIABLE == childs[0] || VARIABLE == childs[childs.Length - 1])
                                {
                                    VARIABLE.spriteRenderer.color = Color.green;
                                }
                                else
                                {
                                    VARIABLE.spriteRenderer.color = new Color(1, 1, 1, 1);
                                }
                            }
                            else
                            {
                                if (!VARIABLE.isStartingBrick && (VARIABLE == childs[0] || VARIABLE == childs[childs.Length - 1]))
                                {
                                    VARIABLE.spriteRenderer.color = Color.green;
                                }
                                else
                                {
                                    VARIABLE.spriteRenderer.color = new Color(1, 1, 1, 1);
                                }
                            }

                            tiles.Add(VARIABLE.currentTile);
                            VARIABLE.currentTile.isMovable = false;
                            VARIABLE.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 90);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Respawn")
        {
            _canPut = false;
            _checkList--;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Grid")
        {
            inGrid = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Respawn")
        {
            _checkList++;
            if (_checkList == 0 && inGrid )
            {
                _canPut = true;
            }
        }

        if (other.tag == "Grid")
        {
            inGrid = false;
        }
    }

    private void AddBricksToPath()
    {
        if (childs[0].isStartingBrick && !childs[childs.Length - 1].isStartingBrick)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                playerMovement.bricksPath.Add(childs[i]);
            }
        }
        else if (childs[childs.Length - 1].isStartingBrick && !childs[0].isStartingBrick)
        {
            for (int i = childs.Length - 1; i >= 0; i--)
            {
                playerMovement.bricksPath.Add(childs[i]);
            }
        }
        else if (childs[childs.Length - 1].isStartingBrick && childs[0].isStartingBrick)
        {
            int pathListlastIndex = playerMovement.bricksPath.Count;

            for (int i = 0; i < childs.Length; i++)
            {
                playerMovement.bricksPath.Add(childs[i]);
                playerMovement.alternativeWay1Inexes.Add(pathListlastIndex);
                pathListlastIndex++;
            }

            for (int i = childs.Length - 1; i >= 0; i--)
            {
                playerMovement.bricksPath.Add(childs[i]);
                playerMovement.alternativeWay2Inexes.Add(pathListlastIndex);
                pathListlastIndex++;
            }
        }
    }

    private void ChangeBrickColor()
    {
        bool hasStart = false;
        bool canBePut = false;
        int startChecker = 0;
        int putChecker = 0;

        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i].currentTile.isMovable)
            {
                putChecker++;
            }

            if (childs[i].isStartingBrick)
            {
                startChecker++;
            }

            if (startChecker > 0 && putChecker >= childs.Length)
            {
                hasStart = true;
                canBePut = true;
            }
        }

        foreach (var item in childs)
        {
            if (hasStart && canBePut)
            {
                item.spriteRenderer.color = Color.green;
            }
            else
            {
                item.spriteRenderer.color = Color.red;
            }
        }
    }

    public void DeleteOneOfAlternativeWays(CheckBox nearBrick)
    {
        if (playerMovement.alternativeWay1Inexes.Count > 0 && playerMovement.alternativeWay2Inexes.Count > 0)
        {
            if (nearBrick == playerMovement.bricksPath[playerMovement.alternativeWay1Inexes[playerMovement.alternativeWay1Inexes.Count - 1]])
            {
                for (int i = 0; i < playerMovement.alternativeWay1Inexes.Count; i++)
                {
                    playerMovement.bricksPath.RemoveAt(playerMovement.alternativeWay2Inexes[0]);
                }
                playerMovement.alternativeWay1Inexes.Clear();
                playerMovement.alternativeWay2Inexes.Clear();
            }
            else if (nearBrick == playerMovement.bricksPath[playerMovement.alternativeWay2Inexes[playerMovement.alternativeWay2Inexes.Count - 1]])
            {
                for (int i = 0; i < playerMovement.alternativeWay2Inexes.Count; i++)
                {
                    playerMovement.bricksPath.RemoveAt(playerMovement.alternativeWay1Inexes[0]);
                }
                playerMovement.alternativeWay1Inexes.Clear();
                playerMovement.alternativeWay2Inexes.Clear();
            }
        }
    }

    public void ResetPath(bool hardReset)
    {
        foreach (var VARIABLE in childs)
        {
            VARIABLE.GetComponent<SpriteRenderer>().color = Color.white;

            VARIABLE.GetComponent<BoxCollider2D>().enabled = true;

            VARIABLE.GetComponent<CheckBox>().isStartingBrick = false;

            if (VARIABLE.GetComponent<SeterStartPointForPath>())
            {
                VARIABLE.GetComponent<SeterStartPointForPath>().OnOfSetters(false);
            }
        }
        _canPut = false;
        this.transform.parent = parentTransform.transform;

        transform.position = transform.parent.position;

        this.transform.SetSiblingIndex(pathIndex);

        stay = false;

        if (hardReset)
        {
            foreach (var item in tiles)
            {
                if (!item.isDestroied)
                {
                    item.isMovable = true;
                }
            }
            tiles.Clear();
            playerMovement.OnOffTilesLocators(false);
            grid.ResetTilesStartPoint();
            playerMovement.OnOffTilesLocators(true);
            playerMovement.bricksPath.Clear();
            playerMovement.lastAddedPath = null;
        }
    }
}
