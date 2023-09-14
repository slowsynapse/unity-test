using UnityEngine;

namespace View
{
    [RequireComponent(typeof(RectTransform))]
    public class LerpTo : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private Vector3 _targetPosition;
        private float _targetRotation;
        private float _currentRotation = 0;
        private bool _rotateY;

        public int CurrentRotation
        {
            set { _currentRotation = value; }
        }

        public void SetTarget(Vector3 position, float rotation, bool rotateY = false)
        {
            _targetPosition = position;
            _targetRotation = rotation;
            _rotateY = rotateY;
        }

        void Update()
        {
            transform.localPosition += (_targetPosition - transform.localPosition) * (speed * Time.deltaTime);

            _currentRotation += (_targetRotation - _currentRotation) * Time.deltaTime * speed;
            if (_rotateY)
                transform.localRotation = Quaternion.Euler(0, _currentRotation, 0);
            else
                transform.localRotation = Quaternion.Euler(0, 0, _currentRotation);
        }

        public void ResetRotation()
        {
            _targetRotation = 0;
        }
    }
}