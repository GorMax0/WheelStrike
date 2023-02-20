using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FactoryTile))]
public class PoolTiles : MonoBehaviour
{
    private List<Tile> _tiles;
    private List<Tile> _disabledTiles;

    public void Initialize(out Vector3 startTilePosition, int stepPositionZ)
    {
        _tiles = GetComponent<FactoryTile>().CreateBeginningLevel(out startTilePosition, stepPositionZ);
        _disabledTiles = new List<Tile>(_tiles.Count);
    }

    public Tile EnableRandom()
    {
        int index = Random.Range(0, _disabledTiles.Count);

        _disabledTiles[index].gameObject.SetActive(true);
        _tiles.Add(_disabledTiles[index]);
        _disabledTiles.RemoveAt(index);

        return _tiles[_tiles.Count - 1];
    }

    public void DisableTile(int indexTile)
    {
        _tiles[indexTile].gameObject.SetActive(false);
        _disabledTiles.Add(_tiles[indexTile]);
        _tiles.RemoveAt(indexTile);
    }
}
