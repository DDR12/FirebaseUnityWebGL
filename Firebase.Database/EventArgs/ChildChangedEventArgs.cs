using System;

namespace Firebase.Database
{
    public sealed class ChildChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The presence of a <see cref="Firebase.Database.DatabaseError"/> indicates that there was an issue subscribing the event to the given DatabaseReference location.
        /// </summary>
        public DatabaseError DatabaseError { get; }
        /// <summary>
        /// The key name of sibling location ordered before the new child.
        /// This will be null for the first child node of a location.
        /// </summary>
        public string PreviouseChildName { get; }
        /// <summary>
        /// Gets the data snapshot for this update if it exists.
        /// </summary>
        public DataSnapshot Snapshot { get; }

        internal ChildChangedEventArgs(DataSnapshot snapshot, string previousChildName)
        {
            Snapshot = snapshot;
            PreviouseChildName = previousChildName;
        }
        internal ChildChangedEventArgs(DatabaseError error)
        {
            DatabaseError = error;
        }
    }
}
