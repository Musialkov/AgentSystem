using DG.Tweening;
using UnityEngine;

namespace Code.Agents
{
    public class Agent : MonoBehaviour
    {
        private string _agentGUID;

        private void Start()
        {
            _agentGUID = System.Guid.NewGuid().ToString();
            MoveToRandomPosition();
        }

        private void MoveToRandomPosition()
        {
            Vector3 targetPosition = GetRandomPosition();
            transform.DOMove(targetPosition, Random.Range(1f, 5f)).OnComplete(OnDestinationReached);
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
