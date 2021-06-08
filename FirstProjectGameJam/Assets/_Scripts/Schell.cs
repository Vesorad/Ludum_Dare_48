using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SchellLvl { lvl1, lvl2, lvl3, lvl4 }

public class Schell : MonoBehaviour
{
    [SerializeField] private SchellLvl schellLvl;
    [SerializeField] private int schellScore;
    [SerializeField] private GameObject textAndLock;

    private SchellLvl lastSchell;
    private Canvas canvas;
    private Grid grid;
    private ScoreManager scoreManager;
    private LvlGenerator[] lvlGenerators;
    private AudioManager _audioManager;
    LvlGenerator currentLvl;
    private int _comboNumber = 0;
    private bool _doCombo = false;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        grid = FindObjectOfType<Grid>();
        scoreManager = FindObjectOfType<ScoreManager>();
        lvlGenerators = FindObjectsOfType<LvlGenerator>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void AddCapturedSchellScore()
    {
        int score = 0;

        for (int i = 0; i < lvlGenerators.Length; i++)
        {
            if (lvlGenerators[i].lvlName == schellLvl)
            {
                lvlGenerators[i].capturetSchells++;
                score = lvlGenerators[i].capturetSchells * schellScore;
            }

            if (_comboNumber == 0)
            {
                foreach (var VARIABLE in FindObjectsOfType<Schell>())
                {
                    VARIABLE.lastSchell = this.schellLvl;
                    VARIABLE._comboNumber++;
                }
            }
            else if (_comboNumber >= 1 && this.schellLvl == lastSchell)
            {
                _doCombo = true;
                Debug.Log("Combo");
                _comboNumber++;
            }
            else
            {
                _comboNumber = 0;
                _doCombo = false;
            }
            scoreManager.AddToScore(score, _doCombo);
        }

        if (_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }

        _audioManager.Play("Points");
        scoreManager.numberOfCollectedShells++;
        scoreManager.UpgradeSheels();
    }

    public void UnlockNextLvl()
    {
        for (int i = 0; i < lvlGenerators.Length; i++)
        {
            if (lvlGenerators[i].lvlName == schellLvl)
            {
                currentLvl = lvlGenerators[i];
            }
        }

        if (currentLvl.capturetSchells == 1)
        {
            grid.activeLvlIndex++;

            if (grid.activeLvlIndex < 4)
            {
                foreach (var tile in grid.createdLevels[grid.activeLvlIndex].tiles)
                {
                    tile.UnlockTile();
                }

                foreach (var item in currentLvl.createdBorders)
                {
                    Destroy(item);
                }
                currentLvl.createdBorders.Clear();
                Instantiate(textAndLock, canvas.transform);
            }
        }
    }
}
