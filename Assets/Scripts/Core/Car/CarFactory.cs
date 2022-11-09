using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CarFactory : MonoBehaviour
    {
        [SerializeField] private List<CarSpawnPoint> _spawnPoints;
        [SerializeField] private List<Car> _prefabs;
        [SerializeField] private List<Material> _materials;

        public void Initialize()
        {
            
        }
    }
}