using System;
using System.Collections.Generic;
/// <summary>
/// Implements a C# promise.
/// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
/// </summary>
public interface IPromise<PromisedT>
{
    /// <summary>
    /// Gets the id of the promise, useful for referencing the promise during runtime.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Set the name of the promise, useful for debugging.
    /// </summary>
    IPromise<PromisedT> WithName(string name);

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// onRejected is called on error.
    /// </summary>
    void Done(Action<PromisedT> onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// Adds a default error handler.
    /// </summary>
    void Done(Action<PromisedT> onResolved);

    /// <summary>
    /// Complete the promise. Adds a default error handler.
    /// </summary>
    void Done();

    /// <summary>
    /// Handle errors for the promise. 
    /// </summary>
    IPromise Catch(Action<Exception> onRejected);

    /// <summary>
    /// Handle errors for the promise. 
    /// </summary>
    IPromise<PromisedT> Catch(Func<Exception, PromisedT> onRejected);

    /// <summary>
    /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved);

    /// <summary>
    /// Add a resolved callback that chains a non-value promise.
    /// </summary>
    IPromise Then(Func<PromisedT, IPromise> onResolved);

    /// <summary>
    /// Add a resolved callback.
    /// </summary>
    IPromise Then(Action<PromisedT> onResolved);

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(
        Func<PromisedT, IPromise<ConvertedT>> onResolved,
        Func<Exception, IPromise<ConvertedT>> onRejected
    );

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// </summary>
    IPromise Then(Action<PromisedT> onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(
        Func<PromisedT, IPromise<ConvertedT>> onResolved,
        Func<Exception, IPromise<ConvertedT>> onRejected,
        Action<float> onProgress
    );

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress);

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// </summary>
    IPromise Then(Action<PromisedT> onResolved, Action<Exception> onRejected, Action<float> onProgress);

    /// <summary>
    /// Return a new promise with a different value.
    /// May also change the type of the value.
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, ConvertedT> transform);

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Returns a promise for a collection of the resolved results.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain);

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Converts to a non-value promise.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    IPromise ThenAll(Func<PromisedT, IEnumerable<IPromise>> chain);

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// Yields the value from the first promise that has resolved.
    /// </summary>
    IPromise<ConvertedT> ThenRace<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain);

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Converts to a non-value promise.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// Yields the value from the first promise that has resolved.
    /// </summary>
    IPromise ThenRace(Func<PromisedT, IEnumerable<IPromise>> chain);

    /// <summary> 
    /// Add a finally callback. 
    /// Finally callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
    /// The returned promise will be resolved or rejected, as per the preceding promise.
    /// </summary> 
    IPromise<PromisedT> Finally(Action onComplete);

    /// <summary>
    /// Add a callback that chains a non-value promise.
    /// ContinueWith callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
    /// The state of the returning promise will be based on the new non-value promise, not the preceding (rejected or resolved) promise.
    /// </summary>
    IPromise ContinueWith(Func<IPromise> onResolved);

    /// <summary> 
    /// Add a callback that chains a value promise (optionally converting to a different value type).
    /// ContinueWith callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
    /// The state of the returning promise will be based on the new value promise, not the preceding (rejected or resolved) promise.
    /// </summary> 
    IPromise<ConvertedT> ContinueWith<ConvertedT>(Func<IPromise<ConvertedT>> onComplete);

    /// <summary>
    /// Add a progress callback.
    /// Progress callbacks will be called whenever the promise owner reports progress towards the resolution
    /// of the promise.
    /// </summary>
    IPromise<PromisedT> Progress(Action<float> onProgress);
}
