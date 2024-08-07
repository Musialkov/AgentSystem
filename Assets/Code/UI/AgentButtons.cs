using System;
using Code.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class AgentButtons : MonoBehaviour
    {
        [SerializeField] private Button addAgentButton;
        [SerializeField] private Button removeRandomAgentButton;
        [SerializeField] private Button removeAllAgentsButton;
        
        private IAgentService _agentService = AgentService.Instance;

        private void Start()
        {
            addAgentButton.onClick.AddListener(_agentService.RequestAgentSpawn);
            removeRandomAgentButton.onClick.AddListener(_agentService.RequestRandomAgentDespawn);
            removeAllAgentsButton.onClick.AddListener(_agentService.RequestAllAgentDespawn);
        }

        private void OnDestroy()
        {
            addAgentButton.onClick.RemoveAllListeners();
            removeRandomAgentButton.onClick.RemoveAllListeners();
            removeAllAgentsButton.onClick.RemoveAllListeners();
        }
    }
}
