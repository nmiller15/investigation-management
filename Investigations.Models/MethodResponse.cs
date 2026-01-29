namespace Investigations.App.Models;

public class MethodResponse<T>
{
    public bool WasSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Payload { get; set; }

    public static MethodResponse<T> Success(T payload, string message = "")
    {
        if (payload == null)
            return MethodResponse<T>.Failure("Payload was null.");

        return new MethodResponse<T>
        {
            WasSuccessful = true,
            Message = message,
            Payload = payload
        };
    }

    public static MethodResponse<T> Failure(string message = "Something went wrong.")
    {
        return new MethodResponse<T>
        {
            WasSuccessful = false,
            Message = message,
            Payload = default(T)
        };
    }
}
