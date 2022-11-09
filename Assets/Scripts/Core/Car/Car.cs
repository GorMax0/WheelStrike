using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Car : MonoBehaviour
    {
        private MeshRenderer _mainMesh;
        private MeshRenderer[] _secondaryMeshs;

        private void Awake()
        {
            _mainMesh = GetComponent<MeshRenderer>();
            _secondaryMeshs = GetComponentsInChildren<MeshRenderer>();
        }
    }
}
