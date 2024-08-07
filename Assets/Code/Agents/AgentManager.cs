using System;
using System.Collections.Generic;
using Code.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Agents
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private float agentSpeed = 5;
        
        private IAgentService _agentService = AgentService.Instance;
        private List<Agent> _agents = new List<Agent>();

        private void Start()
        {
            _agentService.OnRequestAgentSpawn += SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn += RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn += ClearAllAgents;
        }

        private void OnDestroy()
        {
            _agentService.OnRequestAgentSpawn -= SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn -= RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn -= ClearAllAgents;
        }

        private void SpawnAgent()
        {
            Agent agentObject = Instantiate(agentPrefab).GetComponent<Agent>();
            agentObject.onReachDestination += RegisterAgentReachDestination;
            agentObject.StartMovement(agentSpeed);
            
            _agents.Add(agentObject.GetComponent<Agent>());
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void RemoveRandomAgent()
        {
            if (_agents.Count > 0)
            {
                int index = Random.Range(0, _agents.Count);
                Agent agent = _agents[index];
                _agents.RemoveAt(index);
                Destroy(agent.gameObject);
            }
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }
        
        private void ClearAllAgents()
        {
            foreach (var agent in _agents)
            {
                Destroy(agent.gameObject);
            }
            _agents.Clear();
            _agentService.RegisterAgentsNumberChange(_agents.Count);
        }

        private void RegisterAgentReachDestination(string agentGUID, Color agentColor)
        {
            _agentService.RegisterAgentReachDestination(agentGUID, agentColor);
        }
    }
}
