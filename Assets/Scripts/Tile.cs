using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile : MonoBehaviour
{
    public int column;
    public int row;
    public string type;
    public float blinkSpeed;
    public bool selected = false;
    public bool moving = false;
    public Sprite choice;
    public Sprite stable;
    public Sprite inverse;
    //public GameObject GameManager;

    private SpriteRenderer SR;

    public void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        blinkSpeed = blinkSpeed / 8;
    }

    public void Update()
    {

    }

    public IEnumerator Activate()
    {
        while(moving)
        {
            SR.sprite = inverse;
            yield return new WaitForSeconds(blinkSpeed);
            SR.sprite = choice;
            yield return new WaitForSeconds(blinkSpeed);
        }
        SR.sprite = stable;
        yield break;
    }

    public bool IsSameType(Tile otherTile)
    {
        if (otherTile == null)
            return false;

        return string.Compare(this.type, (otherTile as Tile).type) == 0;
    }

    public void Assign(int newColumn, int newRow)
    {
        column = newColumn;
        row = newRow;
    }

    public static void SwapColumnRow(Tile a, Tile b)
    {
        int temp = a.row;
        a.row = b.row;
        b.row = temp;

        temp = a.column;
        a.column = b.column;
        b.column = temp;
    }

    public void OnMouseDown()
    {
        if (!selected)
        {
            selected = true;
            SR.sprite = choice;
        }
        else
        {
            selected = false;
            SR.sprite = stable;
        }
    }

}
