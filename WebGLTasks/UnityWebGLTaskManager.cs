using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firebase.WebGL.Threading
{
    internal class UnityWebGLTaskManager : MonoBehaviour
    {
        static UnityWebGLTaskManager waiter = null;
        public static UnityWebGLTaskManager Instance
        {
            get
            {
                if(waiter == null)
                {
                    waiter = new GameObject("UnityTaskWaiter").AddComponent<UnityWebGLTaskManager>();
                }
                return waiter;
            }
        }
        Queue<Action> jobs = new Queue<Action>();

        protected void Awake()
        {
            waiter = this;
        }

        protected void OnDestroy()
        {
            if (waiter == this)
                waiter = null;
        }

        private void Update()
        {
            while (jobs.Count > 0)
                jobs.Dequeue()?.Invoke();
        }

        private IEnumerator WaitRoutine(TimeSpan time, Action callback)
        {
            yield return new WaitForSeconds((float)time.TotalSeconds);
            callback?.Invoke();
        }

        public static void Dispatch(Action action)
        {
            Instance.jobs.Enqueue(action);
        }
        public static void Wait(TimeSpan time, Action callback)
        {
            Instance.StartCoroutine(Instance.WaitRoutine(time, callback));
        }
    }
}
