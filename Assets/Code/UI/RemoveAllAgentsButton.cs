using Code.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class RemoveAllAgentsButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        private IAgentService _agentService = AgentService.Instance;

        private void Start()
        {
            button.onClick.AddListener(_agentService.RequestAllAgentDespawn);
        }
    }
}
