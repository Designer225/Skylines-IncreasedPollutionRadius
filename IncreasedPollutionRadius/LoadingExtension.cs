using System;
using System.Reflection;
using ICities;
using IncreasedPollutionRadius.Detours;
using IncreasedPollutionRadius.OptionsFramework;
using IncreasedPollutionRadius.Redirection;

namespace IncreasedPollutionRadius
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private static bool StatsUpdated;
        public static int OriginalFactor = -1;

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            Redirector<NaturalResourceManagerDetour>.Deploy();
            StatsUpdated = false;
            OriginalFactor = OptionsWrapper<Options>.Options.factor;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (StatsUpdated)
            {
                return;
            }
            UpdateStats();
            StatsUpdated = true;
        }

        private static void UpdateStats()
        {
            var count = PrefabCollection<BuildingInfo>.LoadedCount();
            for (uint i = 0; i < count; i++)
            {
                var info = PrefabCollection<BuildingInfo>.GetLoaded(i);
                if (info?.m_buildingAI == null || info.m_buildingAI is SnowDumpAI || (info.m_buildingAI is LandfillSiteAI && ((LandfillSiteAI)info.m_buildingAI).m_electricityProduction == 0))
                {
                    continue;
                }
                var type = info.m_buildingAI.GetType();
                var fieldInfo = type.GetField("m_pollutionRadius");
                if (fieldInfo != null && fieldInfo.FieldType == typeof(float))
                {
                    var newValue = (float) fieldInfo.GetValue(info.m_buildingAI)*OriginalFactor;
                    if (Math.Abs(newValue - 60) < 0.1)
                    {
                        newValue += 1; //to prevent multiplying twice
                    }
                    fieldInfo.SetValue(info.m_buildingAI, newValue);
                }
            }
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Redirector<NaturalResourceManagerDetour>.Revert();
        }
    }
}