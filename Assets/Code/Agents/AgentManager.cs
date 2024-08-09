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

        private bool _isPaused;
        private IAgentService _agentService = AgentService.Instance;
        private ITickService _tickService = TickService.Instance;
        private List<Agent> _agents = new List<Agent>();

        private void Awake()
        {
            _agentService.OnRequestAgentSpawn += SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn += RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn += ClearAllAgents;

            _tickService.OnRequestPause += PauseAgents;
            _tickService.OnRequestResume += ResumeAgents;
            _tickService.OnIncreaseSpeed += IncreaseAgentSpeed;
            _tickService.OnDecreaseSpeed += DecreaseAgentSpeed;
        }

        private void Start()
        {
            agentSpeed = Math.Clamp(agentSpeed, agentSpeedRange.x, agentSpeedRange.y);
            _tickService.RegisterAgentsSpeedChange(agentSpeed);
        }

        private void OnDestroy()
        {
            _agentService.OnRequestAgentSpawn -= SpawnAgent;
            _agentService.OnRequestRandomAgentDespawn -= RemoveRandomAgent;
            _agentService.OnRequestAllAgentDespawn -= ClearAllAgents;
            
            _tickService.OnRequestPause -= PauseAgents;
            _tickService.OnRequestResume -= ResumeAgents;
            _tickService.OnIncreaseSpeed -= IncreaseAgentSpeed;
            _tickService.OnDecreaseSpeed -= DecreaseAgentSpeed;
        }

        private void SpawnAgent()
        {
            Agent agentObject = Instantiate(agentPrefab).GetComponent<Agent>();
            agentObject.onReachDestination += RegisterAgentReachDestination;
            agentObject.StartMovement(agentSpeed, _isPaused);
            
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

        private void PauseAgents()
        {
            _isPaused = true;
            _agents.ForEach(x => x.PauseMovement());
        }

        private void ResumeAgents()
        {
            _isPaused = false;
            _agents.ForEach(x => x.ResumeMovement());
        }

        private void IncreaseAgentSpeed()
        {
            agentSpeed = Mathf.Clamp(agentSpeed + speedValueChange, agentSpeedRange.x, agentSpeedRange.y);
            ChangeAgentsSpeed();
            
        }
        
        private void DecreaseAgentSpeed()
        {
            agentSpeed = Mathf.Clamp(agentSpeed - speedValueChange, agentSpeedRange.x, agentSpeedRange.y);
            ChangeAgentsSpeed();
        }

        private void ChangeAgentsSpeed()
        {
            _agents.ForEach(x => x.ChangeMovementSpeed(agentSpeed, _isPaused));
            _tickService.RegisterAgentsSpeedChange(agentSpeed);
        }
    }
}
