using System;
using System.Runtime.CompilerServices;
using ColossalFramework;
using IncreasedPollutionRadius.OptionsFramework;
using IncreasedPollutionRadius.Redirection;
using UnityEngine;

namespace IncreasedPollutionRadius.Detours
{
    [TargetType(typeof(NaturalResourceManager))]
    public class NaturalResourceManagerDetour : NaturalResourceManager
    {
        [RedirectMethod]
        public new int TryDumpResource(NaturalResourceManager.Resource resource, int rate, int max, Vector3 position,
            float radius)
        {
            //begin mod
            if (resource == Resource.Pollution)
            {
                if (Math.Abs(radius - 60.0) < 0.1)
                {
                    radius = LoadingExtension.OriginalFactor * radius;
                }
                var rateFactor = LoadingExtension.OriginalFactor * LoadingExtension.OriginalFactor;
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
            CountResource(resource, position, radius, 0, out numCells, out totalCells, out resultDelta);
            rate = Mathf.Min(rate, max);
            if (resource == Resource.Pollution && PollutionRuntimeOptions.instance.DebugOutput)
            {
                UnityEngine.Debug.Log($"#1: position={position}, radius={radius}, numCells={numCells}, totalCells={totalCells}, resultDelta={resultDelta}, rate={rate}");
            }
            if (totalCells == 0 || rate == 0)
                return 0;
            int cellDelta = (int)(((long)rate << 20) / (long)totalCells);
            CountResource(resource, position, radius, cellDelta, out numCells, out totalCells, out resultDelta);
            if (resource == Resource.Pollution && PollutionRuntimeOptions.instance.DebugOutput)
            {
                UnityEngine.Debug.Log($"#2: position={position}, cellDelta={cellDelta}, numCells={numCells}, totalCells={totalCells}, resultDelta={resultDelta}");
            }
            return rate;
        }

        [RedirectReverse]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private int CountResource(NaturalResourceManager.Resource resource, Vector3 position, float radius,
            int cellDelta, out int numCells, out int totalCells, out int resultDelta)
        {
            UnityEngine.Debug.LogError("failed to detour CountResource()");
            numCells = 0;
            totalCells = 0;
            resultDelta = 0;
            return 0;
        }
    }
}