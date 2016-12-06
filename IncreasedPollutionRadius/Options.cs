using System.Xml.Serialization;
using IncreasedPollutionRadius.OptionsFramework;

namespace IncreasedPollutionRadius
{
    public class Options : IModOptions
    {
        public Options()
        {
            this.factor = 10;
        }

        [TextField("Increase factor (default 10")]
        public int factor { get; private set;  }

        [XmlIgnore]
        public string FileName => "CSL-IncreasedPollutionRadius";
    }
}