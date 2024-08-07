using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Agents
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private float speed;
        private string _agentGUID;

        private void Start()
        {
            _agentGUID = Guid.NewGuid().ToString();
            MoveToRandomPosition();
        }

        private void MoveToRandomPosition()
        {
            if (speed <= 0) return;
            
            var targetPosition = GetRandomPosition();
            var position = transform.position;
            var distance = Vector3.Distance(position, targetPosition);
            var time = distance / speed;
            
            Vector3 direction = (targetPosition - position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            transform.DORotateQuaternion(targetRotation, 0.5f);
            transform.DOMove(targetPosition, time).OnComplete(OnDestinationReached);
        }

        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(-10f, 10f);
            float z = Random.Range(-10f, 10f);
            return new Vector3(x, 0, z);
        }

        private void OnDestinationReached()
        {
            MoveToRandomPosition();
        }
    }
}
