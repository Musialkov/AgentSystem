using System;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Core
{
    public class AgentService : IAgentService
    {
        private static AgentService _instance;

        public static AgentService Instance
        {
            get { return _instance ??= new AgentService(); }
        }
        
        public Action OnRequestAgentSpawn { get; set; }
        public Action OnRequestRandomAgentDespawn { get; set; }
        public Action OnRequestAllAgentDespawn { get; set; }
        public Action<int> OnAgentsNumberChange { get; set; }
        public Action<string, Color> OnAgentReachDestination { get; set; }
        

        public void RequestAgentSpawn()
        {
            OnRequestAgentSpawn?.Invoke();
        }

        public void RequestRandomAgentDespawn()
        {
            OnRequestRandomAgentDespawn?.Invoke();
        }

        public void RequestAllAgentDespawn()
        {
            OnRequestAllAgentDespawn?.Invoke();
        }

        public void RegisterAgentsNumberChange(int agentsNumber)
        {
            OnAgentsNumberChange?.Invoke(agentsNumber);
        }

        public void RegisterAgentReachDestination(string agentGUID, Color agentColor)
        {
            OnAgentReachDestination?.Invoke(agentGUID, agentColor);
        }
    }
}
