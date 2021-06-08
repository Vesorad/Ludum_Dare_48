using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private Grid grid;
    private Rigidbody2D rb;
    private CheckBox brickAllowed;
    private int currentMoveIndex = 0;
    private ScoreManager _scoreManager;
    private RandomPathGen _randomPathGen;
    private AudioManager _audioManager;
    private CheckBox lastBrick;
    private Sprite lastSprite;
    private Tile midTitle;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [HideInInspector] public List<CheckBox> bricksPath = new List<CheckBox>();
    [HideInInspector] public List<int> alternativeWay1Inexes = new List<int>();
    [HideInInspector] public List<int> alternativeWay2Inexes = new List<int>();
    [HideInInspector] public bool isMoving;
    [HideInInspector] public RotateOnClick lastAddedPath;

    [SerializeField] private GameObject scan;
    [SerializeField] private GameObject[] startTilesLocators;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Sprite goBrickSprite;


    public InfoBook infoBook;
    [Header("Dane gracza")]
    public int movment;
    [SerializeField] private float charactersSpeed = 3;
    [SerializeField] private Animator movmentAnim;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = FindObjectOfType<SpriteRenderer>();
        grid = FindObjectOfType<Grid>();
        rb = GetComponent<Rigidbody2D>();
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void FixedUpdate()
    {
        if (isMoving || bricksPath.Count <= 0)
        {
            moveButton.interactable = false;
        }
        else if (!isMoving && bricksPath.Count > 0)
        {
            moveButton.interactable = true;
        }

        Go();
    }

    public void AddMovment()
    {
        movment++;
        movmentAnim.SetBool("Ping", true);
        _scoreManager.UpdateTextMovment();
    }
    private void Go()
    {
        if (isMoving)
        {
            resetButton.interactable = false;

            if (currentMoveIndex < bricksPath.ToArray().Length)
            {
                Rotate();
            }

            if (Vector3.Distance(bricksPath[currentMoveIndex].transform.position, transform.position) < 0.01f)
            {
                currentMoveIndex++;

                if (currentMoveIndex >= bricksPath.ToArray().Length)
                {
                    if (_audioManager == null)
                    {
                        _audioManager = FindObjectOfType<AudioManager>();
                    }
                    _audioManager.Stop("Moledigging");
                    movmentAnim.SetBool("Ping", false);
                    resetButton.interactable = true;
                    isMoving = false;
                    animator.SetTrigger("Indle");
                    currentMoveIndex = 0;
                    bricksPath.Clear();
                    movment--;
                    _scoreManager.UpdateTextMovment();
                    _scoreManager.gameObject.GetComponent<RandomPathGen>().ResetPaths();
                    _scoreManager.gameObject.GetComponent<RandomPathGen>().GenPath();
                    return;
                }
            }
            scan.transform.position = transform.position;
            transform.position = Vector3.MoveTowards(transform.position,
                bricksPath[currentMoveIndex].transform.position, Time.deltaTime * charactersSpeed);
        }
    }

    private void SetPath()
    {
        if (lastAddedPath != null)
        {
            if (lastAddedPath.childs[lastAddedPath.childs.Length - 1].isStartingBrick && lastAddedPath.childs[0].isStartingBrick)
            {
                lastAddedPath.DeleteOneOfAlternativeWays(lastBrick);
            }
        }
    }

    public void Rotate()
    {
        if (transform.position.x < bricksPath[currentMoveIndex].transform.position.x)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.z = -90;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
        if (transform.position.x > bricksPath[currentMoveIndex].transform.position.x)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.z = 90;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
        if (transform.position.y < bricksPath[currentMoveIndex].transform.position.y)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.z = 0;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
        if (transform.position.y > bricksPath[currentMoveIndex].transform.position.y)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.z = 180;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
    }

    public void OnOffTilesLocators(bool activate)
    {
        if (activate)
        {
            foreach (var item in startTilesLocators)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in startTilesLocators)
            {
                item.SetActive(false);
            }
        }
    }

    public void MovePlayer()
    {
        SetPath();
        isMoving = true;
        animator.SetTrigger("Walk");
        ResetColor();
    }

    public void SchowLastPoint()
    {
        if (lastAddedPath != null && !isMoving)
        {
            if (lastAddedPath.childs[lastAddedPath.childs.Length - 1].isStartingBrick && lastAddedPath.childs[0].isStartingBrick)
            {
                if (Vector3.Distance(lastAddedPath.childs[lastAddedPath.childs.Length - 1].transform.position, this.transform.position) >=
                   Vector3.Distance(lastAddedPath.childs[0].transform.position, this.transform.position))
                {
                    lastBrick = lastAddedPath.childs[lastAddedPath.childs.Length - 1];
                    lastSprite = lastAddedPath.childs[lastAddedPath.childs.Length - 1].spriteRenderer.sprite;
                    lastAddedPath.childs[lastAddedPath.childs.Length - 1].spriteRenderer.sprite = goBrickSprite;
                }
                else if (Vector3.Distance(lastAddedPath.childs[lastAddedPath.childs.Length - 1].transform.position, this.transform.position) <
             Vector3.Distance(lastAddedPath.childs[0].transform.position, this.transform.position))
                {
                    lastBrick = lastAddedPath.childs[0];
                    lastSprite = lastAddedPath.childs[0].spriteRenderer.sprite;
                    lastAddedPath.childs[0].spriteRenderer.sprite = goBrickSprite;
                }
            }
            else
            {
                lastBrick = bricksPath[bricksPath.Count - 1];
                lastSprite = bricksPath[bricksPath.Count - 1].spriteRenderer.sprite;
                bricksPath[bricksPath.Count - 1].spriteRenderer.sprite = goBrickSprite;
            }
        }
    }
    public void ResetColor()
    {
        if (lastAddedPath != null)
        {
            lastBrick.spriteRenderer.sprite = lastSprite;
        }
    }
}
