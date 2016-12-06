using System;

namespace IncreasedPollutionRadius.Redirection
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RedirectMethodAttribute : Attribute
    {
        public RedirectMethodAttribute()
        {
            this.OnCreated = false;
        }

        public RedirectMethodAttribute(bool onCreated)
        {
            this.OnCreated = onCreated;
        }

        public bool OnCreated { get; }
    }
}
