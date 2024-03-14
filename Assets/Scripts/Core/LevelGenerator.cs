using Core.Wheel;
using UnityEngine;

namespace Core
{
    public class LevelGenerator : MonoBehaviour
    {
        private const int StepPositionZ = 20;
        private const int PassedTile = 8;
        [SerializeField] private Tile[] _tiles;
        [SerializeField] private PoolTiles _pool;

        private InteractionHandler _interactionHandler;
        private int _countPassedTile;
        private Vector3 _currentTilePosition;

        private void FixedUpdate()
        {
            if (_countPassedTile < PassedTile)
                return;

            ActivateNextTiles();
        }

        private void OnDestroy()
        {
            _interactionHandler.TriggeredNextTile -= OnTirggeredNextTile;
        }

        public void Initialize(InteractionHandler interactionHandler)
        {
            _interactionHandler = interactionHandler;
            _interactionHandler.TriggeredNextTile += OnTirggeredNextTile;
            _pool.Initialize(out _currentTilePosition, StepPositionZ);
        }

        private void ActivateNextTiles()
        {
            for (int i = 0; i < PassedTile; i++)
            {
                Tile tile = _pool.EnableRandom();

                _currentTilePosition = new Vector3(
                    _currentTilePosition.x,
                    _currentTilePosition.y,
                    _currentTilePosition.z + StepPositionZ);

                tile.SetPosition(_currentTilePosition);
            }

            _countPassedTile = 0;
        }

        private void OnTirggeredNextTile()
        {
            _pool.DisableTile(0);
            _countPassedTile++;
        }
    }
}