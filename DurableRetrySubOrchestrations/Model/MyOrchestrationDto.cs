namespace DurableRetrySubOrchestrations.Model;

public class MyOrchestrationDto
{
    public string InputStartData { get; set; }
    public string MyActivityOneResult { get; set; }
    public string MyActivityTwoResult { get; set; }
    public MySubOrchestrationDto MySubOrchestration { get; set; }
}
