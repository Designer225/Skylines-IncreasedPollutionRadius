using ColossalFramework;
using ICities;

namespace IncreasedPollutionRadius
{
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public long frames;


        public override void OnAfterSimulationFrame()
        {
            base.OnAfterSimulationFrame();
            frames++;
            var rate = PollutionRuntimeOptions.instance.TreeFetchRate;
            var radius = PollutionRuntimeOptions.instance.TreeFetchRadius;

            var coefficient = PollutionRuntimeOptions.instance.TreeFetchCoefficient;
            int start = (int) (coefficient < 2 ? 0 : (540 / coefficient) * (frames % coefficient));
            int finish = (int) (coefficient < 2 ? 539 : (540 / coefficient) * (frames % coefficient + 1) - 1);

            for (int index2 = start; index2 <= finish; ++index2)
            {
                for (int index3 = 0; index3 <= 539; ++index3)
                {
                    uint buildingID = TreeManager.instance.m_treeGrid[index2 * 540 + index3];
                    int num5 = 0;
                    while ((int)buildingID != 0)
                    {
                        var tree = TreeManager.instance.m_trees.m_buffer[(int)buildingID];
                        if (!tree.Hidden)
                        {

                            NaturalResourceManager.instance.TryFetchResource(NaturalResourceManager.Resource.Pollution, rate, rate,
                                tree.Position, radius);
                        }

                        buildingID = TreeManager.instance.m_trees.m_buffer[(int)buildingID].m_nextGridTree;
                        if (++num5 >= TreeManager.instance.m_trees.m_size)
                        {
                            CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + System.Environment.StackTrace);
                            break;
                        }
                    }
                }
            }

        }


    }
}