using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contracts.Events;

namespace Arch
{
    public class EventRecognizer
    {
        static Dictionary<string, Type> _events = new Dictionary<string, Type>();
        public static Type GetEventBy(string type)
        {
            if (!_events.Any())
            {
                Assembly.GetAssembly(typeof(Event))
                    .GetTypes()
                    .Where(w => w != typeof(FromGameEvent) && (w.BaseType == typeof(Event) || w.BaseType == typeof(FromGameEvent)))
                    .Select(s =>{

                    return new
                    {
                        TypeName = s.GetProperty("Type").GetValue(Activator.CreateInstance(s)).ToString(),
                        Type = s
                    };})
                    .ToList()
                    .ForEach(f =>
                    {
                        _events.Add(f.TypeName, f.Type);
                    });
            }

            return _events[type];
        }
    }
}