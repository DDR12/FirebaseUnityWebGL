using System.Runtime.InteropServices;

namespace Firebase.Performance
{
    internal class PerformancePInvoke
    {
        [DllImport("__Internal")]
        public static extern void SetDataCollectionEnabled_WebGL(string appName, bool enabled);

        [DllImport("__Internal")]
        public static extern bool GetDataCollectionEnabled_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void SetInstrumentationEnabled_WebGL(string appName, bool enabled);

        [DllImport("__Internal")]
        public static extern bool GetInstrumentationEnabled_WebGL(string appName);

        [DllImport("__Internal")]
        public static extern void CreateAPerformanceTrace_WebGL(uint traceID, string appName, string traceName);

        [DllImport("__Internal")]
        public static extern void RemovePerformanceTrace_WebGL(uint traceID);

        [DllImport("__Internal")]
        public static extern string GetPerformanceTraceAttribute_WebGL(uint traceID, string attribute);
        [DllImport("__Internal")]
        public static extern string GetAllPerformanceTraceCustomAttributes_WebGL(uint traceID);
        [DllImport("__Internal")]
        public static extern int GetPerformanceTraceMetric_WebGL(uint traceID, string metricName);
        [DllImport("__Internal")]
        public static extern void IncrementPerformanceTraceMetric_WebGL(uint traceID, string metricName, int incrementBy);
        [DllImport("__Internal")]
        public static extern void AddPerformanceTraceAttribute_WebGL(uint traceID, string attribute, string value);
        [DllImport("__Internal")]
        public static extern void AddPerformanceTraceMetric_WebGL(uint traceID, string metricName, int value);
        [DllImport("__Internal")]
        public static extern void RecordPerformanceTrace_WebGL(uint traceID, long startTime, uint duration, string optionsJson);
        [DllImport("__Internal")]
        public static extern void RemovePerformanceTraceAttribute_WebGL(uint traceID, string attribute);
        [DllImport("__Internal")]
        public static extern void StartPerformanceTrace_WebGL(uint traceID);
        [DllImport("__Internal")]
        public static extern void StopPerformanceTrace_WebGL(uint traceID);
    }
}
