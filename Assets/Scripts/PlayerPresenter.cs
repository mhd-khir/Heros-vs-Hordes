using UniRx;

namespace Scripts
{
    public class PlayerPresenter
    {
        public PlayerPresenter(Joystick joystick, PlayerView playerView, CompositeDisposable disposer)
        {
            joystick.OnInput.Subscribe(playerView.Move).AddTo(disposer);
        }
    }
}