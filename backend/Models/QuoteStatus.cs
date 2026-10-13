namespace ControlElectricoObrera.Api.Models;

public enum QuoteStatus
{
    New = 0,
    UnderReview = 1,
    PreparingQuote = 2,
    Quoted = 3,
    FollowUp = 4,
    Closed = 5,
    Cancelled = 6
}
