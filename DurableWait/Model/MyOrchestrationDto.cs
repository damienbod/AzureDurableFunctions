namespace DurableWait.Model;

public class MyOrchestrationDto
{
    public BeginRequestData BeginRequest { get; set; }
    public string MyActivityOneResult { get; set; } 
    public string MyActivityTwoResult { get; set; }
}
