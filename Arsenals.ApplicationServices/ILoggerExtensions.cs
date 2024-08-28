using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Arsenals.ApplicationServices;

public static class ILoggerExtensions
{
    public static void LogMethodStart<T>(this ILogger logger,
                                        Expression<Func<T>> argumentExpression,
                                         [CallerMemberName] string methodName = "",
                                         [CallerFilePath] string filePath = "",
                                         [CallerLineNumber] int lineNumber = 0)
    {
        if (logger == null || argumentExpression == null) return;

        var argumentName = GetArgumentName(argumentExpression);
        var argumentValue = GetArgumentValue(argumentExpression);

        var message = $"{methodName}({argumentName} = {argumentValue}) - Start";
        logger.LogInformation(message);
    }

    private static string GetArgumentName<T>(Expression<Func<T>> argumentExpression)
    {
        if (argumentExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        return string.Empty;
    }

    private static T GetArgumentValue<T>(Expression<Func<T>> argumentExpression)
    {
        var compiledExpression = argumentExpression.Compile();
        return compiledExpression();
    }
}
