using System;
using UnityEngine;

namespace Code.Core
{
    public interface IAgentService
    {
        public Action OnRequestAgentSpawn { get; set; }
        public Action OnRequestRandomAgentDespawn { get; set; }
        public Action OnRequestAllAgentDespawn { get; set; }
        public Action<int> OnAgentsNumberChange { get; set; }
        public Action<string, Color> OnAgentReachDestination { get; set; }
        public void RequestAgentSpawn();
        public void RequestRandomAgentDespawn();
        public void RequestAllAgentDespawn();
        public void RegisterAgentsNumberChange(int agentsNumber);
        public void RegisterAgentReachDestination(string agentGUID, Color agentColor);
    }
}
