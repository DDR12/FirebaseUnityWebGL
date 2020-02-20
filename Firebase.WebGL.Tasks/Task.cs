using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Implements a non-generic C# promise, this is a promise that simply resolves without delivering a value.
/// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
/// </summary>
public class Task
{


    /// <summary>
    /// The exception when the promise is rejected.
    /// </summary>
    private Exception rejectionException;

    /// <summary>
    /// Error handlers.
    /// </summary>
    private List<RejectHandler> rejectHandlers;

    /// <summary>
    /// Represents a handler invoked when the promise is resolved.
    /// </summary>
    public struct ResolveHandler
    {
        /// <summary>
        /// Callback fn.
        /// </summary>
        public Action callback;

        /// <summary>
        /// The promise that is rejected when there is an error while invoking the handler.
        /// </summary>
        public IRejectable rejectable;
    }

    /// <summary>
    /// Completed handlers that accept no value.
    /// </summary>
    private List<ResolveHandler> resolveHandlers;

    /// <summary>
    /// Progress handlers.
    /// </summary>
    private List<ProgressHandler> progressHandlers;

    /// <summary>
    /// ID of the promise, useful for debugging.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Name of the promise, when set, useful for debugging.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Tracks the current state of the promise.
    /// </summary>
    public PromiseState CurState { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public Task()
    {
        this.CurState = PromiseState.Pending;
        this.Id = PromiseHelpers.NextPromiseID();
        if (EnablePromiseTracking)
        {
            PendingPromises.Add(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resolver"></param>
    public Task(Action<Action, Action<Exception>> resolver)
    {
        this.CurState = PromiseState.Pending;
        this.Id = PromiseHelpers.NextPromiseID();
        if (EnablePromiseTracking)
        {
            PendingPromises.Add(this);
        }

        try
        {
            resolver(Resolve, SetException);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    private Task(PromiseState initialState)
    {
        CurState = initialState;
        Id = PromiseHelpers.NextPromiseID();
    }


    /// <summary>
    /// Add a rejection handler for this promise.
    /// </summary>
    private void AddRejectHandler(Action<Exception> onRejected, IRejectable rejectable)
    {
        if (rejectHandlers == null)
        {
            rejectHandlers = new List<RejectHandler>();
        }

        rejectHandlers.Add(new RejectHandler
        {
            callback = onRejected,
            rejectable = rejectable
        });
    }

    /// <summary>
    /// Add a resolve handler for this promise.
    /// </summary>
    private void AddResolveHandler(Action onResolved, IRejectable rejectable)
    {
        if (resolveHandlers == null)
        {
            resolveHandlers = new List<ResolveHandler>();
        }

        resolveHandlers.Add(new ResolveHandler
        {
            callback = onResolved,
            rejectable = rejectable
        });
    }

    /// <summary>
    /// Add a progress handler for this promise.
    /// </summary>
    private void AddProgressHandler(Action<float> onProgress, IRejectable rejectable)
    {
        if (progressHandlers == null)
        {
            progressHandlers = new List<ProgressHandler>();
        }

        progressHandlers.Add(new ProgressHandler { callback = onProgress, rejectable = rejectable });
    }

    /// <summary>
    /// Invoke a single error handler.
    /// </summary>
    private void InvokeRejectHandler(Action<Exception> callback, IRejectable rejectable, Exception value)
    {
        //            Argument.NotNull(() => callback);
        //            Argument.NotNull(() => rejectable);

        try
        {
            callback(value);
        }
        catch (Exception ex)
        {
            rejectable.SetException(ex);
        }
    }

    /// <summary>
    /// Invoke a single resolve handler.
    /// </summary>
    private void InvokeResolveHandler(Action callback, IRejectable rejectable)
    {
        //            Argument.NotNull(() => callback);
        //            Argument.NotNull(() => rejectable);

        try
        {
            callback();
        }
        catch (Exception ex)
        {
            rejectable.SetException(ex);
        }
    }

    /// <summary>
    /// Invoke a single progress handler.
    /// </summary>
    private void InvokeProgressHandler(Action<float> callback, IRejectable rejectable, float progress)
    {
        //            Argument.NotNull(() => callback);
        //            Argument.NotNull(() => rejectable);

        try
        {
            callback(progress);
        }
        catch (Exception ex)
        {
            rejectable.SetException(ex);
        }
    }

    /// <summary>
    /// Helper function clear out all handlers after resolution or rejection.
    /// </summary>
    private void ClearHandlers()
    {
        rejectHandlers = null;
        resolveHandlers = null;
        progressHandlers = null;
    }

    /// <summary>
    /// Invoke all reject handlers.
    /// </summary>
    private void InvokeRejectHandlers(Exception ex)
    {
        //            Argument.NotNull(() => ex);

        if (rejectHandlers != null)
        {
            for (int i = 0, maxI = rejectHandlers.Count; i < maxI; ++i)
                InvokeRejectHandler(rejectHandlers[i].callback, rejectHandlers[i].rejectable, ex);
        }

        ClearHandlers();
    }

    /// <summary>
    /// Invoke all resolve handlers.
    /// </summary>
    private void InvokeResolveHandlers()
    {
        if (resolveHandlers != null)
        {
            for (int i = 0, maxI = resolveHandlers.Count; i < maxI; i++)
                InvokeResolveHandler(resolveHandlers[i].callback, resolveHandlers[i].rejectable);
        }

        ClearHandlers();
    }

    /// <summary>
    /// Invoke all progress handlers.
    /// </summary>
    private void InvokeProgressHandlers(float progress)
    {
        if (progressHandlers != null)
        {
            for (int i = 0, maxI = progressHandlers.Count; i < maxI; i++)
                InvokeProgressHandler(progressHandlers[i].callback, progressHandlers[i].rejectable, progress);
        }
    }

    /// <summary>
    /// Reject the promise with an exception.
    /// </summary>
    public void SetException(Exception ex)
    {
        //            Argument.NotNull(() => ex);

        if (CurState != PromiseState.Pending)
        {
            throw new PromiseStateException(
                "Attempt to reject a promise that is already in state: " + CurState
                + ", a promise can only be rejected when it is still in state: "
                + PromiseState.Pending
            );
        }

        rejectionException = ex;
        CurState = PromiseState.Rejected;

        if (EnablePromiseTracking)
        {
            PendingPromises.Remove(this);
        }

        InvokeRejectHandlers(ex);
    }


    /// <summary>
    /// Resolve the promise with a particular value.
    /// </summary>
    public void Resolve()
    {
        if (CurState != PromiseState.Pending)
        {
            throw new PromiseStateException(
                "Attempt to resolve a promise that is already in state: " + CurState
                + ", a promise can only be resolved when it is still in state: "
                + PromiseState.Pending
            );
        }

        CurState = PromiseState.Resolved;

        if (EnablePromiseTracking)
        {
            PendingPromises.Remove(this);
        }

        InvokeResolveHandlers();
    }


    /// <summary>
    /// Report progress on the promise.
    /// </summary>
    public void ReportProgress(float progress)
    {
        if (CurState != PromiseState.Pending)
        {
            throw new PromiseStateException(
                "Attempt to report progress on a promise that is already in state: "
                + CurState + ", a promise can only report progress when it is still in state: "
                + PromiseState.Pending
            );
        }

        InvokeProgressHandlers(progress);
    }


    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// onRejected is called on error.
    /// </summary>
    public void Done(Action onResolved, Action<Exception> onRejected)
    {
        Then(onResolved, onRejected)
            .Catch(ex =>
                PropagateUnhandledException(this, ex)
            );
    }

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// Adds a default error handler.
    /// </summary>
    public void Done(Action onResolved)
    {
        Then(onResolved)
            .Catch(ex =>
                PropagateUnhandledException(this, ex)
            );
    }

    /// <summary>
    /// Complete the promise. Adds a defualt error handler.
    /// </summary>
    public void Done()
    {
        if (CurState == PromiseState.Resolved)
            return;

        Catch(ex => PropagateUnhandledException(this, ex));
    }

    /// <summary>
    /// Set the name of the promise, useful for debugging.
    /// </summary>
    public IPromise WithName(string name)
    {
        this.Name = name;
        return this;
    }

    /// <summary>
    /// Handle errors for the promise. 
    /// </summary>
    public IPromise Catch(Action<Exception> onRejected)
    {
        //            Argument.NotNull(() => onRejected);

        if (CurState == PromiseState.Resolved)
        {
            return this;
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action resolveHandler = () => resultPromise.Resolve();

        Action<Exception> rejectHandler = ex =>
        {
            try
            {
                onRejected(ex);
                resultPromise.Resolve();
            }
            catch (Exception callbackException)
            {
                resultPromise.SetException(callbackException);
            }
        };

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

        return resultPromise;
    }

    /// <summary>
    /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback that chains a non-value promise.
    /// </summary>
    public IPromise Then(Func<IPromise> onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback.
    /// </summary>
    public IPromise Then(Action onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved, Func<Exception, IPromise<ConvertedT>> onRejected)
    {
        return Then(onResolved, onRejected, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected)
    {
        return Then(onResolved, onRejected, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// </summary>
    public IPromise Then(Action onResolved, Action<Exception> onRejected)
    {
        return Then(onResolved, onRejected, null);
    }

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(
        Func<IPromise<ConvertedT>> onResolved,
        Func<Exception, IPromise<ConvertedT>> onRejected,
        Action<float> onProgress)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                return onResolved();
            }
            catch (Exception ex)
            {
                return TaskCompletionSource<ConvertedT>.Rejected(ex);
            }
        }

        // This version of the function must supply an onResolved.
        // Otherwise there is now way to get the converted value to pass to the resulting promise.
        //            Argument.NotNull(() => onResolved);

        var resultPromise = new TaskCompletionSource<ConvertedT>();
        resultPromise.WithName(Name);

        Action resolveHandler = () =>
        {
            onResolved()
                .Progress(progress => resultPromise.ReportProgress(progress))
                .Then(
                    // Should not be necessary to specify the arg type on the next line, but Unity (mono) has an internal compiler error otherwise.
                    chainedValue => resultPromise.SetResult(chainedValue),
                    ex => resultPromise.SetException(ex)
                );
        };

        Action<Exception> rejectHandler = ex =>
        {
            if (onRejected == null)
            {
                resultPromise.SetException(ex);
                return;
            }

            try
            {
                onRejected(ex)
                    .Then(
                        chainedValue => resultPromise.SetResult(chainedValue),
                        callbackEx => resultPromise.SetException(callbackEx)
                    );
            }
            catch (Exception callbackEx)
            {
                resultPromise.SetException(callbackEx);
            }
        };

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        if (onProgress != null)
        {
            ProgressHandlers(this, onProgress);
        }

        return resultPromise;
    }

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                return onResolved();
            }
            catch (Exception ex)
            {
                return Rejected(ex);
            }
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action resolveHandler;
        if (onResolved != null)
        {
            resolveHandler = () =>
            {
                onResolved()
                    .Progress(progress => resultPromise.ReportProgress(progress))
                    .Then(
                        () => resultPromise.Resolve(),
                        ex => resultPromise.SetException(ex)
                    );
            };
        }
        else
        {
            resolveHandler = resultPromise.Resolve;
        }

        Action<Exception> rejectHandler;
        if (onRejected != null)
        {
            rejectHandler = ex =>
            {
                onRejected(ex);
                resultPromise.SetException(ex);
            };
        }
        else
        {
            rejectHandler = resultPromise.SetException;
        }

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        if (onProgress != null)
        {
            ProgressHandlers(this, onProgress);
        }

        return resultPromise;
    }

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// </summary>
    public IPromise Then(Action onResolved, Action<Exception> onRejected, Action<float> onProgress)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                onResolved();
                return this;
            }
            catch (Exception ex)
            {
                return Rejected(ex);
            }
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action resolveHandler;
        if (onResolved != null)
        {
            resolveHandler = () =>
            {
                onResolved();
                resultPromise.Resolve();
            };
        }
        else
        {
            resolveHandler = resultPromise.Resolve;
        }

        Action<Exception> rejectHandler;
        if (onRejected != null)
        {
            rejectHandler = ex =>
            {
                if (onRejected != null)
                {
                    onRejected(ex);
                    resultPromise.Resolve();
                    return;
                }

                resultPromise.SetException(ex);
            };
        }
        else
        {
            rejectHandler = resultPromise.SetException;
        }

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        if (onProgress != null)
        {
            ProgressHandlers(this, onProgress);
        }

        return resultPromise;
    }

    /// <summary>
    /// Helper function to invoke or register resolve/reject handlers.
    /// </summary>
    private void ActionHandlers(IRejectable resultPromise, Action resolveHandler, Action<Exception> rejectHandler)
    {
        if (CurState == PromiseState.Resolved)
        {
            InvokeResolveHandler(resolveHandler, resultPromise);
        }
        else if (CurState == PromiseState.Rejected)
        {
            InvokeRejectHandler(rejectHandler, resultPromise, rejectionException);
        }
        else
        {
            AddResolveHandler(resolveHandler, resultPromise);
            AddRejectHandler(rejectHandler, resultPromise);
        }
    }

    /// <summary>
    /// Helper function to invoke or register progress handlers.
    /// </summary>
    private void ProgressHandlers(IRejectable resultPromise, Action<float> progressHandler)
    {
        if (CurState == PromiseState.Pending)
        {
            AddProgressHandler(progressHandler, resultPromise);
        }
    }

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    public IPromise ThenAll(Func<IEnumerable<IPromise>> chain)
    {
        return Then(() => All(chain()));
    }

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Converts to a non-value promise.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    public IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain)
    {
        return Then(() => TaskCompletionSource<ConvertedT>.All(chain()));
    }

    /// <summary>
    /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
    /// Returns a promise of a collection of the resolved results.
    /// </summary>
    public static IPromise All(params IPromise[] promises)
    {
        return All((IEnumerable<IPromise>)promises); // Cast is required to force use of the other All function.
    }

    /// <summary>
    /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
    /// Returns a promise of a collection of the resolved results.
    /// </summary>
    public static IPromise All(IEnumerable<IPromise> promises)
    {
        var promisesArray = promises.ToArray();
        if (promisesArray.Length == 0)
        {
            return Resolved();
        }

        var remainingCount = promisesArray.Length;
        var resultPromise = new Task();
        resultPromise.WithName("All");
        var progress = new float[remainingCount];

        promisesArray.Each((promise, index) =>
        {
            promise
                .Progress(v =>
                {
                    progress[index] = v;
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                        resultPromise.ReportProgress(progress.Average());
                    }
                })
                .Then(() =>
                {
                    progress[index] = 1f;

                    --remainingCount;
                    if (remainingCount <= 0 && resultPromise.CurState == PromiseState.Pending)
                    {
                            // This will never happen if any of the promises errorred.
                            resultPromise.Resolve();
                    }
                })
                .Catch(ex =>
                {
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                            // If a promise errorred and the result promise is still pending, reject it.
                            resultPromise.SetException(ex);
                    }
                })
                .Done();
        });

        return resultPromise;
    }

    /// <summary>
    /// Chain a sequence of operations using promises.
    /// Reutrn a collection of functions each of which starts an async operation and yields a promise.
    /// Each function will be called and each promise resolved in turn.
    /// The resulting promise is resolved after each promise is resolved in sequence.
    /// </summary>
    public IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain)
    {
        return Then(() => Sequence(chain()));
    }

