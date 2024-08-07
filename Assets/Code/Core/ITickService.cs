using System;

namespace Code.Core
{
    public interface ITickService
    {
        public Action OnRequestPause { get; set; }
        public Action OnRequestResume { get; set; }
        public Action OnIncreaseSpeed { get; set; }
        public Action OnDecreaseSpeed { get; set; }
        public Action<float> OnSpeedChange { get; set; }
        public void RequestPause();
        public void RequestResume();
        public void IncreaseSpeed();
        public void DecreaseSpeed();
        public void RegisterAgentsSpeedChange(float agentsSpeed);
    }
}
