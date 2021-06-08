using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Skipscene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void LoadScene()
    {
        text.SetText("Loading");
        SceneManager.LoadScene(2);
    }
}
