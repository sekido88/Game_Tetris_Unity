using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Tilemaps;

public class SandTouchManager : MonoBehaviour
{
    [SerializeField] private TilesData TileDatas;
    [SerializeField] private Tilemap _tileMap;

    private Tile _tile;
    private Camera _camera;

    [SerializeField] private float _fallSpeed = 0.25f;

    private bool[,] _cells;
    [SerializeField] private int row = 100;
    [SerializeField] private int col = 100;

    bool IsHold = false;

    float _fallTimer = 0f;

    List<Vector3Int> _fallingCells = new List<Vector3Int>();

    void Start()
    {
        _tile = TileDatas.GetTileData(TileData.Type.Purple).Tile;
        _camera = Camera.main;

        int offset = 1;
        _cells = new bool[row + offset, col + offset];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || IsHold)
        {
            IsHold = true;

            Vector3 mousePostion = Input.mousePosition;
            Vector3 mousePostionWorld = CommonUtils.ScreenToWorldPoint(mousePostion, _camera);
            Vector3Int cellPostion = _tileMap.WorldToCell(mousePostionWorld);

            if (IsValidPosition(cellPostion.x, cellPostion.y))
            {
                _cells[cellPostion.x, cellPostion.y] = true;
                _tileMap.SetTile(cellPostion, _tile);
                _fallingCells.Add(cellPostion);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsHold = false;
        }

        _fallTimer += Time.deltaTime;
        if (_fallTimer >= _fallSpeed)
        {
            UpdateCell();
            _fallTimer = 0f;
        }
    }

    void UpdateCell()
    {
        for (int index = 0; index < _fallingCells.Count; index++)
        {
            var pos = _fallingCells[index];
            int i = pos.x;
            int j = pos.y;
            if (!_cells[i, j] == true) continue;

            if (IsValidPosition(i, j - 1) && _cells[i, j - 1] == false)
            {
                _cells[i, j - 1] = true;
                SwapTile(i, j, i, j - 1);
                _cells[i, j] = false;
                _fallingCells[index] = new Vector3Int(i, j - 1, 0);
            }
            else if (IsValidPosition(i + 1, j - 1) && _cells[i + 1, j - 1] == false)
            {
                _cells[i, j] = false;
                SwapTile(i, j, i + 1, j - 1);
                _cells[i + 1, j - 1] = true;
                _fallingCells[index] = new Vector3Int(i + 1, j - 1, 0);
            }
            else if (IsValidPosition(i - 1, j - 1) && _cells[i - 1, j - 1] == false)
            {
                _cells[i, j] = false;
                SwapTile(i, j, i - 1, j - 1);
                _cells[i - 1, j - 1] = true;
                _fallingCells[index] = new Vector3Int(i - 1, j - 1, 0);
            }
            else
            {
                _fallingCells.RemoveAt(index);
                index--;
            }
        }


    }

    private void SwapTile(int x, int y, int u, int v)
    {
        _tileMap.SetTile(new Vector3Int(x, y, 0), null);
        _tileMap.SetTile(new Vector3Int(u, v, 0), _tile);
    }


    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x <= row && y >= 0 && y <= col;
    }

}