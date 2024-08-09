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
        private Queue<Agent> _agentPool = new Queue<Agent>();

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
            InitializeAgentPool();
            
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
        
        private void InitializeAgentPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
                agent.gameObject.SetActive(false);
                _agentPool.Enqueue(agent);
            }
        }

        private void SpawnAgent()
        {
            Agent agentObject = GetPooledAgent();
                
            agentObject.onReachDestination += RegisterAgentReachDestination;
            agentObject.StartMovement(agentSpawnPoint, agentSpeed, _isPaused);
            
            _agents.Add(agentObject);
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private Agent GetPooledAgent()
        {
            if (_agentPool.Count > 0)
            {
                Agent agent = _agentPool.Dequeue();
                agent.gameObject.SetActive(true);
                return agent;
            }
            else
            {
                return Instantiate(agentPrefab).GetComponent<Agent>();
            }
        }
        
        private void RemoveRandomAgent()
        {
            if (_agents.Count > 0)
            {
                int index = Random.Range(0, _agents.Count);
                Agent agent = _agents[index];
                _agents.RemoveAt(index);
                ReturnAgentToPool(agent);
            }
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void ClearAllAgents()
        {
            foreach (var agent in _agents)
            {
                ReturnAgentToPool(agent);
            }
            _agents.Clear();
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void ReturnAgentToPool(Agent agent)
        {
            agent.onReachDestination -= RegisterAgentReachDestination;
            agent.gameObject.SetActive(false);
            _agentPool.Enqueue(agent);
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
