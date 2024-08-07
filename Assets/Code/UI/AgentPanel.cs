using System;
using Code.Core;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.UI
{
    public class AgentPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI agentsNumber;
        [SerializeField] private TextMeshProUGUI agentMessage;

        private IAgentService _agentService = AgentService.Instance;

        private void Start()
        {
            _agentService.OnAgentsNumberChange += UpdateLabel;
            _agentService.OnAgentReachDestination += UpdateAgentMessage;
        }

        private void OnDestroy()
        {
            _agentService.OnAgentsNumberChange -= UpdateLabel;
        }

        private void UpdateLabel(int agentNumber)
        {
            agentsNumber.text = agentNumber.ToString();
        }

        private void UpdateAgentMessage(string agentGUID, Color agentColor)
        {
            string colorTag = ColorUtility.ToHtmlStringRGB(agentColor);
            string formattedText = $"Agent <color=#{colorTag}>{agentGUID}</color> arrived";
            agentMessage.text = formattedText;
        }
    }
}
