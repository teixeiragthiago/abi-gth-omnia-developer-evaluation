namespace Ambev.DeveloperEvaluation.WebApi.Common;

public static class ValidationMessages
{
    public static string Min(string field, object value)
        => string.Format("{0} must be at least {1}.", field, value);

    public static string Max(string field, object value)
        => string.Format("{0} cannot exceed {1}.", field, value);

    public static string GreaterThan(string field, object value)
        => string.Format("{0} must be greater than {1}.", field, value);

    public static string LessThanOrEqual(string field, object value)
        => string.Format("{0} cannot be greater than {1}.", field, value);
}
