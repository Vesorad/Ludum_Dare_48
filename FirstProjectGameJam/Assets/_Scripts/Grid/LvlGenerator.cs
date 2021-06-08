using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlGenerator : MonoBehaviour
{
    public SchellLvl lvlName;
    [SerializeField] private List<GameObject> tilesSegments = new List<GameObject>();
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject gridBoxChecker;
    [Space(10)]
    [SerializeField] private int numberOfSegmentsToGenerate;
    [SerializeField] private int segmentSize;
    [Tooltip("Liczone w segmentach")] [SerializeField] private int lvlHeight;

    private float deltaY;
    private Grid grid;
    private bool lvlIsActive;

    [HideInInspector] public Tile[] tiles;
    [HideInInspector] public int capturetSchells;
    [HideInInspector] public List<GameObject> createdBorders = new List<GameObject>();

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        grid.deltaY += (lvlHeight * segmentSize);

        deltaY = 0;

        for (int i = 0; i < lvlHeight; i++)
        {
            GenarateLevelSegments(deltaY);

            if (i == lvlHeight - 1)
            {
                GenerateBorder((lvlHeight * segmentSize) - 0.5f);
            }

            CreateBoxChecker();
        }

        tiles = GetComponentsInChildren<Tile>();
    }

    private void GenarateLevelSegments(float y)
    {
        int delta = 0;

        for (int i = 0; i < numberOfSegmentsToGenerate; i++)
        {
            int random = Random.Range(0, tilesSegments.ToArray().Length);
            var obj = Instantiate(tilesSegments[random], transform);
            Vector2 newPos = obj.transform.localPosition;
            newPos.x += delta;
            newPos.y = -y;
            obj.transform.localPosition = newPos;
            tilesSegments.Remove(tilesSegments[random]);

            deltaY = y + segmentSize;
            delta += segmentSize;
        }
    }

    private void GenerateBorder(float y)
    {
        int delta = 0;

        for (int i = 0; i < numberOfSegmentsToGenerate * segmentSize; i++)
        {
            var obj = Instantiate(border, transform);
            Vector2 newPos = obj.transform.localPosition;
            newPos.x += delta;
            newPos.y = -y;
            obj.transform.localPosition = newPos;
            createdBorders.Add(obj);
            delta++;
        }
    }

    private void CreateBoxChecker()
    {
        var boxChecker = Instantiate(gridBoxChecker, transform);
        Vector2 newBoxPos = boxChecker.transform.localPosition;
        newBoxPos.y = (-(lvlHeight * segmentSize) / 2) + 0.5f;
        newBoxPos.x = (transform.localPosition.x + (numberOfSegmentsToGenerate * segmentSize) / 2) - 0.5f;
        boxChecker.transform.localPosition = newBoxPos;

        BoxCollider2D boxCollider2D = boxChecker.GetComponent<BoxCollider2D>();
        Vector2 colliderSize = boxCollider2D.size;
        colliderSize.x = numberOfSegmentsToGenerate * segmentSize;
        colliderSize.y = segmentSize * lvlHeight;
        boxCollider2D.size = colliderSize;
    }
}
