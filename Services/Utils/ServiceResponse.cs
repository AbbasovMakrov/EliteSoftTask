namespace EliteSoftTask.Services.Utils;

public struct ServiceResponse<TSuccess,TFail>
{
    public TSuccess? Success { get; private init; }
    public TFail? Fail { get; private init; }
    public bool IsSuccess { get; init; } = false;

    private ServiceResponse(TSuccess successValue,TFail? failValue)
    {
        Success = successValue;
        Fail = failValue;
    }
    public static ServiceResponse<TSuccess, TFail> ActionSuccess(TSuccess value)
    {
        return new ServiceResponse<TSuccess, TFail>(value,default){IsSuccess = true};
    }
    public static ServiceResponse<TSuccess, TFail> ActionFail(TFail value)
    {
        return new ServiceResponse<TSuccess, TFail>(default,value);
    }
    
    
}
public readonly struct ServiceResponse<TSuccess>(TSuccess value)
{
    public TSuccess? Success { get; } = value;
    public bool IsSuccess => true;
}
