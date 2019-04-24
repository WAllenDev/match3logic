using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int columns = 8;
    public int rows = 8;
    public Tile foodTile;
    public Tile waterTile;
    public Tile resourceTile;
    public Tile populationTile;
    public Tile technologyTile;
    public Tile[] tiles;
    public Tile[,] boardPositions;
    public float animSpeed = 0.5f;
    public float moveTime = 1f;
    private Transform boardHolder;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitialiseList();
    }

    void InitialiseList()
    {
        tiles = new Tile[] { foodTile, waterTile, resourceTile, populationTile, technologyTile };
        boardPositions = new Tile[columns, rows];
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                Tile nextTile;
                int i = 0;
                do
                {
                    i++;
                    nextTile = tiles[Random.Range(0, tiles.Length)];
                    nextTile.Assign(x, y);
                } while (MakesMatch(nextTile));
                boardPositions[x, y] = PlaceNewTile(x, y, nextTile);
                MoveTile(boardPositions[x,y]);
            }
        }
    }

    Tile PlaceNewTile(int column, int row, Tile newTile)
    {
        Tile placing = Instantiate(newTile, new Vector3(column, 16, 0f), Quaternion.identity) as Tile;
        placing.GetComponent<Tile>().Assign(column, row);
        return placing;
    }

    bool MakesMatch(Tile moved)
    {
        bool matches = VerticalMatches(moved).Count > 2 || HorizontalMatches(moved).Count > 2;
        return matches;
    }

    private List<Tile> VerticalMatches(Tile movedTile)  //Will always return at least 1 item in the list.  movedTile is always added to the list.
    {
        List<Tile> Matches = new List<Tile>();
        Matches.Add(movedTile);
        int yPos = movedTile.row;
        bool match;
        do
        {
            yPos++;
            if (yPos < rows && yPos >= 0)
            {
                Tile nextTile = boardPositions[movedTile.column, yPos] as Tile;
                match = movedTile.IsSameType(nextTile);
                if (match)
                    Matches.Add(nextTile);
            }
            else
            {
                match = false;
            }

        } while (match);
        yPos = movedTile.row;
        do
        {
            yPos--;
            if (yPos < rows && yPos >= 0)
            {
                Tile nextTile = boardPositions[movedTile.column, yPos] as Tile;
                match = movedTile.IsSameType(nextTile);
                if (match)
                    Matches.Add(nextTile);
            }
            else
            {
                match = false;
            }
        } while (match);
        return Matches;
    }

    private List<Tile> HorizontalMatches(Tile movedTile)  //Will always return at least 1 item in the list.  movedTile is always added to the list.
    {
        List<Tile> Matches = new List<Tile>();
        Matches.Add(movedTile);
        int xPos = movedTile.column;
        bool match;
        do
        {
            xPos++;
            if (xPos < rows && xPos >= 0)
            {
                Tile nextTile = boardPositions[xPos, movedTile.row] as Tile;
                match = movedTile.IsSameType(nextTile);
                if (match)
                    Matches.Add(nextTile);
            }
            else
            {
                match = false;
            }

        } while (match);
        xPos = movedTile.column;
        do
        {
            xPos--;
            if (xPos < rows && xPos >= 0)
            {
                Tile nextTile = boardPositions[xPos, movedTile.row] as Tile;
                match = movedTile.IsSameType(nextTile);
                if (match)
                    Matches.Add(nextTile);
            }
            else
            {
                match = false;
            }
        } while (match);
        return Matches;
    }

    public void MoveTile(Tile mover)
    {
        mover.moving = true;
        StartCoroutine(mover.Activate());
        Vector3 end = new Vector3(mover.column, mover.row, 0f);
        StartCoroutine(Movement(mover, end));
    }

    protected IEnumerator Movement(Tile moving, Vector3 end)
    {
        float remainingDistance = Vector3.Distance(moving.transform.position, end);
        while (remainingDistance > float.Epsilon)
        {
            moving.transform.position = Vector3.MoveTowards(moving.transform.position, end, (moveTime * Time.deltaTime));
            remainingDistance = Vector3.Distance(moving.transform.position, end);
            yield return null;
       }
        moving.moving = false;
       yield break;
    }

    public void SwapTiles(Tile a, Tile b)
    {

    }
}
