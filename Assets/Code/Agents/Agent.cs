using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Agents
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private MeshRenderer agentMarker;
        
        private float _speed;
        private string _agentGUID;
        private Color _agentColor;
        private Tween _currentRotationTween;
        private Tween _currentMoveTween;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        public Action<string, Color> onReachDestination;

        public void Start()
        {
            _agentGUID = Guid.NewGuid().ToString();
            _agentColor = GetRandomColor();
            agentMarker.material.color = _agentColor;
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }

        public void StartMovement(float newSpeed, bool isPaused)
        {
            if (newSpeed <= 0) return;
            _speed = newSpeed;
            
            MoveToRandomPosition();
            if(isPaused) PauseMovement();
        }

        public void PauseMovement()
        {
            _currentRotationTween.Pause();
            _currentMoveTween.Pause();
        }
        
        public void ResumeMovement()
        {
            _currentRotationTween.Play();
            _currentMoveTween.Play();
        }

        public void ChangeMovementSpeed(float newSpeed, bool isPaused)
        {
            if (newSpeed <= 0) return;

            _speed = newSpeed;
            var position = transform.position;
            float remainingDistance = Vector3.Distance(position, _targetPosition);
            Vector3 direction = (_targetPosition - position).normalized;
            float newTime = remainingDistance / _speed;
            
            _targetRotation = Quaternion.LookRotation(direction);
            
            _currentMoveTween.Kill();
            _currentMoveTween = transform.DOMove(_targetPosition, newTime).OnComplete(OnDestinationReached).Play();
            _currentRotationTween.Kill();
            _currentRotationTween = transform.DORotateQuaternion(_targetRotation, 0.5f);
            
            if(isPaused) PauseMovement();
        }
        
        private void MoveToRandomPosition()
        {
            if (_speed <= 0) return;
            
            _targetPosition = GetRandomPosition();
            var position = transform.position;
            var distance = Vector3.Distance(position, _targetPosition);
            var time = distance / _speed;
            
            Vector3 direction = (_targetPosition - position).normalized;
            _targetRotation = Quaternion.LookRotation(direction);
            
            _currentRotationTween = transform.DORotateQuaternion(_targetRotation, 0.5f);
            _currentMoveTween = transform.DOMove(_targetPosition, time).OnComplete(OnDestinationReached);
        }

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(-10f, 10f);
            float z = Random.Range(-10f, 10f);
            return new Vector3(x, 0, z);
        }

        private void OnDestinationReached()
        {
            onReachDestination?.Invoke(_agentGUID, _agentColor);
            MoveToRandomPosition();
        }
        
        private Color GetRandomColor()
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            return randomColor;
        }
    }
}
