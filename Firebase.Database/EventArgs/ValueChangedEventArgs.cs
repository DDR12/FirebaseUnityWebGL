namespace Firebase.Database
{
    /// <summary>
    /// Event arguments passed with the <see cref="Query.ValueChanged"/>
    /// </summary>
    public sealed class ValueChangedEventArgs
    {
        /// <summary>
        /// Gets the database error if one exists.
        /// </summary>
        public DatabaseError DatabaseError { get; }
        /// <summary>
        /// Gets the snapshot for this value update event if it exists.
        /// </summary>
        public DataSnapshot Snapshot { get; }

        internal ValueChangedEventArgs(DataSnapshot snapshot)
        {
            this.Snapshot = snapshot;
        }
        internal ValueChangedEventArgs(DatabaseError error)
        {
            this.DatabaseError = error;
        }
    }
}
