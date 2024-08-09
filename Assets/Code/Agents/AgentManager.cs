using System;
using System.Collections.Generic;
using Code.Core;
using GD.MinMaxSlider;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Agents
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private float agentSpeed = 5;
        [SerializeField, Range(1, 5)] private float speedValueChange = 2;
        [SerializeField, MinMaxSlider(1,50) ] private Vector2 agentSpeedRange = new Vector2(1,50);
        [SerializeField] private Vector3 agentSpawnPoint;
        [SerializeField] private int poolSize = 10;

        private bool _isPaused;
        private IAgentService _agentService = AgentService.Instance;
        private ITickService _tickService = TickService.Instance;
        private List<Agent> _agents = new List<Agent>();
        private List<Agent> _agentPool = new List<Agent>();

        private void Awake()
        {
            _agentService.OnRequestAgentSpawn += SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn += RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn += ClearAllAgents;

            _tickService.OnRequestPause += () => SetPauseState(true);
            _tickService.OnRequestResume += () => SetPauseState(false);
            _tickService.OnIncreaseSpeed += () => AdjustAgentSpeed(speedValueChange);
            _tickService.OnDecreaseSpeed += () => AdjustAgentSpeed(-speedValueChange);
        }

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
                agent.gameObject.SetActive(false);
                _agentPool.Add(agent);
            }
            
            agentSpeed = Math.Clamp(agentSpeed, agentSpeedRange.x, agentSpeedRange.y);
            _tickService.RegisterAgentsSpeedChange(agentSpeed);
        }

        private void OnDestroy()
        {
            _agentService.OnRequestAgentSpawn -= SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn -= RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn -= ClearAllAgents;
            
            _tickService.OnRequestPause -= () => SetPauseState(true);
            _tickService.OnRequestResume -= () => SetPauseState(false);
            _tickService.OnIncreaseSpeed -= () => AdjustAgentSpeed(speedValueChange);
            _tickService.OnDecreaseSpeed -= () => AdjustAgentSpeed(-speedValueChange);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(agentSpawnPoint, 1f);
        }

        private void SpawnAgent()
        {
            Agent agentObject;
            
            if (_agentPool.Count > 0)
            {
                agentObject = _agentPool[0];
                _agentPool.RemoveAt(0);
                agentObject.gameObject.SetActive(true);
            }
            else
            {
                agentObject = Instantiate(agentPrefab).GetComponent<Agent>();
            }
                
            agentObject.onReachDestination += RegisterAgentReachDestination;
            agentObject.StartMovement(agentSpawnPoint, agentSpeed, _isPaused);
            
            _agents.Add(agentObject);
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void RemoveRandomAgent()
        {
            if (_agents.Count > 0)
            {
                int index = Random.Range(0, _agents.Count);
                Agent agent = _agents[index];
                _agents.RemoveAt(index);
                
                agent.onReachDestination -= RegisterAgentReachDestination;
                agent.gameObject.SetActive(false);
                _agentPool.Add(agent);
            }
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void ClearAllAgents()
        {
            foreach (var agent in _agents)
            {
                agent.onReachDestination -= RegisterAgentReachDestination;
                agent.gameObject.SetActive(false);
                _agentPool.Add(agent);
            }
            _agents.Clear();
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }

        private void RegisterAgentReachDestination(string agentGUID, Color agentColor)
        {
            _agentService.RegisterAgentReachDestination(agentGUID, agentColor);
        }
        
        private void SetPauseState(bool isPaused)
        {
            _isPaused = isPaused;
            _agents.ForEach(agent => 
            {
                if (isPaused)
                    agent.PauseMovement();
                else
                    agent.ResumeMovement();
            });
        }

        private void AdjustAgentSpeed(float speedChange)
        {
            agentSpeed = Mathf.Clamp(agentSpeed + speedChange, agentSpeedRange.x, agentSpeedRange.y);
            _agents.ForEach(x => x.ChangeMovementSpeed(agentSpeed, _isPaused));
            _tickService.RegisterAgentsSpeedChange(agentSpeed);
        }
    }
}
