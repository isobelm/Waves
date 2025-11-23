using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
// using System.Runtime;
public class TilemapGenerator : MonoBehaviour
{
    public Tilemap inputMap;
    public Tilemap ouputMap;
    public TileBase[] tileArray;
    public Bounds bounds;

    private Dictionary<int, Func<TileBase>> tilePicker;

    void Start()
    {
        InitialiseTilePicker();
        bounds = inputMap.localBounds;
        GenerateTilemap();
    }

    void InitialiseTilePicker()
	{
		tilePicker = new Dictionary<int, Func<TileBase>>()
        {
            {0b0010, () => tileArray[0]},
            {0b0101, () => tileArray[1]},
            {0b1011, () => tileArray[2]},
            {0b0011, () => tileArray[3]},
            {0b1001, () => tileArray[4]},
            {0b0111, () => tileArray[5]},
            {0b1111, CentreTile},
            {0b1110, () => tileArray[7]},
            {0b0100, () => tileArray[8]},
            {0b1100, () => tileArray[9]},
            {0b1101, () => tileArray[10]},
            {0b1010, () => tileArray[11]},
            {0b0001, () => tileArray[13]},
            {0b0110, () => tileArray[14]},
            {0b1000, () => tileArray[15]},
        };

	}

    void GenerateTilemap()
    {
        for (int x = (int)bounds.min.x; x < (int)bounds.max.x; x++)
        {
            for (int y = (int)bounds.min.y; y < (int)bounds.max.y; y++)
            {
                Vector3Int position = new(x, y, 0);

                int whichTile = 0b1111;
                if (!inputMap.GetTile(position))
                    whichTile ^= 0b1000;
                if (!inputMap.GetTile(new Vector3Int(x + 1, y, 0)))
                    whichTile ^= 0b0100;
                if (!inputMap.GetTile(new Vector3Int(x, y - 1, 0)))
                    whichTile ^= 0b0010;
                if (!inputMap.GetTile(new Vector3Int(x + 1, y - 1, 0)))
                    whichTile ^= 0b0001;


                if (whichTile != 0)
                {
                    ouputMap.SetTile(position, tilePicker[whichTile]());
                }
            }
        }
    }
    
    TileBase CentreTile()
    {
        TileBase tile;
        if (UnityEngine.Random.Range(0f, 1f) > 0.9f)
        {
            tile = tileArray[12];
        }
        else
            tile = tileArray[6];
        return tile;
	}
}