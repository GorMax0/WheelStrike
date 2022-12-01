using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Brick _brickTemplate;
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 50;

        private BoxCollider _boxCollider;
        private List<Brick> _bricks = new List<Brick>();

        [field: SerializeField] public int Reward { get; private set; }

        public void Create()
        {
            GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick);

            float halfWidthBrick = widthBrick / 2;
            float halfHeightBrick = heightBrick / 2;
            float nextBrickPositionX;
            float nextBrickPositionY;
            bool isEvenRow;

            for (int i = 0; i < _height; i++)
            {
                nextBrickPositionY = transform.position.y + halfHeightBrick + heightBrick * i;
                isEvenRow = i % 2 == 0;

                for (int j = 0; j < _width; j++)
                {
                    nextBrickPositionX = isEvenRow ? transform.position.x + halfWidthBrick + widthBrick * j : transform.position.x + widthBrick * j;
                    Vector3 nextPosition = new Vector3(nextBrickPositionX, nextBrickPositionY, transform.position.z);

                    Brick brick = Instantiate(_brickTemplate, nextPosition, _brickTemplate.transform.rotation, transform);
                    _bricks.Add(brick);
                }
            }

            SetColliderParameters(widthBrick, heightBrick, lengthBrick);
        }

        public void EnableGravityBricks()
        {
            foreach (Brick brick in _bricks)
            {
                brick.EnableGravity();
            }
        }

        private void GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick)
        {
            if (_brickTemplate.TryGetComponent(out MeshFilter meshFilterBrick) == false)
                throw new NullReferenceException($"{GetType()}: GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick): " +
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