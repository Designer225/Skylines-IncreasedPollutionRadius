using System.Xml.Serialization;

namespace IncreasedPollutionRadius.OptionsFramework
{
    public interface IModOptions
    {
        [XmlIgnore]
        string FileName
        {
            get;
        }
    }
}