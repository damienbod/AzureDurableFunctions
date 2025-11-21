namespace DurableRetrySubOrchestrations;

public static class Constants
{
    public const string MyOrchestration = "MyOrchestration";
    public const string MySecondOrchestration = "MySecondOrchestration";
    public const string MyActivityOne = "MyActivityOne";
    public const string MyActivityTwo = "MyActivityTwo";
    public const string MyActivityThree = "MyActivityThree";
    public const string MyActivityFour= "MyActivityFour";

    public const string BeginFlowWithHttpPost = "BeginFlowWithHttpPost";

    // Diagnostics API
    public const string Diagnostics = "Diagnostics";
    public const string GetAllFlows = "GetAllFlows";
    public const string GetCompletedFlows = "GetCompletedFlows";
    public const string GetNotCompletedFlows = "GetNotCompletedFlows";
}
