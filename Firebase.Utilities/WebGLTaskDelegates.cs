using System;

namespace Firebase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="taskID"></param>
    /// <param name="errorJson"></param>
    public delegate void VoidTaskWebGLDelegate(int taskID, string errorJson);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="taskID"></param>
    /// <param name="dataJson"></param>
    /// <param name="errorJson"></param>
    public delegate void GenericTaskWebGLDelegate(int taskID, string dataJson, string errorJson);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="taskID"></param>
    /// <param name="pBuffer"></param>
    /// <param name="bufferLength"></param>
    /// <param name="errorJson"></param>
    public delegate void ByteArrayTaskWebGLDelegate(int taskID, IntPtr pBuffer, int bufferLength, string errorJson);
}