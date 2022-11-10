using System.Collections.Generic;
using UnityEngine;
using Services.GameStates;

namespace Core
{
    public class CarFactory : MonoBehaviour
    {
        [SerializeField] private List<CarSpawnPoint> _spawnPoints;
        [SerializeField] private List<Car> _prefabs;
        [SerializeField] private List<CarColor> _colors;

        public void CreateCars(GameStateService gameStateService)
        {
            int indexCar;
            int indexColor;

            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                indexCar = Random.Range(0, _prefabs.Count);
                indexColor = Random.Range(0, _colors.Count);

                Car car = Instantiate(_prefabs[indexCar], _spawnPoints[i].transform.position, _spawnPoints[i].transform.rotation, transform);
                car.Initialize(gameStateService, _colors[indexColor].Material);

                Destroy(_spawnPoints[i].gameObject);
                _spawnPoints.RemoveAt(i);
                i--;
            }
        }
    }
}