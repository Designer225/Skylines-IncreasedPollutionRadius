using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncreasedPollutionRadius
{
    [ConfigurationPath("IncreasedPollutionRadius.xml")]
    class IncreasedPollutionRadiusConfig
    {
        public double NoisePollutionFactor { get; set; } = 10.0;

        public double GroundPollutionFactor { get; set; } = 10.0;
    }
}
