namespace Hootsuite.Domain
{
    /// <summary>
    /// Enum MessageState
    /// </summary>
    public enum MessageState
    {
        /// <summary>
        /// The scheduled
        /// </summary>
        SCHEDULED,
        /// <summary>
        /// The pending approval
        /// </summary>
        PENDING_APPROVAL,
        /// <summary>
        /// The approved
        /// </summary>
        APPROVED,
        /// <summary>
        /// The submitted
        /// </summary>
        SUBMITTED,
        /// <summary>
        /// The sent
        /// </summary>
        SENT,
        /// <summary>
        /// The send failed permanently
        /// </summary>
        SEND_FAILED_PERMANENTLY,
        /// <summary>
        /// The deleted
        /// </summary>
        DELETED,
        /// <summary>
        /// The rejected
        /// </summary>
        REJECTED
    }
}
