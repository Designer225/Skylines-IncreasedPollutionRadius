using System;
using System.Runtime.CompilerServices;
using ColossalFramework;
using IncreasedPollutionRadius.OptionsFramework;
using IncreasedPollutionRadius.Redirection;
using UnityEngine;

namespace IncreasedPollutionRadius
{
    [TargetType(typeof(NaturalResourceManager))]
    public class NaturalResourceManagerDetour : NaturalResourceManager
    {
        private static bool DEBUG_OUTPUT = false;

        [RedirectMethod]
        public int TryDumpResource(NaturalResourceManager.Resource resource, int rate, int max, Vector3 position, float radius, bool refresh)
        {    
            //begin mod
            if (resource == Resource.Pollution)
            {
                if (Math.Abs(radius - 60.0) < 0.1)
                {
                    radius = LoadingExtension.OriginalFactor * radius;
                }
                var rateFactor = LoadingExtension.OriginalFactor /** LoadingExtension.OriginalFactor*/;
                rate = rate * rateFactor;
                max = max * rateFactor;
            }
            else
            {
                radius = Mathf.Max(33.75f, (float)Singleton<SimulationManager>.instance.m_randomizer.Int32(200, 1000) * (radius * (1f / 1000f)));
            }
            //end mod
            int numCells;
            int totalCells;
            int resultDelta;
            this.CountResource(resource, position, radius, 0, out numCells, out totalCells, out resultDelta, false);
            rate = Mathf.Min(rate, max);
            //begin mod
            if (resource == Resource.Pollution && DEBUG_OUTPUT)
            {
                UnityEngine.Debug.Log($"#1: position={position}, radius={radius}, numCells={numCells}, totalCells={totalCells}, resultDelta={resultDelta}, rate={rate}");
            }
            //end mod
            if (totalCells == 0 || rate == 0)
                return 0;
            int cellDelta = (int)(((long)rate << 20) / (long)totalCells);
            this.CountResource(resource, position, radius, cellDelta, out numCells, out totalCells, out resultDelta, refresh);
            //begin mod
            if (resource == Resource.Pollution && DEBUG_OUTPUT)
            {
                UnityEngine.Debug.Log($"#2: position={position}, cellDelta={cellDelta}, numCells={numCells}, totalCells={totalCells}, resultDelta={resultDelta}");
            }
            //end mod
            return rate;
        }

        [RedirectReverse]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private int CountResource(NaturalResourceManager.Resource resource, Vector3 position, float radius, int cellDelta, out int numCells, out int totalCells, out int resultDelta, bool refresh)
        {
            UnityEngine.Debug.LogError("failed to detour CountResource()");
            numCells = 0;
            totalCells = 0;
            resultDelta = 0;
            return 0;
        }
    }
}