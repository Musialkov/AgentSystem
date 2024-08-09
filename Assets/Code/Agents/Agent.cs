using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Agents
{
    public class Agent : MonoBehaviour
    {
        private float _speed;
        private string _agentGUID;
        private Color _agentColor;
        private Tween _currentRotationTween;
        private Tween _currentMoveTween;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        public Action<string, Color> onReachDestination;

        public void OnEnable()
        {
            _agentGUID = Guid.NewGuid().ToString();
            _agentColor = GetRandomColor();
        }

        private void OnDisable()
        {
            transform.position = new Vector3(0, 0, 0);
            KillTweens();
        }

        public void StartMovement(Vector3 spawnPoint, float newSpeed, bool isPaused)
        {
            if (!IsValidSpeed(newSpeed)) return;
            _speed = newSpeed;
            
            transform.position = spawnPoint;
            transform.rotation = Quaternion.identity;
            
            MoveToRandomPosition();
            if(isPaused) PauseMovement();
        }

        public void PauseMovement()
        {
            _currentRotationTween?.Pause();
            _currentMoveTween?.Pause();
        }
        
        public void ResumeMovement()
        {
            _currentRotationTween?.Play();
            _currentMoveTween?.Play();
        }

        public void ChangeMovementSpeed(float newSpeed, bool isPaused)
        {
            if (!IsValidSpeed(newSpeed)) return;

            _speed = newSpeed;
            UpdateMovement();
            
            if(isPaused) PauseMovement();
        }

        private void MoveToRandomPosition()
        {
            if (!IsValidSpeed(_speed)) return;
            
            _targetPosition = GetRandomPosition();
            UpdateMovement();
        }

        private bool IsValidSpeed(float speed)
        {
            return speed > 0;
        }

        private void UpdateMovement()
        {
            KillTweens();

            var position = transform.position;
            Vector3 direction = (_targetPosition - position).normalized;
            _targetRotation = Quaternion.LookRotation(direction);
            
            float remainingDistance = Vector3.Distance(position, _targetPosition);
            float movementTime = remainingDistance / _speed;

            _currentRotationTween = transform.DORotateQuaternion(_targetRotation, 0.5f);
            _currentMoveTween = transform.DOMove(_targetPosition, movementTime).OnComplete(OnDestinationReached);
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
            return new Color(Random.value, Random.value, Random.value);
        }

        private void KillTweens()
        {
            _currentRotationTween?.Kill();
            _currentMoveTween?.Kill();
        }
    }
}
