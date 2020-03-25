using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[Serializable]
public class TileSet
{
    public Color color;
    public Tile tileObj;
}
public class TileMapBuilder : MonoBehaviour
{

    public Texture2D levelMap;
    public int spacing;
    [Header("Config Color Tiles")]
    public List<TileSet> tiles;

    private Dictionary<Color, Tile> tilesDictionay;
    private Tilemap tileMap;

    private void Start()
    {
        Setup();
        GenerateLevel();
    }

    private void Setup()
    {
        tileMap = GetComponent<Tilemap>();

        if (tileMap == null)
        {
            Debug.LogError("Component TileMap not found...");
            return;
        }
        
        tilesDictionay = new Dictionary<Color, Tile>();
        foreach (TileSet tile in tiles)
        {
            if (!tilesDictionay.ContainsKey(tile.color))
            {
                tilesDictionay.Add(tile.color, tile.tileObj);
            }
            else
            {
                Debug.LogError("Duplicate colors found: " + tile.color);
            }
        }
    }

    private void GenerateLevel()
    {
        int with = levelMap.width;
        int heigth = levelMap.height;
        
        for (int x = 0; x < with; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                Vector3 relativeTilePos = new Vector3(x * spacing, y * spacing, 0);
                Vector3Int pos = Vector3Int.FloorToInt(relativeTilePos + transform.position); 
                
                Tile tile;
                if (tilesDictionay.TryGetValue(levelMap.GetPixel(x,y), out tile))
                {
                    tileMap.SetTile(pos, tile);
                }
                else
                {
                    Debug.Log($"Level texture could not find color in map: {x}, {y}");
                }
                
            }
        }
    }
}
