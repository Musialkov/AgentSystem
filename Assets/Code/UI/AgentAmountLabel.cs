using System;
using Code.Core;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class AgentAmountLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI agentsNumber;

        private IAgentService _agentService = AgentService.Instance;

        private void Start()
        {
            _agentService.OnAgentsNumberChange += UpdateLabel;
        }

        private void OnDestroy()
        {
            _agentService.OnAgentsNumberChange -= UpdateLabel;
        }

        private void UpdateLabel(int agentNumber)
        {
            agentsNumber.text = agentNumber.ToString();
        }
    }
}
