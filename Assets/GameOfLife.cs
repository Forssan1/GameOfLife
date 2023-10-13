using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameOfLife : MonoBehaviour
{
    Cell[,] cells;
    float cellSize = 0.25f; // Size of our cells
    int numberOfColumns, numberOfRows;
    int neighbourCount;
    public GameObject cellPrefab;
    public Slider speedSlider;
    public Slider zoomSlider;
    public GameObject pauseMenu;
    bool paused;
    bool acorn1Bool = false;
    bool acorn2Bool = false;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        numberOfColumns = Mathf.FloorToInt((Camera.main.orthographicSize * Camera.main.aspect * 2) / cellSize);
        numberOfRows = Mathf.FloorToInt((Camera.main.orthographicSize * 2) / cellSize);
        cells = new Cell[numberOfColumns, numberOfRows];

        for (int y = 0; y < numberOfRows; y++)
        {
            for (int x = 0; x < numberOfColumns; x++)
            {
                //Adds all cells
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize * Camera.main.aspect, y * cellSize - Camera.main.orthographicSize);
                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                
                cells[x, y] = newCell.GetComponent<Cell>();
                cells[x, y].UpdateStatus();  
            }
        }
    }

    void Update()
    {
        //Sets the camera size to the zoom slider value
        Camera.main.orthographicSize = zoomSlider.value;

        MouseClick();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Application.targetFrameRate = 60;
                pauseMenu.SetActive(true);   
            }
            else
            {
                pauseMenu.SetActive(false);
            }

        }
        if (!paused)
        {
            //Sets the frame rate to the speed slider value
            Application.targetFrameRate = (int)speedSlider.value;

            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    neighbourCount = 0;
                    GetNeighbor(x, y);

                    //checks the rules
                    if (neighbourCount < 2)
                    {
                        cells[x, y].alive = false; // Dead

                    }
                    else if (neighbourCount > 3)
                    {
                        cells[x, y].alive = false; // Dead

                    }
                    else if (neighbourCount == 3)
                    {
                        cells[x, y].alive = true; // Alive

                    }
                }
            }

            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColumns; x++)
                {
                    cells[x, y].UpdateStatus();
                }
            } 
        }
    }

    void GetNeighbor(int x, int y)
    {
        //Loops for all types of offsets for each neighbour
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {

                if (xOffset == 0 && yOffset == 0)
                {
                    //skips the original cell
                    continue; 
                }

                int neighborX = x + xOffset;
                int neighborY = y + yOffset;

                // checks so that the cell is within the borders
                if (neighborX >= 0 && neighborX < numberOfColumns && neighborY >= 0 && neighborY < numberOfRows)
                {
                    if (cells[neighborX, neighborY].cellState == 1)
                    {

                        neighbourCount++;
                    }
                }
            }
        }

    }
    void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Finds the cell position based on the position of the mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int cellXPosition = Mathf.FloorToInt((mousePosition.x + 15 * Camera.main.aspect) / cellSize);
            int cellYPosition = Mathf.FloorToInt((mousePosition.y + 15) / cellSize);

            // checks so that the cell is within the borders
            if (cellXPosition >= 0 && cellXPosition < numberOfColumns && cellYPosition >= 270 / zoomSlider.value && cellYPosition < numberOfRows)
            {
                if (acorn1Bool)
                {
                    acorn1(cellXPosition, cellYPosition);
                }

                else if (acorn2Bool)
                {
                    acorn2(cellXPosition, cellYPosition);
                }

                else
                {
                    if (cells[cellXPosition, cellYPosition].alive == false)
                    {
                        cells[cellXPosition, cellYPosition].alive = true;
                        cells[cellXPosition, cellYPosition].cellState = 1;
                        cells[cellXPosition, cellYPosition].UpdateStatus();
                    }

                    else if (cells[cellXPosition, cellYPosition].alive == true)
                    {
                        cells[cellXPosition, cellYPosition].alive = false;
                        cells[cellXPosition, cellYPosition].cellState = 0;
                        cells[cellXPosition, cellYPosition].UpdateStatus();
                    }
                }

            }
        }
    }

    private void acorn1(int cellXPosition, int cellYPosition)
    {
        //coordinates for each cell within the acorn
        int[] acornX = { 0, 1, 1, 3, 4, 5, 6 };
        int[] acornY = { 0, 0, 2, 1, 0, 0, 0 };

        for (int i = 0; i < acornX.Length; i++)
        {
            int targetX = cellXPosition + acornX[i];
            int targetY = cellYPosition + acornY[i];

            if (targetX >= 0 && targetX < numberOfColumns && targetY >= 0 && targetY < numberOfRows)
            {
                if (cells[targetX, targetY].alive == false)
                {
                    cells[targetX, targetY].alive = true;
                    cells[targetX, targetY].cellState = 1;
                    cells[targetX, targetY].UpdateStatus();
                }
            }
        }
    }

    private void acorn2(int cellXPosition, int cellYPosition)
    {
        //coordinates for each cell within the acorn
        int[] acornX = { 0, 1, 0, 1,10,10, 10, 11, 11, 12, 12, 13, 13, 14, 15,15,16,16,16,17,20,20,20,21,21,21,22,22,24,24,24,24,34,34,35,35};
        int[] acornY = { 0, 0, 1, 1, 0, 1, -1, 2, -2, 3, -3, 3, -3, 0, 2, -2, 0, 1, -1, 0, 1, 2, 3, 1, 2, 3, 0, 4, 0, -1, 4, 5, 2, 3, 2, 3};

        for (int i = 0; i < acornX.Length; i++)
        {
            int targetX = cellXPosition + acornX[i];
            int targetY = cellYPosition + acornY[i];

            if (targetX >= 0 && targetX < numberOfColumns && targetY >= 0 && targetY < numberOfRows)
            {
                if (cells[targetX, targetY].alive == false)
                {
                    cells[targetX, targetY].alive = true;
                    cells[targetX, targetY].cellState = 1;
                    cells[targetX, targetY].UpdateStatus();
                }
            }
        }
    }

    //Acorn managers for the buttons
    public void Acorn1Manager()
    {
        acorn1Bool = !acorn1Bool;
        acorn2Bool = false;

    }
    public void Acorn2Manager()
    {
        acorn2Bool = !acorn2Bool;
        acorn1Bool = false;

    }
}