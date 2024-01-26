using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class WallCreator : MonoBehaviour
    {
        [SerializeField] private Brick _brickTemplate;
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 34;

        private const int Half = 2;

        private BoxCollider _boxCollider;

        public List<Brick> Create()
        {
            List<Brick> bricks = new List<Brick>();

            GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick);

            float halfWidthBrick = widthBrick / Half;
            float halfHeightBrick = heightBrick / Half;
            float nextBrickPositionX;
            float nextBrickPositionY;
            bool isEvenRow;
            Transform parentBrick = transform;

            for (int i = 0; i < _height; i++)
            {
                nextBrickPositionY = transform.position.y + halfHeightBrick + heightBrick * i;
                isEvenRow = i % Half == 0;

                for (int j = 0; j < _width; j++)
                {
                    nextBrickPositionX = isEvenRow ? transform.position.x + halfWidthBrick + widthBrick * j :
                        transform.position.x + widthBrick * j;

                    Vector3 nextPosition = new Vector3(nextBrickPositionX, nextBrickPositionY, parentBrick.position.z);

                    Brick brick = Instantiate(
                        _brickTemplate,
                        nextPosition,
                        _brickTemplate.transform.rotation,
                        parentBrick);

                    brick.gameObject.SetActive(false);
                    bricks.Add(brick);
                }
            }

            SetColliderParameters(widthBrick, heightBrick, lengthBrick);

            return bricks;
        }

        private void GetSizeBrick(out float widthBrick, out float heightBrick, out float lengthBrick)
        {
            if (_brickTemplate.TryGetComponent(out MeshFilter meshFilterBrick) == false)
                throw new NullReferenceException(
                    $"{GetType()}: GetSizeBrick(out float widthBrick, out float heightBrick, "
                    + $"out float lengthBrick): Brick Template named {_brickTemplate.name} "
                    + $"does not have component MeshFilter.");

            widthBrick = meshFilterBrick.sharedMesh.bounds.size.x;
            heightBrick = meshFilterBrick.sharedMesh.bounds.size.z;
            lengthBrick = meshFilterBrick.sharedMesh.bounds.size.y;
        }

        private void SetColliderParameters(float widthBrick, float heightBrick, float lengthBrick)
        {
            float widthCollider = widthBrick * _width;
            float heightCollider = heightBrick * _height;
            float halfWidthCollider = widthCollider / Half;
            float halfHeightCollider = heightCollider / Half;

            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.size = new Vector3(widthCollider, heightCollider, lengthBrick);
            _boxCollider.center = new Vector3(halfWidthCollider, halfHeightCollider, 0f);
        }
    }
}