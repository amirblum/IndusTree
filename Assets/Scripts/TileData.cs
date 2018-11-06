using System.Collections;
using System.Collections.Generic;

public class TileData
{
    public enum TileType
    {
        Empty = -1,
        Organic = 0,
        Mechanic = 1,
    }

    public TileType tileType;

    public TileData()
    {
        tileType = TileType.Empty;
    }
}