    /// <summary>
    /// Chain a number of operations using promises.
    /// Takes a number of functions each of which starts an async operation and yields a promise.
    /// </summary>
    public static IPromise Sequence(params Func<IPromise>[] fns)
    {
        return Sequence((IEnumerable<Func<IPromise>>)fns);
    }

    /// <summary>
    /// Chain a sequence of operations using promises.
    /// Takes a collection of functions each of which starts an async operation and yields a promise.
    /// </summary>
    public static IPromise Sequence(IEnumerable<Func<IPromise>> fns)
    {
        var promise = new Task();

        int count = 0;

        fns.Aggregate(
            Resolved(),
            (prevPromise, fn) =>
            {
                int itemSequence = count;
                ++count;

                return prevPromise
                        .Then(() =>
                        {
                            var sliceLength = 1f / count;
                            promise.ReportProgress(sliceLength * itemSequence);
                            return fn();
                        })
                        .Progress(v =>
                        {
                            var sliceLength = 1f / count;
                            promise.ReportProgress(sliceLength * (v + itemSequence));
                        })
                ;
            }
        )
        .Then((Action)promise.Resolve)
        .Catch(promise.SetException);

        return promise;
    }

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// </summary>
    public IPromise ThenRace(Func<IEnumerable<IPromise>> chain)
    {
        return Then(() => Race(chain()));
    }

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Converts to a value promise.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// </summary>
    public IPromise<ConvertedT> ThenRace<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain)
    {
        return Then(() => TaskCompletionSource<ConvertedT>.Race(chain()));
    }

    /// <summary>
    /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
    /// Returns the value from the first promise that has resolved.
    /// </summary>
    public static IPromise Race(params IPromise[] promises)
    {
        return Race((IEnumerable<IPromise>)promises); // Cast is required to force use of the other function.
    }

    /// <summary>
    /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
    /// Returns the value from the first promise that has resolved.
    /// </summary>
    public static IPromise Race(IEnumerable<IPromise> promises)
    {
        var promisesArray = promises.ToArray();
        if (promisesArray.Length == 0)
        {
            throw new InvalidOperationException("At least 1 input promise must be provided for Race");
        }

        var resultPromise = new Task();
        resultPromise.WithName("Race");

        var progress = new float[promisesArray.Length];

        promisesArray.Each((promise, index) =>
        {
            promise
                .Progress(v =>
                {
                    progress[index] = v;
                    resultPromise.ReportProgress(progress.Max());
                })
                .Catch(ex =>
                {
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                            // If a promise errorred and the result promise is still pending, reject it.
                            resultPromise.SetException(ex);
                    }
                })
                .Then(() =>
                {
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                        resultPromise.Resolve();
                    }
                })
                .Done();
        });

        return resultPromise;
    }

    /// <summary>
    /// Convert a simple value directly into a resolved promise.
    /// </summary>
    private static IPromise resolvedPromise = new Task(PromiseState.Resolved);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IPromise Resolved()
    {
        return resolvedPromise;
    }

    /// <summary>
    /// Convert an exception directly into a rejected promise.
    /// </summary>
    public static IPromise Rejected(Exception ex)
    {
        //            Argument.NotNull(() => ex);

        var promise = new Task(PromiseState.Rejected);
        promise.rejectionException = ex;
        return promise;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public IPromise Finally(Action onComplete)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                onComplete();
                return this;
            }
            catch (Exception ex)
            {
                return Rejected(ex);
            }
        }

        var promise = new Task();
        promise.WithName(Name);

        this.Then((Action)promise.Resolve);
        this.Catch(e =>
        {
            try
            {
                onComplete();
                promise.SetException(e);
            }
            catch (Exception ne)
            {
                promise.SetException(ne);
            }
        });

        return promise.Then(onComplete);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public IPromise ContinueWith(Func<IPromise> onComplete)
    {
        var promise = new Task();
        promise.WithName(Name);

        this.Then((Action)promise.Resolve);
        this.Catch(e => promise.Resolve());

        return promise.Then(onComplete);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ConvertedT"></typeparam>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public IPromise<ConvertedT> ContinueWith<ConvertedT>(Func<IPromise<ConvertedT>> onComplete)
    {
        var promise = new Task();
        promise.WithName(Name);

        this.Then((Action)promise.Resolve);
        this.Catch(e => promise.Resolve());

        return promise.Then(onComplete);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onProgress"></param>
    /// <returns></returns>
    public IPromise Progress(Action<float> onProgress)
    {
        if (CurState == PromiseState.Pending && onProgress != null)
        {
            ProgressHandlers(this, onProgress);
        }
        return this;
    }

    /// <summary>
    /// Raises the UnhandledException event.
    /// </summary>
    internal static void PropagateUnhandledException(object sender, Exception ex)
    {
        unhandlerException?.Invoke(sender, new ExceptionEventArgs(ex));
    }
}