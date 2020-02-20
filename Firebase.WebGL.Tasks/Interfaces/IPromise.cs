using System;
using System.Collections.Generic;
/// <summary>
/// Implements a non-generic C# promise, this is a promise that simply resolves without delivering a value.
/// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
/// </summary>
public interface IPromise
{
    /// <summary>
    /// ID of the promise, useful for debugging.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Set the name of the promise, useful for debugging.
    /// </summary>
    IPromise WithName(string name);

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// onRejected is called on error.
    /// </summary>
    void Done(Action onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Completes the promise. 
    /// onResolved is called on successful completion.
    /// Adds a default error handler.
    /// </summary>
    void Done(Action onResolved);

    /// <summary>
    /// Complete the promise. Adds a default error handler.
    /// </summary>
    void Done();

    /// <summary>
    /// Handle errors for the promise. 
    /// </summary>
    IPromise Catch(Action<Exception> onRejected);

    /// <summary>
    /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved);

    /// <summary>
    /// Add a resolved callback that chains a non-value promise.
    /// </summary>
    IPromise Then(Func<IPromise> onResolved);

    /// <summary>
    /// Add a resolved callback.
    /// </summary>
    IPromise Then(Action onResolved);

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved, Func<Exception, IPromise<ConvertedT>> onRejected);

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Add a resolved callback and a rejected callback.
    /// </summary>
    IPromise Then(Action onResolved, Action<Exception> onRejected);

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a value promise (optionally converting to a different value type).
    /// </summary>
    IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved, Func<Exception, IPromise<ConvertedT>> onRejected, Action<float> onProgress);

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// The resolved callback chains a non-value promise.
    /// </summary>
    IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress);

    /// <summary>
    /// Add a resolved callback, a rejected callback and a progress callback.
    /// </summary>
    IPromise Then(Action onResolved, Action<Exception> onRejected, Action<float> onProgress);

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    IPromise ThenAll(Func<IEnumerable<IPromise>> chain);

    /// <summary>
    /// Chain an enumerable of promises, all of which must resolve.
    /// Converts to a non-value promise.
    /// The resulting promise is resolved when all of the promises have resolved.
    /// It is rejected as soon as any of the promises have been rejected.
    /// </summary>
    IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain);

    /// <summary>
    /// Chain a sequence of operations using promises.
    /// Reutrn a collection of functions each of which starts an async operation and yields a promise.
    /// Each function will be called and each promise resolved in turn.
    /// The resulting promise is resolved after each promise is resolved in sequence.
    /// </summary>
    IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain);

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// </summary>
    IPromise ThenRace(Func<IEnumerable<IPromise>> chain);

    /// <summary>
    /// Takes a function that yields an enumerable of promises.
    /// Converts to a value promise.
    /// Returns a promise that resolves when the first of the promises has resolved.
    /// </summary>
    IPromise<ConvertedT> ThenRace<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain);

    /// <summary> 
    /// Add a finally callback. 
    /// Finally callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
    /// The returned promise will be resolved or rejected, as per the preceding promise.
    /// </summary> 
    IPromise Finally(Action onComplete);

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
    IPromise Progress(Action<float> onProgress);
}
