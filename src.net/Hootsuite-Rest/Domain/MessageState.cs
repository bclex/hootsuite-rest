namespace Hootsuite.Domain
{
    public enum MessageState
    {
        SCHEDULED,
        PENDING_APPROVAL,
        APPROVED,
        SUBMITTED,
        SENT,
        SEND_FAILED_PERMANENTLY,
        DELETED,
        REJECTED
    }
}
