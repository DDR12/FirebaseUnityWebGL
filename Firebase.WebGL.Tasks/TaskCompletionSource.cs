using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Implements a C# promise.
/// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
/// </summary>
public class TaskCompletionSource<PromisedT> : IPromise<PromisedT>, IPendingPromise<PromisedT>, IPromiseInfo
{
    /// <summary>
    /// The exception when the promise is rejected.
    /// </summary>
    private Exception rejectionException;

    /// <summary>
    /// The value when the promises is resolved.
    /// </summary>
    private PromisedT resolveValue;

    /// <summary>
    /// Error handler.
    /// </summary>
    private List<RejectHandler> rejectHandlers;

    /// <summary>
    /// Progress handlers.
    /// </summary>
    private List<ProgressHandler> progressHandlers;

    /// <summary>
    /// Completed handlers that accept a value.
    /// </summary>
    private List<Action<PromisedT>> resolveCallbacks;
    private List<IRejectable> resolveRejectables;

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
    public TaskCompletionSource()
    {
        this.CurState = PromiseState.Pending;
        this.Id = PromiseHelpers.NextPromiseID();

        if (Task.EnablePromiseTracking)
        {
            Task.PendingPromises.Add(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resolver"></param>
    public TaskCompletionSource(Action<Action<PromisedT>, Action<Exception>> resolver)
    {
        this.CurState = PromiseState.Pending;
        this.Id = PromiseHelpers.NextPromiseID();

        if (Task.EnablePromiseTracking)
        {
            Task.PendingPromises.Add(this);
        }

        try
        {
            resolver(SetResult, SetException);
        }
        catch (Exception ex)
        {
            SetException(ex);
        }
    }

    private TaskCompletionSource(PromiseState initialState)
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

        rejectHandlers.Add(new RejectHandler { callback = onRejected, rejectable = rejectable });
    }

    /// <summary>
    /// Add a resolve handler for this promise.
    /// </summary>
    private void AddResolveHandler(Action<PromisedT> onResolved, IRejectable rejectable)
    {
        if (resolveCallbacks == null)
        {
            resolveCallbacks = new List<Action<PromisedT>>();
        }

        if (resolveRejectables == null)
        {
            resolveRejectables = new List<IRejectable>();
        }

        resolveCallbacks.Add(onResolved);
        resolveRejectables.Add(rejectable);
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
    /// Invoke a single handler.
    /// </summary>
    private void InvokeHandler<T>(Action<T> callback, IRejectable rejectable, T value)
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
    /// Helper function clear out all handlers after resolution or rejection.
    /// </summary>
    private void ClearHandlers()
    {
        rejectHandlers = null;
        resolveCallbacks = null;
        resolveRejectables = null;
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
                InvokeHandler(rejectHandlers[i].callback, rejectHandlers[i].rejectable, ex);
        }

        ClearHandlers();
    }

    /// <summary>
    /// Invoke all resolve handlers.
    /// </summary>
    private void InvokeResolveHandlers(PromisedT value)
    {
        if (resolveCallbacks != null)
        {
            for (int i = 0, maxI = resolveCallbacks.Count; i < maxI; i++)
            {
                InvokeHandler(resolveCallbacks[i], resolveRejectables[i], value);
            }
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
            for (int i = 0, maxI = progressHandlers.Count; i < maxI; ++i)
                InvokeHandler(progressHandlers[i].callback, progressHandlers[i].rejectable, progress);
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

        if (Task.EnablePromiseTracking)
        {
            Task.PendingPromises.Remove(this);
        }

        InvokeRejectHandlers(ex);
    }

    /// <summary>
    /// Resolve the promise with a particular value.
    /// </summary>
    public void SetResult(PromisedT value)
    {
        if (CurState != PromiseState.Pending)
        {
            throw new PromiseStateException(
                "Attempt to resolve a promise that is already in state: " + CurState
                + ", a promise can only be resolved when it is still in state: "
                + PromiseState.Pending
            );
        }

        resolveValue = value;
        CurState = PromiseState.Resolved;

        if (Task.EnablePromiseTracking)
        {
            Task.PendingPromises.Remove(this);
        }

        InvokeResolveHandlers(value);
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
    public void Done(Action<PromisedT> onResolved, Action<Exception> onRejected)
    {
        Then(onResolved, onRejected)
            .Catch(ex =>
                Task.PropagateUnhandledException(this, ex)
            );
    }

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// Adds a default error handler.
    /// </summary>
    public void Done(Action<PromisedT> onResolved)
    {
        Then(onResolved)
            .Catch(ex =>
                Task.PropagateUnhandledException(this, ex)
            );
    }

    /// <summary>
    /// Complete the promise. Adds a default error handler.
    /// </summary>
    public void Done()
    {
        if (CurState == PromiseState.Resolved)
            return;

        Catch(ex =>
            Task.PropagateUnhandledException(this, ex)
        );
    }

    /// <summary>
    /// Set the name of the promise, useful for debugging.
    /// </summary>
    public IPromise<PromisedT> WithName(string name)
    {
        this.Name = name;
        return this;
    }

    /// <summary>
    /// Handle errors for the promise. 
    /// </summary>
    public IPromise Catch(Action<Exception> onRejected)
    {
        if (CurState == PromiseState.Resolved)
        {
            return Task.Resolved();
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action<PromisedT> resolveHandler = _ => resultPromise.Resolve();

        Action<Exception> rejectHandler = ex =>
        {
            try
            {
                onRejected(ex);
                resultPromise.Resolve();
            }
            catch (Exception cbEx)
            {
                resultPromise.SetException(cbEx);
            }
        };

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

        return resultPromise;
    }

    /// <summary>
    /// Handle errors for the promise.
    /// </summary>
    public IPromise<PromisedT> Catch(Func<Exception, PromisedT> onRejected)
    {
        if (CurState == PromiseState.Resolved)
        {
            return this;
        }

        var resultPromise = new TaskCompletionSource<PromisedT>();
        resultPromise.WithName(Name);

        Action<PromisedT> resolveHandler = v => resultPromise.SetResult(v);

        Action<Exception> rejectHandler = ex =>
        {
            try
            {
                resultPromise.SetResult(onRejected(ex));
            }
            catch (Exception cbEx)
            {
                resultPromise.SetException(cbEx);
            }
        };

        ActionHandlers(resultPromise, resolveHandler, rejectHandler);
        ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

        return resultPromise;
    }

    /// <summary>
    /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback that chains a non-value promise.
    /// </summary>
    public IPromise Then(Func<PromisedT, IPromise> onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback.
    /// </summary>
    public IPromise Then(Action<PromisedT> onResolved)
    {
        return Then(onResolved, null, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(
        Func<PromisedT, IPromise<ConvertedT>> onResolved,
        Func<Exception, IPromise<ConvertedT>> onRejected
    )
    {
        return Then(onResolved, onRejected, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    public IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected)
    {
        return Then(onResolved, onRejected, null);
    }

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// </summary>
    public IPromise Then(Action<PromisedT> onResolved, Action<Exception> onRejected)
    {
        return Then(onResolved, onRejected, null);
    }


    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(
        Func<PromisedT, IPromise<ConvertedT>> onResolved,
        Func<Exception, IPromise<ConvertedT>> onRejected,
        Action<float> onProgress
    )
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                return onResolved(resolveValue);
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

        Action<PromisedT> resolveHandler = v =>
        {
            onResolved(v)
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
    public IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                return onResolved(resolveValue);
            }
            catch (Exception ex)
            {
                return Task.Rejected(ex);
            }
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action<PromisedT> resolveHandler = v =>
        {
            if (onResolved != null)
            {
                onResolved(v)
                    .Progress(progress => resultPromise.ReportProgress(progress))
                    .Then(
                        () => resultPromise.Resolve(),
                        ex => resultPromise.SetException(ex)
                    );
            }
            else
            {
                resultPromise.Resolve();
            }
        };

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
    public IPromise Then(Action<PromisedT> onResolved, Action<Exception> onRejected, Action<float> onProgress)
    {
        if (CurState == PromiseState.Resolved)
        {
            try
            {
                onResolved(resolveValue);
                return Task.Resolved();
            }
            catch (Exception ex)
            {
                return Task.Rejected(ex);
            }
        }

        var resultPromise = new Task();
        resultPromise.WithName(Name);

        Action<PromisedT> resolveHandler = v =>
        {
            if (onResolved != null)
            {
                onResolved(v);
            }

            resultPromise.Resolve();
        };

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
    /// Return a new promise with a different value.
    /// May also change the type of the value.
    /// </summary>
    public IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, ConvertedT> transform)
    {
        //            Argument.NotNull(() => transform);
        return Then(value => TaskCompletionSource<ConvertedT>.Resolved(transform(value)));
    }

    /// <summary>
    /// Helper function to invoke or register resolve/reject handlers.
    /// </summary>
    private void ActionHandlers(IRejectable resultPromise, Action<PromisedT> resolveHandler, Action<Exception> rejectHandler)
    {
        if (CurState == PromiseState.Resolved)
        {
            InvokeHandler(resolveHandler, resultPromise, resolveValue);
        }
        else if (CurState == PromiseState.Rejected)
        {
            InvokeHandler(rejectHandler, resultPromise, rejectionException);
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
    /// Chain a number of operations using promises.
    /// Returns the value of the first promise that resolves, or otherwise the exception thrown by the last operation.
    /// </summary>
    public static IPromise<T> First<T>(params Func<IPromise<T>>[] fns)
    {
        return First((IEnumerable<Func<IPromise<T>>>)fns);
    }

    /// <summary>
    /// Chain a number of operations using promises.
    /// Returns the value of the first promise that resolves, or otherwise the exception thrown by the last operation.
    /// </summary>
    public static IPromise<T> First<T>(IEnumerable<Func<IPromise<T>>> fns)
    {
        var promise = new TaskCompletionSource<T>();

        int count = 0;

        fns.Aggregate(
            TaskCompletionSource<T>.Rejected(null),
            (prevPromise, fn) =>
            {
                int itemSequence = count;
                ++count;

                var newPromise = new TaskCompletionSource<T>();
                prevPromise
                    .Progress(v =>
                    {
                        var sliceLength = 1f / count;
                        promise.ReportProgress(sliceLength * (v + itemSequence));
                    })
                    .Then((Action<T>)newPromise.SetResult)
                    .Catch(ex =>
                    {
                        var sliceLength = 1f / count;
                        promise.ReportProgress(sliceLength * itemSequence);

                        fn()
                            .Then(value => newPromise.SetResult(value))
                            .Catch(newPromise.SetException)
                            .Done()
                        ;
                    })
                ;
                return newPromise;
            })
        .Then(value => promise.SetResult(value))
        .Catch(ex =>
        {
            promise.ReportProgress(1f);
            promise.SetException(ex);
        });

        return promise;
    }

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Returns a promise for a collection of the resolved results.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    public IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain)
    {
        return Then(value => TaskCompletionSource<ConvertedT>.All(chain(value)));
    }

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Converts to a non-value promise.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    public IPromise ThenAll(Func<PromisedT, IEnumerable<IPromise>> chain)
    {
        return Then(value => Task.All(chain(value)));
    }

    /// <summary>
    /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
    /// Returns a promise of a collection of the resolved results.
    /// </summary>
    public static IPromise<IEnumerable<PromisedT>> All(params IPromise<PromisedT>[] promises)
    {
        return All((IEnumerable<IPromise<PromisedT>>)promises); // Cast is required to force use of the other All function.
    }

    /// <summary>
    /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
    /// Returns a promise of a collection of the resolved results.
    /// </summary>
    public static IPromise<IEnumerable<PromisedT>> All(IEnumerable<IPromise<PromisedT>> promises)
    {
        var promisesArray = promises.ToArray();
        if (promisesArray.Length == 0)
        {
            return TaskCompletionSource<IEnumerable<PromisedT>>.Resolved(Enumerable.Empty<PromisedT>());
        }

        var remainingCount = promisesArray.Length;
        var results = new PromisedT[remainingCount];
        var progress = new float[remainingCount];
        var resultPromise = new TaskCompletionSource<IEnumerable<PromisedT>>();
        resultPromise.WithName("All");

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
                .Then(result =>
                {
                    progress[index] = 1f;
                    results[index] = result;

                    --remainingCount;
                    if (remainingCount <= 0 && resultPromise.CurState == PromiseState.Pending)
                    {
                        // This will never happen if any of the promises errorred.
                        resultPromise.SetResult(results);
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
    /// Takes a function that yields an enumerable of promises.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// Yields the value from the first promise that has resolved.
    /// </summary>
    public IPromise<ConvertedT> ThenRace<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain)
    {
        return Then(value => TaskCompletionSource<ConvertedT>.Race(chain(value)));
    }

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Converts to a non-value promise.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// Yields the value from the first promise that has resolved.
    /// </summary>
    public IPromise ThenRace(Func<PromisedT, IEnumerable<IPromise>> chain)
    {
        return Then(value => Task.Race(chain(value)));
    }

    /// <summary>
    /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
    /// Returns the value from the first promise that has resolved.
    /// </summary>
    public static IPromise<PromisedT> Race(params IPromise<PromisedT>[] promises)
    {
        return Race((IEnumerable<IPromise<PromisedT>>)promises); // Cast is required to force use of the other function.
    }

    /// <summary>
    /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
    /// Returns the value from the first promise that has resolved.
    /// </summary>
    public static IPromise<PromisedT> Race(IEnumerable<IPromise<PromisedT>> promises)
    {
        var promisesArray = promises.ToArray();
        if (promisesArray.Length == 0)
        {
            throw new InvalidOperationException(
                "At least 1 input promise must be provided for Race"
            );
        }

        var resultPromise = new TaskCompletionSource<PromisedT>();
        resultPromise.WithName("Race");

        var progress = new float[promisesArray.Length];

        promisesArray.Each((promise, index) =>
        {
            promise
                .Progress(v =>
                {
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                        progress[index] = v;
                        resultPromise.ReportProgress(progress.Max());
                    }
                })
                .Then(result =>
                {
                    if (resultPromise.CurState == PromiseState.Pending)
                    {
                        resultPromise.SetResult(result);
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
    /// Convert a simple value directly into a resolved promise.
    /// </summary>
    public static IPromise<PromisedT> Resolved(PromisedT promisedValue)
    {
        var promise = new TaskCompletionSource<PromisedT>(PromiseState.Resolved);
        promise.resolveValue = promisedValue;
        return promise;
    }

    /// <summary>
    /// Convert an exception directly into a rejected promise.
    /// </summary>
    public static IPromise<PromisedT> Rejected(Exception ex)
    {
        //            Argument.NotNull(() => ex);

        var promise = new TaskCompletionSource<PromisedT>(PromiseState.Rejected);
        promise.rejectionException = ex;
        return promise;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public IPromise<PromisedT> Finally(Action onComplete)
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

        var promise = new TaskCompletionSource<PromisedT>();
        promise.WithName(Name);

        this.Then((Action<PromisedT>)promise.SetResult);
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

        return promise.Then(v =>
        {
            onComplete();
            return v;
        });
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

        this.Then(x => promise.Resolve());
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

        this.Then(x => promise.Resolve());
        this.Catch(e => promise.Resolve());

        return promise.Then(onComplete);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onProgress"></param>
    /// <returns></returns>
    public IPromise<PromisedT> Progress(Action<float> onProgress)
    {
        if (CurState == PromiseState.Pending && onProgress != null)
        {
            ProgressHandlers(this, onProgress);
        }
        return this;
    }
}