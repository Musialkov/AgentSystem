using Code.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class TimeButtons : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button increaseSpeedButton;
        [SerializeField] private Button decreaseSpeedButton;

        private ITickService _tickService = TickService.Instance;

        private void Awake()
        {
            pauseButton.onClick.AddListener(_tickService.RequestPause);
            resumeButton.onClick.AddListener(_tickService.RequestResume);
            increaseSpeedButton.onClick.AddListener(_tickService.IncreaseSpeed);
            decreaseSpeedButton.onClick.AddListener(_tickService.DecreaseSpeed);
        }

        private void OnDestroy()
        {
            pauseButton.onClick.RemoveAllListeners();
            resumeButton.onClick.RemoveAllListeners();
            increaseSpeedButton.onClick.RemoveAllListeners();
            decreaseSpeedButton.onClick.RemoveAllListeners();
        }
    }
}
