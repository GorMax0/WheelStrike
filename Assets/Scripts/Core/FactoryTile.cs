using System.Collections.Generic;
using UnityEngine;

public class FactoryTile : MonoBehaviour
{
    [SerializeField] private List<Tile> _prefabTiles;

    public List<Tile> CreateBeginningLevel(out Vector3 nextTilePosition, int stepPositionZ)
    {
        List<Tile> spawnedTiles = new List<Tile>(_prefabTiles.Count);
        int countSpawned = 1;
        nextTilePosition = Vector3.zero;

        for (int i = _prefabTiles.Count; i > 0; i--)
        {
            int indexTile = Random.Range(0, _prefabTiles.Count);
            nextTilePosition = new Vector3(0, 0, countSpawned++ * stepPositionZ);

            Tile tile = Instantiate(_prefabTiles[indexTile], nextTilePosition, Quaternion.identity, transform);
            _prefabTiles.RemoveAt(indexTile);
            spawnedTiles.Add(tile);
        }

        return spawnedTiles;
    }
}
