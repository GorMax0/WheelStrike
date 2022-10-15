using UnityEngine;
using Zenject;
using Obi;
using Services.GameStates;

namespace Core
{
    [RequireComponent(typeof(ObiParticleAttachment))]
    public class Rope : MonoBehaviour
    {
        [SerializeField] private Movement _jointObject;

        private ObiParticleAttachment[] _joints;
        private GameStateService _gameStateService;

        private void Awake()
        {
            _joints = GetComponents<ObiParticleAttachment>();
        }

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        [Inject]
        private void Construct(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGameRunning()
        {
            foreach (ObiParticleAttachment joint in _joints)
            {
                if (joint.target == _jointObject.transform)
                    joint.enabled = false;
            }
        }
    }
}