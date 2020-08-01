namespace MyAzureFunctions
{
    public static class Constants
    {
        public const string MyOrchestration = "MyOrchestration";
        public const string MyActivityOne = "MyActivityOne";
        public const string MyActivityTwo = "MyActivityTwo";

        public const string MyExternalInputEvent = "MyExternalInputEvent";

        public const string BeginFlowWithHttpPost = "BeginFlowWithHttpPost";
        public const string ExternalHttpPostInput = "ExternalHttpPostInput";

        // Diagnostics API
        public const string Diagnostics = "Diagnostics";
        public const string GetAllFlows = "GetAllFlows";
        public const string GetCompletedFlows = "GetCompletedFlows";
        public const string GetNotCompletedFlows = "GetNotCompletedFlows";
    }
}
