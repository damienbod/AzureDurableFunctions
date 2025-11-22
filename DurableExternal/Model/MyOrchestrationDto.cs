namespace MyAzureFunctions.Model;

public class MyOrchestrationDto
{
    public string InputStartData { get; set; } = string.Empty;
    public string MyActivityOneResult { get; set; } = string.Empty;
    public string ExternalInputData { get; set; } = string.Empty;
    public string MyActivityTwoResult { get; set; } = string.Empty;
}
