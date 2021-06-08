using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGoust : MonoBehaviour
{
    [SerializeField] private GameObject brickGoust;

    private List<GhostBrick> ghostChilds = new List<GhostBrick>();
    [HideInInspector] public RotateOnClick[] currentPatchToCheck = new RotateOnClick[4];
  /*  [HideInInspector]*/ public int posiblesMoves;

    public void PatchChecker()
    {
        StartCoroutine(WaitForEnd());
    }

    public void ClearChecker()
    {
        foreach (var item in ghostChilds)
        {
            Destroy(item.gameObject);
        }
        ghostChilds.Clear();
    }

    private IEnumerator WaitForEnd()
    {
        for (int i = 0; i < currentPatchToCheck.Length; i++)
        {
            MakeGoust(currentPatchToCheck[i]);
            StartCoroutine(WaitCourutine());
            yield return new WaitForSeconds(2f);
            ClearChecker();
        }
    }

    private void MakeGoust(RotateOnClick currentPatchToCheck)
    {
        for (int i = 0; i < currentPatchToCheck.childs.Length; i++)
        {
            var obj = Instantiate(brickGoust, transform);
            Vector2 newpos = obj.transform.localPosition;
            newpos.x = currentPatchToCheck.childs[i].transform.localPosition.x - currentPatchToCheck.childs[0].transform.localPosition.x;
            newpos.y = currentPatchToCheck.childs[i].transform.localPosition.y - currentPatchToCheck.childs[0].transform.localPosition.y;
            obj.transform.localPosition = newpos;
            ghostChilds.Add(obj.GetComponent<GhostBrick>());
        }
    }

    IEnumerator WaitCourutine()
    {
        yield return new WaitForSeconds(0.1f);
        CheckMoveIsPosible();
    }

    private void CheckMoveIsPosible()
    {
        int numberOfPosible = 0;

        for (int i = 0; i < ghostChilds.Count; i++)
        {
            if (ghostChilds[i].canGo)
            {
                numberOfPosible++;
            }
        }

        if (numberOfPosible == ghostChilds.Count)
        {
            posiblesMoves++;
        }

        StartCoroutine(ChangeRotation());
    }

    private IEnumerator ChangeRotation()
    {
        for (int i = 0; i < 3; i++)
        {
            int numberOfPosible = 0;

            foreach (var item in ghostChilds)
            {
                item.GetComponent<BoxCollider2D>().enabled = false;
                item.canGo = false;
            }

            transform.rotation = Quaternion.Euler(0, 0, 90 * (i + 1));

            foreach (var item in ghostChilds)
            {
                item.GetComponent<BoxCollider2D>().enabled = true;

                yield return new WaitForSeconds(0.1f);

                if (item.canGo)
                {
                    numberOfPosible++;
                }
            }

            if (numberOfPosible == ghostChilds.Count)
            {
                posiblesMoves++;
            }
        }
    }
}
