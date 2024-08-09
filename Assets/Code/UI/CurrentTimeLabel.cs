using Code.Core;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class CurrentTimeLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speedText;

        private ITickService _tickService = TickService.Instance;
        
        private void Awake()
        {
            _tickService.OnSpeedChange += UpdateLabel;
        }

        private void UpdateLabel(float currentSpeed)
        {
            speedText.text = currentSpeed.ToString();
        }
    }
}
