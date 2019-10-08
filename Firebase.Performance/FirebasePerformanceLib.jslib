var FirebasePerformanceLib =
{
    $PerformanceWebGL:
    {
        traces: {},
        AddTrace: function (id, trace)
        {
            traces[id] = trace;
        },
        RemoveTrace: function (traceID)
        {
            delete traces[traceID];
        },
        GetTrace: function (traceID, logIfNotFound)
        {
            var trace = traces[traceID];
            if (logIfNotFound && !trace)
            {
                console.log("The underlying object of the Trace was lost.");
            }
            return trace;
        },
    },
    SetDataCollectionEnabled_WebGL: function (appNamePtr, enabled)
    {
        firebase.performance(_GetApp(appNamePtr)).dataCollectionEnabled = enabled;
    },
    GetDataCollectionEnabled_WebGL: function (appNamePtr)
    {
        return firebase.performance(_GetApp(appNamePtr)).dataCollectionEnabled;
    },
    SetInstrumentationEnabled_WebGL: function (appNamePtr, enabled)
    {
        firebase.performance(_etApp(appNamePtr)).instrumentationEnabled = enabled;
    },
    GetInstrumentationEnabled_WebGL: function (appNamePtr)
    {
        return firebase.performance(_GetApp(appNamePtr)).instrumentationEnabled;
    },
    CreateAPerformanceTrace_WebGL: function (id, appNamePtr, traceNamePtr)
    {
        var app = _GetApp(appNamePtr);
        var traceName = Pointer_stringify(traceNamePtr);
        var trace = firebase.performance(app).trace(traceName);
        PerformanceWebGL.AddTrace(id, trace);
    },
    RemovePerformanceTrace_WebGL: function (traceID)
    {
        PerformanceWebGL.RemoveTrace(traceID);
    },
    GetPerformanceTraceAttribute_WebGL: function (traceID, attributePtr)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return null;

        var attribute = Pointer_stringify(attributePtr);
        var value = trace.getAttribute(attribute);
        return _AllocateString(value);
    },
    GetAllPerformanceTraceCustomAttributes_WebGL: function (traceID)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return null;
        var attributes = JSON.stringify(trace.getAttributes());
        return _AllocateString(attributes);
    },
    GetPerformanceTraceMetric_WebGL: function (traceID, metricNamePtr)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return -1;
        var metricName = Pointer_stringify(metricNamePtr);
        return trace.getMetric(metricName);
    },
    IncrementPerformanceTraceMetric_WebGL: function (traceID, metricNamePtr, incrementBy)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        var metricName = Pointer_stringify(metricNamePtr);
        trace.incrementMetric(metricName, incrementBy);
    },
    AddPerformanceTraceAttribute_WebGL: function (traceID, attributePtr, valuePtr)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        var attribute = Pointer_stringify(attributePtr);
        var value = Pointer_stringify(valuePtr);
        trace.putAttribute(attribute, value);
    },
    AddPerformanceTraceMetric_WebGL: function (traceID, metricNamePtr, value)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        var metricName = Pointer_stringify(metricNamePtr);
        trace.putMetric(metricName, value);
    },
    RecordPerformanceTrace_WebGL: function (traceID, startTime, duration, optionsJsonPtr)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        var optionsJson = Pointer_stringify(optionsJsonPtr);
        var options = JSON.parse(optionsJson);
        trace.record(startTime, duration, options);
    },
    RemovePerformanceTraceAttribute_WebGL: function (traceID, attributePtr)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        var attribute = Pointer_stringify(attributePtr);
        trace.removeAttribute(attribute);
    },
    StartPerformanceTrace_WebGL: function (traceID)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        trace.start();
    },
    StopPerformanceTrace_WebGL: function (traceID)
    {
        var trace = PerformanceWebGL.GetTrace(traceID, true);
        if (!trace)
            return;
        trace.stop();
    },
};
autoAddDeps(FirebasePerformanceLib, '$PerformanceWebGL');
mergeInto(LibraryManager.library, FirebasePerformanceLib);