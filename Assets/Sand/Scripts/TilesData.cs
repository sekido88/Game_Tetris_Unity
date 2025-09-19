
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


using Type = TileData.Type;

[System.Serializable]
public class TileData
{
    public enum Type
    {
        Blue,
        Cyan,
        Ghost,
        Green,
        Red,
        Yellow,
        Purple,
        Orange,
        Grid
    }
    public Type CurrentType;
    public Tile Tile;
}

[CreateAssetMenu(fileName = "TilesData", menuName = "Tilemap/Tiles Data", order = 0)]
public class TilesData : ScriptableObject
{
    public List<TileData> L_TileDatas = new();
    public TileData GetTileData(Type type)
    {
        int index = (int)type;

        if (index >= 0 && index < L_TileDatas.Count)
        {
            return L_TileDatas[index];
        }

        return null;
    }
}