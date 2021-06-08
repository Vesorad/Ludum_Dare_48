using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RandomPathGen : MonoBehaviour
{
   // [SerializeField] private MoveManager moveManager;
    [SerializeField] private List<GameObject> paths;
    [SerializeField] private List<GameObject> parentsPath;
    [SerializeField] List<GameObject> localPath;
    private List<GameObject> _paths = new List<GameObject>();
    [SerializeField] private GameObject panelEndGame;

    private Movement _movement;
    private AudioManager _audioManager;
    private void Awake()
    {
        _movement = FindObjectOfType<Movement>();
        localPath = new List<GameObject>(paths);
        for (int j = 0; j < parentsPath.Count; j++)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                var newPath = Instantiate(paths[i], parentsPath[j].transform.position, transform.rotation);
                newPath.GetComponent<RotateOnClick>().pathIndex = i;
                newPath.transform.parent = parentsPath[j].transform;
                newPath.GetComponent<RotateOnClick>().parentTransform = parentsPath[j];
                _paths.Add(newPath);
                newPath.SetActive(false);
            }
        }

        GenPath();
    }

    public void ResetPaths()
    {
        foreach (var VARIABLE in _paths)
        {
            VARIABLE.GetComponent<RotateOnClick>().ResetPath(true);
        }
    }

    public void GenPath()
    {
        GetComponent<CameraMovment>().enabled = true;
        for (int j = 0; j < parentsPath.Count; j++)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                var kid = parentsPath[j].transform.GetChild(i);
                kid.gameObject.SetActive(false);
            }

            int pathIndex;
            do
            {
                pathIndex = Random.Range(0, localPath.Count);
            } while (localPath[pathIndex] == null);
            
            var child = parentsPath[j].transform.GetChild(pathIndex);
            child.gameObject.SetActive(true);

           // moveManager.currentPaths[j] = child.GetComponent<RotateOnClick>();

            localPath[pathIndex] = null;
        }

      //  moveManager.SetCurrentPaths();

        if (_movement.movment==0)
        {
            EndGame();
        }
        
        localPath = new List<GameObject>(paths);
    }

    public void EndGame()
    {
        if (_audioManager == null)
        {
            _audioManager=FindObjectOfType<AudioManager>();
        }
        _audioManager.Play("Lose");
        panelEndGame.SetActive(true);
        GetComponent<ScoreManager>().UpdateText();
        GetComponent<CameraMovment>().enabled = false;
    }
}
