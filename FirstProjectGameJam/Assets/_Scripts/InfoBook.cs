using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBook : MonoBehaviour
{
    [SerializeField] private GameObject[] bookLockers;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite wormSprite;
    [SerializeField] private GameObject mark;

    [HideInInspector] private int collectedWorms = 3;
    [HideInInspector] private int actualCollectIndex = 0;

    private string lockText;
    private string wormText;

    private void Awake()
    {
        SetTextsAndImages();
    }

    private void SetTextsAndImages()
    {
        lockText = "Locked";
        wormText = $"Collect {collectedWorms}x";


        for (int i = 0; i < bookLockers.Length; i++)
        {
            if (i == actualCollectIndex)
            {
                texts[i].SetText(wormText);
                images[i].sprite = wormSprite;
            }
            else if (i > actualCollectIndex)
            {
                texts[i].SetText(lockText);
                images[i].sprite = lockSprite;
            }
        }
    }

    public void CollectWorm()
    {
        if (actualCollectIndex < bookLockers.Length)
        {
            collectedWorms--;

            if (collectedWorms == 0)
            {
                mark.SetActive(true);
                bookLockers[actualCollectIndex].SetActive(false);
                actualCollectIndex++;
                collectedWorms = 3;
            }

            SetTextsAndImages();
        }
    }

    public void OffMark()
    {
        mark.SetActive(false);
    }
}
