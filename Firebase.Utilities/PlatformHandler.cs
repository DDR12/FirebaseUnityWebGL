using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Utility checks/methods for platform specific operations without having the user use #if #else directives in their code.
/// </summary>
public static class PlatformHandler
{
    /// <summary>
    /// By default, the unity application captures all input events and pipelines them into the unity application, we don't want this for events such as showing the phone auth recaptcha, showing prompts etc.
    /// To allow input to normal web page, pass false, to allow unity to receive mouse/keyboard events again, pass true.
    /// </summary>
    /// <param name="capture">Whether or not unity should handle all keyboard/mouse input events on the page.</param>
    public static void CaptureWebGLInput(bool capture)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        UnityEngine.WebGLInput.captureAllKeyboardInput = capture;
#endif
    }

    /// <summary>
    /// Notifies users that a feature in the webgl wrapper is just a stub to match the official editor library methods and thus using/calling said feature has no effect.
    /// </summary>
    public static void NotifyWebGLFeatureDoesntHaveAMatch([CallerMemberName] string memberName = null)
    {
        Debug.Log($"Using: {memberName} has no effect in WebGL, this implementation is just a stub to match official library.");
    }
    /// <summary>
    /// Notifies that a feature is automatically handled in webgl and has no actual callable implentation in the javascript library.
    /// </summary>
    /// <param name="memberName"></param>
    public static void NotifyFeatureIsUselessInWebGL([CallerMemberName] string memberName = null)
    {
        Debug.Log($"Using: {memberName} in webgl is useless the functionality is handled automatically, if your app/game is webgl only, this is only usefull only in editor.");
    }
    /// <summary>
    /// Returns an exception for trying to use the calling member in a non-webgl build platform.
    /// </summary>
    /// <param name="memberName">The member that the user tried to call outside WebGL build.</param>
    /// <returns></returns>
    public static System.Exception GetWebGLOnlyFeatureException([CallerMemberName] string memberName = null)
    {
        return new System.Exception($"{memberName} is WebGL only feature, that means it is only functional when running the app/game in the browser.");
    }
}