using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public int allPosibleMoves;
    [SerializeField] private PathGoust[] pathGousts;
    [SerializeField] private RandomPathGen randomPathGen;

   /* [HideInInspector]*/ public RotateOnClick[] currentPaths;

    private void Awake()
    {
        currentPaths = new RotateOnClick[4];
    }

    public void SetCurrentPaths()
    {
        foreach (var item in pathGousts)
        {
            item.currentPatchToCheck = currentPaths;

            item.PatchChecker();
        }

        StartCoroutine(SumPosibleMovesCourutine());
    }

    private IEnumerator SumPosibleMovesCourutine()
    {
        yield return new WaitForSeconds(8f);

        for (int i = 0; i < pathGousts.Length; i++)
        {
            allPosibleMoves += pathGousts[i].posiblesMoves;
        }

        if (allPosibleMoves == 0)
        {
            randomPathGen.EndGame();
        }
        yield return new WaitForSeconds(8f);
        allPosibleMoves = 0;
    }
}
