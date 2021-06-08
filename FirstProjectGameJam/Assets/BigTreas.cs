using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigTreas : MonoBehaviour
{
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name =="ScanScore")
        {
           _scoreManager.UpdateTextBigPrize();
           Destroy(gameObject);
        }
    }
}
