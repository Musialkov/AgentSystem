using System;

namespace Code.Core
{
    public interface IAgentService
    {
        public Action OnRequestAgentSpawn { get; set; }
        public Action OnRequestRandomAgentDespawn { get; set; }
        public Action OnRequestAllAgentDespawn { get; set; }
        public Action<int> OnAgentsNumberChange { get; set; }
        public void RequestAgentSpawn();
        public void RequestRandomAgentDespawn();
        public void RequestAllAgentDespawn();
        public void RegisterAgentsNumberChange(int agentsNumber);
    }
}
