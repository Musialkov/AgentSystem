using System;

namespace Code.Core
{
    public class TickService : ITickService
    {
        private static TickService _instance;

        public static TickService Instance
        {
            get { return _instance ??= new TickService(); }
        }

        public Action OnRequestPause { get; set; }
        public Action OnRequestResume { get; set; }
        public Action OnIncreaseSpeed { get; set; }
        public Action OnDecreaseSpeed { get; set; }
        public Action<float> OnSpeedChange { get; set; }

        public void RequestPause()
        {
            OnRequestPause?.Invoke();
        }

        public void RequestResume()
        {
            OnRequestResume?.Invoke();
        }

        public void IncreaseSpeed()
        {
            OnIncreaseSpeed?.Invoke();
        }

        public void DecreaseSpeed()
        {
            OnDecreaseSpeed?.Invoke();
        }

        public void RegisterAgentsSpeedChange(float agentsSpeed)
        {
            OnSpeedChange?.Invoke(agentsSpeed);
        }
    }
}
