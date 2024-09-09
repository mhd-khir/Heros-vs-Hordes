using UnityEngine;

namespace Scripts
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField, Range(float.Epsilon, 5f)]
        private float _speed;

        public void Move(Vector2 direction)
        {
            var oldPosition = transform.position;
            transform.position = Vector3.Lerp(oldPosition, oldPosition + (Vector3)direction * _speed,
                Time.deltaTime);
        }

        public Vector2 Position
        {
            get => transform.position;
        }
    }
}