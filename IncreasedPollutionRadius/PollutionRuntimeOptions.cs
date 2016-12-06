using ColossalFramework;

namespace IncreasedPollutionRadius
{
    public class PollutionRuntimeOptions : Singleton<PollutionRuntimeOptions>
    {
        private PollutionRuntimeOptions()
        {
            DebugOutput = false;
            TreeFetchRate = 10;
            TreeFetchRadius = 100;
            TreeFetchCoefficient = 540;
        }
        public bool DebugOutput { get; set; }
        public int TreeFetchRate { get; set; }
        public int TreeFetchRadius { get; set; }

        public int TreeFetchCoefficient { get; set; } //1..540

    }
}