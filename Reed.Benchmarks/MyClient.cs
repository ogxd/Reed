namespace Reed.Benchmarks;

public partial class MyClient
{
    public MyClient()
    {
        this.MyCall(12, CancellationToken.None);
    }

    public partial Task MyCall(int bidule, CancellationToken token);
    
    [Resilient<ICustomPolicy>(nameof(MyCall))]
    public async Task<int> MyCallInternal(int bidule, CancellationToken token)
    {
        return await Task.Delay(1000);
    }
}

public interface ICustomPolicy : ICircuitBreakerPolicy, IRetryPolicy
{
    
}

public class Policy : ICustomPolicy
{
    public bool IsExceptionHandled(Exception exception)
    {
        return exception is TimeoutException;
    }

    public int CircuitBreakerFailureThreshold { get; }
    public int RetryAttempts { get; }
}