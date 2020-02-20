﻿
using System;

namespace Firebase.WebGL.Threading
{
    public class TaskScheduler
    {
        public TaskScheduler(System.Threading.SynchronizationContext context)
        {
        }

        public static TaskScheduler FromCurrentSynchronizationContext()
        {
            return new TaskScheduler(System.Threading.SynchronizationContext.Current);
        }

        public void Post(Action action)
        {
            UnityWebGLTaskManager.Dispatch(action);
        }
    }
}
