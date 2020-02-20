using System;
using System.Collections.Generic;

namespace Firebase
{
    /// <summary>
    /// Coordinates events between the C# and Javascript sides, as a middle man.
    /// </summary>
    public static class FirebaseWebGLCSharpEventsCoordinator
    {
        static uint ListenerID = 0;
        static IDictionary<object, uint> handlersIDs = null;
        static FirebaseWebGLCSharpEventsCoordinator()
        {
            handlersIDs = new Dictionary<object, uint>();
        }

        /// <summary>
        /// Subscribes an generic event locally, and returns a listener id (if needed) to identify the listener on the Javascript side.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The event handler to unlisten to</param>
        /// <param name="value">The subscriber to attach.</param>
        /// <returns></returns>
        public static uint? SubscribeToEvent<T>(EventHandler<T>  eventHandler, EventHandler<T> value)
        {
            if (value == null)
                return null;
            eventHandler += value;
            if (eventHandler.GetInvocationList().Length == 1)
            {
                if (handlersIDs.TryGetValue(eventHandler, out uint id))
                    return id;
                else
                {

                    handlersIDs.Add(eventHandler, ListenerID++);
                    return handlersIDs[eventHandler];
                }
            }
            else
                return null;
        }

        /// <summary>
        /// UnSubscribes an non-generic event locally, and returns The listener id (if needed) to identify the listener on the Javascript side.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler">The event handler to unlisten to</param>
        /// <param name="value">The subscriber to detach.</param>
        /// <returns></returns>
        public static uint? UnsubscribeFromEvent<T>(EventHandler<T> eventHandler, EventHandler<T> value)
        {
            if (value == null)
                return null;
            var wasEmpty = eventHandler.GetInvocationList().Length == 0;
            eventHandler -= value;
            var isEmpty = eventHandler.GetInvocationList().Length == 0;
            if (isEmpty != wasEmpty)
            {
                if (handlersIDs.TryGetValue(eventHandler, out uint id))
                    return id;
            }
            return null;

        }
    }
}
