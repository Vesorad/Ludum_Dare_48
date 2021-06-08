using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMana : MonoBehaviour
{
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void MenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ExitGame();
        }

    }
    public void VolumeSet(float volume)
    {
        if (_audioManager==null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }

        _audioManager.SetVolume(volume);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

   public void MusicKret()
    {
        if (_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        _audioManager.Play("Moledigging");
    }
    public void ButtonMusic()
    {
        if (_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        _audioManager.Play("Button");
    }

    public void PathMusic()
    {
        if (_audioManager == null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        _audioManager.Play("ConfirmPath");
    }

    public void GoToCredits(GameObject credits)
    {
        credits.SetActive(true);
    }

    public void CloseCredits(GameObject credits)
    {
        credits.SetActive(false);
    }
}
