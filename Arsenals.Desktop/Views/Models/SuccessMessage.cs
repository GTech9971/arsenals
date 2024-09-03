using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Arsenals.Desktop.Views.Models;

public class SuccessMessage : ValueChangedMessage<string>
{
    public SuccessMessage(string message, string title = "成功", string cancel = "OK") : base(message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(cancel, nameof(cancel));

        Title = title;
        Cancel = cancel;
    }

    public string Title { get; }

    public string Cancel { get; }
}
