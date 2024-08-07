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
        private Tween _currentMoveTween;

        public Action<string, Color> onReachDestination;

        public void Start()
        {
            _agentGUID = Guid.NewGuid().ToString();
            _agentColor = GetRandomColor();
            agentMarker.material.color = _agentColor;
        }

        public void StartMovement(float newSpeed)
        {
            if (newSpeed <= 0) return;
            _speed = newSpeed;
            
            MoveToRandomPosition();
        }

        private void MoveToRandomPosition()
        {
            if (_speed <= 0) return;
            
            var targetPosition = GetRandomPosition();
            var position = transform.position;
            var distance = Vector3.Distance(position, targetPosition);
            var time = distance / _speed;
            
            Vector3 direction = (targetPosition - position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.DORotateQuaternion(targetRotation, 0.5f);
            _currentMoveTween = transform.DOMove(targetPosition, time).OnComplete(OnDestinationReached);
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
