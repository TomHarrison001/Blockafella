using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    private List<GameObject> objects, firstGrid, secondGrid;
    private int[,] grid1, grid2;
    private int frame = 0;
    private bool doubleMoves;
    public int Frame { get { return frame; } }

    private void Awake()
    {
        objects = new List<GameObject>();
        firstGrid = new List<GameObject>();
        secondGrid = new List<GameObject>();
    }

    private void Start()
    {
        GenerateNewLevel();
    }

    private void DeleteLevel()
    {
        foreach (var g in objects)
        {
            Destroy(g);
        }
        objects = new List<GameObject>();
    }

    public void GenerateNewLevel()
    {
        frame = 0;
        doubleMoves = false;
        DeleteLevel();
        grid1 = new int[5, 9];
        grid2 = new int[5, 9];
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                var value = Random.Range(0, 2);
                grid1[x, y] = value;
                if (value == 1) CreateObject(x, y, -1, "rock", firstGrid);
                value = Random.Range(0, 2);
                grid2[x, y] = value;
                if (value == 1) CreateObject(x, y, frame, "rock", secondGrid);
            }
        }
        ShowPowerUps();
    }

    public void ShowRocks()
    {
        foreach (var g in firstGrid)
        {
            Destroy(g);
        }
        grid1 = grid2;
        firstGrid = secondGrid;
        secondGrid = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid2[i, j] = Random.Range(0, 2);
            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (grid2[x, y] == 1)
                {
                    CreateObject(x, y, frame, "rock", secondGrid);
                }
            }
        }
    }

    public void ShowPowerUps()
    {
        if (frame == 0)
        {
            for (int i = 0; i < 9 / 5 + 1; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int x = Random.Range(0, 5);
                    int y = Random.Range(4 * i, 4 * (i + 1));
                    if (grid1[x, y] == 0)
                    {
                        grid1[x, y] = 2;
                        CreateObject(x, y, -1, "extraMoves", firstGrid);
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < 9 / 5 + 1; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                int x = Random.Range(0, 5);
                int y = Random.Range(4 * i, 4 * (i + 1));
                if (grid2[x, y] == 0)
                {
                    grid2[x, y] = 2;
                    var chance = 100 - frame;
                    CreateObject(x, y, frame, (Random.Range(0, 100) <= chance ? "extraMoves" : "destroyLine"), secondGrid);
                    break;
                }
            }
        }
        int d = Random.Range(0, 5);
        int b = Random.Range(0, 9);
        if (grid2[d, b] == 0) CreateObject(d, b, frame, (Random.Range(0, 5) == 0 && !doubleMoves ? "doubleMoves" : "bonusScore"), secondGrid);
        frame++;
    }

    private void CreateObject(int x, int y, int frame, string name, List<GameObject> list)
    {
        GameObject g = Instantiate(rock, new Vector2(1.2f * (x - 2f), 1.2f * (3f - y - 9 * (frame + 1))), Quaternion.identity);
        g.transform.localScale = Vector2.one;
        g.name = name;
        objects.Add(g);
        list.Add(g);
        if (name == "extraMoves")
            g.GetComponent<SpriteRenderer>().color = new Color(0.97647f, 1f, 0.37255f);
        if (name == "destroyLine")
            g.GetComponent<SpriteRenderer>().color = new Color(1f, 0.41176f, 0.41176f);
        if (name == "bonusScore")
            g.GetComponent<SpriteRenderer>().color = new Color(0.81176f, 0.66275f, 0.78039f);
        if (name == "doubleMoves") {
            g.GetComponent<SpriteRenderer>().color = new Color(0.49f, 1f, 0.49f);
            doubleMoves = true;
        }
    }
}
