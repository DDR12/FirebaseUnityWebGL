using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firebase.Auth.UI
{
    /// <summary>
    /// [Effective only in WebGL, acts as a stub in other platforms].
    /// Represents an instance of a native firebase auth ui, which is pretty, native, cross platform and saves you alot of work, validations, rendering etc etc.
    /// </summary>
    public sealed class AuthUI : IDisposable
    {
        static uint UIID = 0;
        static Dictionary<uint, AuthUI> AuthUIs = new Dictionary<uint, AuthUI>();

        /// <summary>
        /// Event triggered when the visibility of this Auth UI Instance changes.
        /// </summary>
        public event EventHandler<AuthUIVisibilityChangeEventArgs> OnVisibilityChanged;
        /// <summary>
        /// Whether or not this Auth UI instance is currently visible.
        /// </summary>
        public bool IsVisible { get; private set; }
        /// <summary>
        /// Returns true if this UI is pending redirect.
        /// </summary>
        public bool IsPendingRedirect
        {
            get
            {

#if !UNITY_EDITOR && UNITY_WEBGL
                    return AuthUIPInvoke.GetAuthUIIsPendingRedirect_WebGL(ID);
#else
                return false;
#endif
            }
        }
        /// <summary>
        /// The Auth instance associated with this UI.
        /// </summary>
        public FirebaseAuth Auth { get; }
        private readonly uint ID = 0;
        /// <summary>
        /// Create a new Auth UI instance for this Auth instance, this doesn't immediately show the ui, to show it use <see cref="Start(Config)"/>
        /// </summary>
        /// <param name="auth"></param>
        public AuthUI(FirebaseAuth auth)
        {
            Auth = auth;
            ID = UIID++;
            // So when users use this api in editor or other platform, it just acts as a stub and not throw errors.

#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.CreateNewAuthUI_WebGL(ID, auth.App.Name, AuthUIShownCallback_AOT);
            AuthUIs.Add(ID, this);
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        ~AuthUI()
        {
            Dispose();
        }
        private void SetVisible(bool visible)
        {
            IsVisible = visible;
            OnVisibilityChanged?.Invoke(this, new AuthUIVisibilityChangeEventArgs(visible));
        }
        /// <summary>
        /// Auto sign-in for returning users can be disabled by calling <see cref="DisableAutoSignIn"/>. 
        /// This may be needed if the FirebaseUI sign-in page is being rendered after the user signs out.
        /// Auto sign-in for returning users is enabled by default except when prompt is
        /// not 'none' in the Google provider custom parameters. Calling this will disable auto sign in.
        /// </summary>
        public void DisableAutoSignIn()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.DisableAuthUIAutoSignIn_WebGL(ID);
#endif
        }

        /// <summary>
        /// Starts and shows the auth UI, using the supplied configrations, the ui is rendered, and on login success returns the user or an error.
        /// </summary>
        /// <param name="config">Settings for the displayed UI instance.</param>
        /// <returns>A task that resolves with the logged in user or an error otherwise.</returns>
        public Task<SignInResult> Start(Config config)
        {
            TaskCompletionSource<SignInResult> task = WebGLTaskManager.GetTask<SignInResult>();

#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.StartAuthUI_WebGL(ID, task.Task.Id, config.ToJson(), WebGLTaskManager.DequeueTask);
#endif
            return task.Task;
        }
        

        /// <summary>
        /// Manual sign in trigger, This triggers automatically when the user clicks the sign in button, I don't know why you may need to trigger this manually. but its there.
        /// </summary>
        public void SignIn()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.AuthUISignIn_WebGL(ID);
#endif
        }

        /// <summary>
        /// Resets the ui but keeps the instance alive, so you don't have to instantiate another UI if your app is single paged, which unity apps are, so just call reset before <see cref="Start"/>
        /// </summary>
        public void Reset()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.ResetAuthUI_WebGL(ID);
#endif
        }

        /// <summary>
        /// Deletes this ui Instance, after calling this, you can't call reset, you have to instantiate a new instace of the UI.
        /// </summary>
        /// <returns>A task that resolves with the id of the deleted ui, or rejects if an error occurs.</returns>
        public Task DeleteAsync()
        {
            TaskCompletionSource<object> task = WebGLTaskManager.GetTask();

#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.DeleteAuthUI_WebGL(task.Task.Id, ID, WebGLTaskManager.DequeueTask);
#endif
            return task.Task;
        }

        /// <summary>
        /// Dispose of this instace of the Auth UI.
        /// If you call this manually, this auth ui instace will be un usable, if you want to delete the ui but keep the instance for re-use, use <see cref="DeleteAsync"/>
        /// </summary>
        public void Dispose()
        {
            AuthUIs.Remove(ID);

#if !UNITY_EDITOR && UNITY_WEBGL
            AuthUIPInvoke.ReleaseAuthUI_WebGL(ID);
#endif
        }

        [AOT.MonoPInvokeCallback(typeof(WebGLAuthUIVisibilityCallback))]
        private static void AuthUIShownCallback_AOT(uint uiID, bool isVisible)
        {
            // Give input control to the browser, cause unity hijacks it, once we're done, we're giving the control back to unity.
            PlatformHandler.CaptureWebGLInput(false);
            if (AuthUIs.TryGetValue(uiID, out AuthUI authUI))
            {
                authUI.SetVisible(isVisible);
            }
            bool anyUIIsVisible = AuthUIs.Values.Any(o => o.IsVisible);
            PlatformHandler.CaptureWebGLInput(!anyUIIsVisible);
        }
    }
}
