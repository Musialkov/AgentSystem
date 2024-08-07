using System;
using Code.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class AddAgentButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        private IAgentService _agentService = AgentService.Instance;

        private void Start()
        {
            button.onClick.AddListener(_agentService.RequestAgentSpawn);
        }
    }
}
