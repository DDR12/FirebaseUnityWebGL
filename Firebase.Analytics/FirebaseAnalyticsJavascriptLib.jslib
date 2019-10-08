var FirebaseAnalyticsJavascriptLib =
{
    $AnalyticsWebGL:
    {
    },
    SetAnalyticsSettings_WebGL: function (settingsJsonPtr)
    {
        var settings = JSON.parse(Pointer_stringify(settingsJsonPtr));
        firebase.analytics.settings(settings);
    },
    SetAnalyticsUserProperty_WebGL: function (appNamePtr, propertyNamePtr, propertyValuePtr)
    {
        var propertyName = Pointer_stringify(propertyNamePtr);
        var propertyValue = Pointer_stringify(propertyValuePtr);
        var params = {};
        params[propertyName] = propertyValue;
        firebase.analytics(_GetApp(appNamePtr)).setUserProperties(params);
    },
    SetAnalyticsUserId_WebGL: function (appNamePtr, userIdPtr)
    {
        firebase.analytics(_GetApp(appNamePtr)).setUserId(Pointer_stringify(userIdPtr));
    },
    SetAnalyticsCurrentScreen_WebGL: function (appNamePtr, screenNamePtr, optionsJsonPtr)
    {
        var screenName = Pointer_stringify(screenNamePtr);
        var options = JSON.parse(Pointer_stringify(optionsJsonPtr));
        firebase.analytics(_GetApp(appNamePtr)).setCurrentScreen(screenName, options);
    },
    LogAnalyticsEvent_WebGL: function (appNamePtr, eventNamePtr, eventParamsJsonPtr, optionsJsonPtr)
    {
        var eventName = Pointer_stringify(eventNamePtr);
        var eventParams = JSON.parse(Pointer_stringify(eventParamsJsonPtr));
        var options = JSON.parse(Pointer_stringify(optionsJsonPtr));
        firebase.analytics(_GetApp(appNamePtr)).logEvent(eventName, eventParams, options);
    },
    SetAnalyticsCollectionEnabled_WebGL: function (appNamePtr, enabled)
    {
        firebase.analytics(_GetApp(appNamePtr)).setAnalyticsCollectionEnabled(enabled);
    },
};
autoAddDeps(FirebaseAnalyticsJavascriptLib, '$AnalyticsWebGL');
mergeInto(LibraryManager.library, FirebaseAnalyticsJavascriptLib);