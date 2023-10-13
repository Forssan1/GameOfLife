using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    internal float CellSize;
    SpriteRenderer spriteRenderer;
    public int cellState;
    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = alive;
        if (alive)
        {
            cellState = 1;
        }
        else
        {
            cellState = 0;
        }
    }
}