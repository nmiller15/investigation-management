using Serilog;

namespace Investigations.Models.Shared;

public class MethodResponse<T>
{
    public bool WasSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Payload { get; set; } = default(T)!;

    public static MethodResponse<T> Success(T? payload, string message = "")
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
        Log.Warning("MethodResponse<{Type}> Failure: {Message}", typeof(T), message);
        return new MethodResponse<T>
        {
            WasSuccessful = false,
            Message = message,
            Payload = default(T)!
        };
    }

    public override string ToString()
    {
        if (WasSuccessful)
        {
            return $"Success: {Message} \n {Payload}";
        }

        return $"FAILURE: {Message}";
    }
}
