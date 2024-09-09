using UniRx;
using UnityEngine;

namespace Scripts
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private Joystick _joyStick;
        [SerializeField] private PlayerView _playerView;

        private readonly CompositeDisposable _disposer = new();
        
        private void Start()
        {
            var playerPresenter = new PlayerPresenter(_joyStick, _playerView, _disposer);
        }
        
        private void OnDestroy()
        {
            _disposer.Dispose();
        }
    }
}