namespace Arsenals.Domains.Exceptions;

/// <summary>
/// 重複例外
/// </summary>
public class DuplicateException : Exception
{
    public DuplicateException() : base() { }
    public DuplicateException(string message) : base(message) { }
    public DuplicateException(string message, Exception inner) : base(message, inner) { }
}
