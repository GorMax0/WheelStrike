using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    public class Wall : MonoBehaviour
    {
        [SerializeField] private GameObject _wallSubstrate;
        [SerializeField] private List<Brick> _bricks = new List<Brick>();
        [SerializeField] private bool _isCreate;

        private AudioSource _audioSource;
        private WallCreator _creator;

        [field: SerializeField] public int Reward { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (_isCreate == true)
                _bricks = _creator.Create();
        }

        public void Collide()
        {
            PlaySound();
            _wallSubstrate.SetActive(false);
            _bricks.ForEach(brick => brick.gameObject.SetActive(true));
        }
        private void PlaySound() => _audioSource.Play();

        public void StopMoveBricks() => _bricks.ForEach(brick => brick.StopMove());
    }
}