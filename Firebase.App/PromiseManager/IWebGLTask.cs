
using System;
namespace Firebase.WebGL.Threading
{
    /// <summary>
    /// Interface for a Pending Task, running on the javascript side, aka a 'Promise'.
    /// </summary>
    internal interface IWebGLTask
    {
        void SetResult(string json, string error);
        void SetResult(IntPtr pBuffer, int bufferLength, string error);
    }
}