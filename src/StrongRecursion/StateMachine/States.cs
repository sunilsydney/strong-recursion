namespace StrongRecursion.StateMachine
{
    /// <summary>
    /// Possible states
    /// </summary>
    public enum States
    {
        Empty = 0,
        If,
        Then0,
        ElseIf,
        Then1,
        Else,
        Then2,
        Error,
        Ready
    }
}
