using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickChecker : MonoBehaviour
{
  /*  [HideInInspector]*/ public CheckBox nearCheckBox;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerTileChecker>())
        {
            nearCheckBox = collision.GetComponent<PlayerTileChecker>().GetComponentInParent<CheckBox>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearCheckBox != null)
        {
            nearCheckBox = null;
        }
    }
}
