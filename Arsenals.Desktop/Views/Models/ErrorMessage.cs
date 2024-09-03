using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Arsenals.Desktop.Views.Models;

public class ErrorMessage : ValueChangedMessage<Exception>
{
    public ErrorMessage(Exception exception) : base(exception)
    {
        Title = "エラー";
        Cancel = "OK";
    }

    public string Title { get; }

    public string Cancel { get; }
}
