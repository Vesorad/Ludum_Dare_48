using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI scoreTextMovment;
    [SerializeField] private TextMeshProUGUI bigPrize;
    [SerializeField] private int allElements;
   
    private Movement _movement;
   
    [SerializeField] private int _prizeNumber = 0;
    [HideInInspector]public int numberOfCollectedShells=0;
    
    [Header("Punktacja główna")]
    [SerializeField] private Animator comboAnim;
    [SerializeField] private TextMeshProUGUI scoreTextInGame;
    [SerializeField] private TextMeshProUGUI scoreTextEnd;
    [SerializeField] private TextMeshProUGUI scoreTextEnd2;
    [SerializeField] private Animator scoreAnimator;
    private int _score = 0;
    [Header("Panele ")]
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private GameObject pauseMenu;
    
    [Header("Punktacja muszelek")]
    [SerializeField] private TextMeshProUGUI scoreOfdShells;
    [SerializeField] private Animator schellsAnim;
    bool ok= true;
    private void Awake()
    {
        _movement = FindObjectOfType<Movement>();
      
    }

    private void Start()
    {
        allElements = FindObjectsOfType<BigTreas>().Length;
        UpgradeSheels();
    }
    public int GetScore()
    {
        return _score;
    }

    public void UpgradeSheels()
    {
        schellsAnim.SetBool("Ping",true);
        scoreOfdShells.text = numberOfCollectedShells.ToString();
        StartCoroutine(WaitOneSec());
    }
    public void AddToScore(int scoreValue, bool ok)
    {
        if (ok)
        {
            comboAnim.SetBool("Out",true);  
        }
        _score += scoreValue;
        scoreAnimator.SetBool("Ping",true);
        scoreTextInGame.text = GetScore().ToString();
        StartCoroutine(WaitOneSec());
    }

    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(3);
        scoreAnimator.SetBool("Ping",false);
        schellsAnim.SetBool("Ping",false);
        comboAnim.SetBool("Out",false);  
    }
    public void UpdateTextBigPrize()
    {
        _prizeNumber++;
        bigPrize.text = _prizeNumber.ToString();
        if (allElements == _prizeNumber)
        {
            winGamePanel.SetActive(true);
            scoreTextEnd2.text = GetScore().ToString();
        }
    }
    public void UpdateText()
    {
        scoreTextEnd.text = GetScore().ToString();
    }
    public void UpdateTextMovment()
    {
        scoreTextMovment.text = _movement.movment.ToString();
    }
    public void ResetGame()
    {
        _score = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

   public void Pause()
    {
        if (ok)
        {
            pauseMenu.SetActive(true);
            ok = false;
        }
        else
        {
            pauseMenu.SetActive(false);
            ok = true;
        }
    }
}
