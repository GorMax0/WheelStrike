using UnityEngine;

namespace Trail
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TrailFX : MonoBehaviour
    {
        private ParticleSystem _trail;
        private bool _isBought;
        private bool _isSelected = true;

        public bool IsSelected => _isSelected;

        private void Awake()
        {
            _trail = GetComponent<ParticleSystem>();
        }


        public void Play() => _trail.Play();

        public void Stop() => _trail.Stop();
    }
}