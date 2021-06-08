using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;

    [HideInInspector] public int deltaY;
    [HideInInspector] public int activeLvlIndex;
    [HideInInspector] public List<LvlGenerator> createdLevels = new List<LvlGenerator>();

    public Tile[] allTiles;

    private void Awake()
    {
        deltaY = 0;

        for (int i = 0; i < levels.Length; i++)
        {
            GenerateLevel(deltaY, i);

            if (i != 0)
            {
                foreach (var tile in createdLevels[i].tiles)
                {
                    tile.LockTile();
                }
            }
            else
            {
                activeLvlIndex = 0;
                foreach (var tile in createdLevels[activeLvlIndex].tiles)
                {
                    tile.UnlockTile();
                }
            }
        }

        allTiles = FindObjectsOfType<Tile>();
    }

    private void GenerateLevel(float deltaY, int index)
    {
        var obj = Instantiate(levels[index], transform);
        Vector2 newPos = obj.transform.localPosition;
        newPos.y = -deltaY;
        obj.transform.localPosition = newPos;

        createdLevels.Add(obj.GetComponent<LvlGenerator>());
    }

    public void ResetTilesStartPoint()
    {
        foreach (var item in allTiles)
        {
            item.isStartingPathTile = false;
        }
    }
}
