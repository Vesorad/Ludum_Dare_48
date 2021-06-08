using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeterStartPointForPath : MonoBehaviour
{
    [SerializeField] private GameObject[] startPointsSetters;

    public void OnOfSetters(bool activate)
    {
        if (activate)
        {
            foreach (var item in startPointsSetters)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in startPointsSetters)
            {
                item.SetActive(false);
            }
        }
    }
}
