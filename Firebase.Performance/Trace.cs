using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Firebase.Performance
{
    public sealed class Trace : IDisposable
    {
        static uint TracesID = 0;
        static IDictionary<uint, Trace> traces = null;

        public uint ID { get; }
        public string Name { get; }
        private Trace(uint id, string traceName)
        {
            ID = id;
            Name = traceName;
        }
        static Trace()
        {
            traces = new Dictionary<uint, Trace>();
        }
        /// <summary>
        /// Retrieves the value that the custom attribute is set to.
        /// </summary>
        /// <param name="attribute">Name of the custom attribute.</param>
        /// <returns></returns>
        public string GetAttribute(string attribute)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(attribute, nameof(attribute));
            return PerformancePInvoke.GetPerformanceTraceAttribute_WebGL(ID, attribute);
        }

        /// <summary>
        /// Returns a map of all custom attributes of a trace instance.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetAttributes()
        {
            string map = PerformancePInvoke.GetAllPerformanceTraceCustomAttributes_WebGL(ID);
            if (string.IsNullOrWhiteSpace(map))
                return new Dictionary<string, string>();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(map);
        }
        /// <summary>
        /// Returns the value of the custom metric by that name. If a custom metric with that name does not exist returns zero.
        /// </summary>
        /// <param name="metricName">Name of the custom metric.</param>
        /// <returns></returns>
        public int GetMetric(string metricName)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(metricName, nameof(metricName));
            return PerformancePInvoke.GetPerformanceTraceMetric_WebGL(ID, metricName);
        }

        /// <summary>
        /// Adds to the value of a custom metric. 
        /// If a custom metric with the provided name does not exist, it creates one with that name and the value equal to the given number.
        /// </summary>
        /// <param name="metricName">The name of the custom metric.</param>
        /// <param name="incrementBy">The number to be added to the value of the custom metric. If not provided, it uses a default value of one.</param>
        public void IncrementMetric(string metricName, int incrementBy)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(metricName, nameof(metricName));
            PerformancePInvoke.IncrementPerformanceTraceMetric_WebGL(ID, metricName, incrementBy);
        }

        /// <summary>
        /// Set a custom attribute of a <see cref="Trace"/> to a certain value.
        /// </summary>
        /// <param name="attribute">Name of the custom attribute.</param>
        /// <param name="value">Value of the custom attribute.</param>
        public void PutAttribute(string attribute, string value)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(attribute, nameof(attribute));
            PreconditionUtilities.CheckNotNullOrEmpty(value, nameof(value));
            PerformancePInvoke.AddPerformanceTraceAttribute_WebGL(ID, attribute, value);
        }

        /// <summary>
        /// Sets the value of the specified custom metric to the given number regardless of whether a metric with that name already exists on the <see cref="Trace"/> instance or not.
        /// </summary>
        /// <param name="metricName">Name of the custom metric.</param>
        /// <param name="value">Value to of the custom metric.</param>
        public void PutMetric(string metricName, int value)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(metricName, nameof(metricName));
            PerformancePInvoke.AddPerformanceTraceMetric_WebGL(ID, metricName, value);
        }


        /// <summary>
        /// Records a <see cref="Trace"/> from given parameters. This provides a direct way to use <see cref="Trace"/> without a need to start/stop. This is useful for use cases in which the <see cref="Trace"/> cannot directly be used (e.g. if the duration was captured before the Performance SDK was loaded).
        /// </summary>
        /// <param name="startTime">Trace start time.</param>
        /// <param name="durationInMilleseconds">The duraction of the trace in millisec.</param>
        /// <param name="options">An object which can optionally hold maps of custom metrics and custom attributes.</param>
        public void Record(DateTime startTime, uint durationInMilleseconds, TraceOptions options = null)
        {
            long totalMilleseconds = ((DateTimeOffset)startTime).ToUnixTimeMilliseconds();
            PerformancePInvoke.RecordPerformanceTrace_WebGL(ID, totalMilleseconds, durationInMilleseconds, TraceOptions.ToJson(options));
        }

        /// <summary>
        /// Removes the specified custom attribute from a <see cref="Trace"/> instance.
        /// </summary>
        /// <param name="attribute">Name of the custom attribute.</param>
        public void RemoveAttribute(string attribute)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(attribute, nameof(attribute));
        }

        /// <summary>
        /// Starts the timing for the <see cref="Trace"/> instance.
        /// </summary>
        public void Start()
        {
            PerformancePInvoke.StartPerformanceTrace_WebGL(ID);
        }
        /// <summary>
        /// Stops the timing of the <see cref="Trace"/> instance and logs the data of the instance.
        /// </summary>
        public void Stop()
        {
            PerformancePInvoke.StopPerformanceTrace_WebGL(ID);
        }

        public void Dispose()
        {
            traces.Remove(ID);
            PerformancePInvoke.RemovePerformanceTrace_WebGL(ID);
        }

        public static Trace Create(string appName, string traceName)
        {
            PreconditionUtilities.CheckNotNullOrEmpty(traceName, nameof(traceName));
            uint id = TracesID++;
            Trace trace = new Trace(id, traceName);
            traces.Add(id, trace);
            PerformancePInvoke.CreateAPerformanceTrace_WebGL(id, appName, traceName);
            return trace;
        }
    }
}
