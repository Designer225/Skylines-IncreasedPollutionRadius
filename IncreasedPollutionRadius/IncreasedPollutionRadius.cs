using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using ICities;

namespace IncreasedPollutionRadius
{
    class IncreasedPollutionRadius : IUserMod
    {
        private static IncreasedPollutionRadiusConfig config = Configuration<IncreasedPollutionRadiusConfig>.Load();
        public string Name => "Increased Pollution Radius";
        public string Description => "Makes pollution radius much more significant";

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("Increased Pollution Radius");
            group.AddTextfield("Ground Pollution Factor", ""+config.GroundPollutionFactor, x =>
            {
                var result = config.GroundPollutionFactor;
                Double.TryParse(x, out result);
                config.GroundPollutionFactor = result;
                Configuration<IncreasedPollutionRadius>.Save();
            });
            group.AddTextfield("Noise Pollution Factor", "" + config.NoisePollutionFactor, x =>
            {
                var result = config.NoisePollutionFactor;
                Double.TryParse(x, out result);
                config.NoisePollutionFactor = result;
                Configuration<IncreasedPollutionRadius>.Save();
            });
        }

        public class PollutionLogic : LoadingExtensionBase
        {
            private static bool StatsUpdated;
            public static float GroundPollutionFactor = -1;

            public override void OnCreated(ILoading loading)
            {
                base.OnCreated(loading);
                var harmony = HarmonyInstance.Create("d225.bpincreasedpollutionradius");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                //Redirector<NaturalResourceManagerDetour>.Deploy();
                StatsUpdated = false;
                GroundPollutionFactor = (float) config.GroundPollutionFactor;
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
                        var newValue = (float)fieldInfo.GetValue(info.m_buildingAI) * GroundPollutionFactor;
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
                //Redirector<NaturalResourceManagerDetour>.Revert();
            }
        }
    }
}
