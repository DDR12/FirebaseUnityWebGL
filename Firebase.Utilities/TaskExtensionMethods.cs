
using Firebase.WebGL.Threading;

namespace Firebase
{
    /// <summary>
    /// Helper extension methods for <see cref="Task"/> and its children.
    /// </summary>
    public static class TaskExtensionMethods
    {
        /// <summary>
        /// Returns true if the task completed without errors or being cancelled, this is different from <see cref="Task.IsCompleted"/> becasue a task can complete with an error or complete with cancellation.
        /// </summary>
        /// <param name="task">The task to check if it is successfull.</param>
        /// <returns>True if the task completed without errors or cancellation, false otherwise.</returns>
        public static bool IsSuccess(this Task task)
        {
            PreconditionUtilities.CheckNotNull(task, nameof(task));
            return task.IsCompleted && task.IsCanceled == false && task.IsFaulted == false;
        }
    }
}
