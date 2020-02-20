using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class PromiseTimer : IPromiseTimer
{
    /// <summary>
    /// The current running total for time that this PromiseTimer has run for
    /// </summary>
    private float curTime;

    /// <summary>
    /// The current running total for the amount of frames the PromiseTimer has run for
    /// </summary>
    private int curFrame;

    /// <summary>
    /// Currently pending promises
    /// </summary>
    private readonly LinkedList<PredicateWait> waiting = new LinkedList<PredicateWait>();

    /// <summary>
    /// Resolve the returned promise once the time has elapsed
    /// </summary>
    public IPromise WaitFor(float seconds)
    {
        return WaitUntil(t => t.elapsedTime >= seconds);
    }

    /// <summary>
    /// Resolve the returned promise once the predicate evaluates to false
    /// </summary>
    public IPromise WaitWhile(Func<TimeData, bool> predicate)
    {
        return WaitUntil(t => !predicate(t));
    }

    /// <summary>
    /// Resolve the returned promise once the predicate evalutes to true
    /// </summary>
    public IPromise WaitUntil(Func<TimeData, bool> predicate)
    {
        var promise = new Task();

        var wait = new PredicateWait()
        {
            timeStarted = curTime,
            pendingPromise = promise,
            timeData = new TimeData(),
            predicate = predicate,
            frameStarted = curFrame
        };

        waiting.AddLast(wait);

        return promise;
    }
    /// <summary>
    /// Cancel a promise.
    /// </summary>
    /// <param name="promise">The promise to cancel.</param>
    /// <returns></returns>
    public bool Cancel(IPromise promise)
    {
        var node = FindInWaiting(promise);

        if (node == null)
        {
            return false;
        }

        node.Value.pendingPromise.SetException(new TaskCancelledException("Promise was cancelled by user."));
        waiting.Remove(node);

        return true;
    }

    LinkedListNode<PredicateWait> FindInWaiting(IPromise promise)
    {
        for (var node = waiting.First; node != null; node = node.Next)
        {
            if (node.Value.pendingPromise.Id.Equals(promise.Id))
            {
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Update all pending promises. Must be called for the promises to progress and resolve at all.
    /// </summary>
    public void Update(float deltaTime)
    {
        curTime += deltaTime;
        curFrame += 1;

        var node = waiting.First;
        while (node != null)
        {
            var wait = node.Value;

            var newElapsedTime = curTime - wait.timeStarted;
            wait.timeData.deltaTime = newElapsedTime - wait.timeData.elapsedTime;
            wait.timeData.elapsedTime = newElapsedTime;
            var newElapsedUpdates = curFrame - wait.frameStarted;
            wait.timeData.elapsedUpdates = newElapsedUpdates;

            bool result;
            try
            {
                result = wait.predicate(wait.timeData);
            }
            catch (Exception ex)
            {
                wait.pendingPromise.SetException(ex);

                node = RemoveNode(node);
                continue;
            }

            if (result)
            {
                wait.pendingPromise.Resolve();

                node = RemoveNode(node);
            }
            else
            {
                node = node.Next;
            }
        }
    }

    /// <summary>
    /// Removes the provided node and returns the next node in the list.
    /// </summary>
    private LinkedListNode<PredicateWait> RemoveNode(LinkedListNode<PredicateWait> node)
    {
        var currentNode = node;
        node = node.Next;

        waiting.Remove(currentNode);

        return node;
    }
}