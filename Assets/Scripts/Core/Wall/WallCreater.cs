using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Wall
{
    [RequireComponent(typeof(BoxCollider))]
    public class WallCreater : MonoBehaviour
    {
        [SerializeField] private Brick _brickTemplate;
        [SerializeField] private int _width = 8;
        [SerializeField] private int _height = 70;

        private BoxCollider _boxCollider;
        private List<Brick> _bricks = new List<Brick>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                Create();
        }

        public void EnableGravityBricks()
        {
            foreach (Brick brick in _bricks)
            {
                Debug.Log("is work!");
                brick.EnableGravity();
            }
        }

        private void Create()
        {


            GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick);

            float halfWidthBrick = widthBrick / 2;
            float halfHeightBrick = heightBrick / 2;

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    float nextBrickPositionX = j == 0 ? transform.position.x + halfWidthBrick : transform.position.x + halfWidthBrick + widthBrick * j;
                    float nextBrickPositionY = i == 0 ? transform.position.y + halfHeightBrick : transform.position.y + halfHeightBrick + heightBrick * i;
                    Vector3 nextPosition = new Vector3(nextBrickPositionX, nextBrickPositionY, transform.position.z);

                    Brick brick = Instantiate(_brickTemplate, nextPosition, _brickTemplate.transform.rotation, transform);
                    _bricks.Add(brick);
                }
            }

            SetColliderParameters(widthBrick, heightBrick, lengthBrick);
        }

        private void GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick)
        {
            if (_brickTemplate.TryGetComponent(out MeshFilter meshFilterBrick) == false)
                throw new NullReferenceException($"{typeof(WallCreater)}: GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick): " +
                    $"Brick Template named {_brickTemplate.name} does not have component MeshFilter.");

            widthBrick = meshFilterBrick.sharedMesh.bounds.size.x;
            heightBrick = meshFilterBrick.sharedMesh.bounds.size.z;
            lengthBrick = meshFilterBrick.sharedMesh.bounds.size.y;
        }

        private void SetColliderParameters(float widthBrick, float heightBrick, float lengthBrick)
        {
            float widthCollider = widthBrick * _width;
            float heightCollider = heightBrick * _height;
            float halfWidthCollider = widthCollider / 2;
            float halfHeightCollider = heightCollider / 2;

            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.size = new Vector3(widthCollider, heightCollider, lengthBrick);
            _boxCollider.center = new Vector3(halfWidthCollider, halfHeightCollider, 0f);
        }
    }
}