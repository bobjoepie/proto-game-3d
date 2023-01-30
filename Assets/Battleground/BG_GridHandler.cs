using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BG_GridHandler : MonoBehaviour
{
    public int gridX = 10;
    public int gridY = 10;
    public BG_GridCell gridCellPrefab;
    public BG_GridCell[][] gridCells;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Battleground");
        mainCamera = Camera.main;
        gridCells = new BG_GridCell[gridX][];
        for (int i = 0; i < gridX; i++)
        {
            gridCells[i] = new BG_GridCell[gridY];
        }

        for (int j = 0; j < gridY; j++)
        {
            for (int i = 0; i < gridY; i++)
            {
                var pos = new Vector3(i+0.5f, 2, j+0.5f);
                var gridCell = Instantiate(gridCellPrefab, transform);
                gridCell.name = $"Cell ({i},{j})";
                gridCell.transform.localPosition = pos;
                gridCells[i][j] = gridCell;
            }
        }
    }
}
