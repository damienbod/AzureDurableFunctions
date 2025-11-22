using DurableWait.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DurableWait.Activities;

public class MyActivities
{
    private readonly MyConfiguration _myConfiguration;
    private readonly MyConfigurationSecrets _myConfigurationSecrets;
    private readonly ILogger<MyActivities> _logger;

    public MyActivities(ILogger<MyActivities> logger,
        IOptions<MyConfiguration> myConfiguration,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _myConfiguration = myConfiguration.Value;
        _myConfigurationSecrets = myConfigurationSecrets.Value;
        _logger = logger;
    }

    [Function(Constants.MyActivityOne)]
    public string MyActivityOne([ActivityTrigger] BeginRequestData beginRequestData)
    {
        _logger.LogInformation("Activity {myActivityOne} {beginRequestDataId} {myConfigurationName} {myConfigurationSecretsMySecretOne} amount of retries: {myConfigurationAmountOfRetries}.",
            Constants.MyActivityOne, beginRequestData.Id, _myConfiguration.Name, _myConfigurationSecrets.MySecretOne, _myConfiguration.AmountOfRetries);
        
        return $"{Constants.MyActivityOne} {beginRequestData.Id} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
    }

    [Function(Constants.MyActivityTwo)]
    public string MyActivityTwo([ActivityTrigger] MyOrchestrationDto myOrchestrationDto)
    {
        // simi HTTP request which lasts 14s and causes timeout
        // Thread.Sleep(14000);

        _logger.LogInformation("Activity {myActivityTwo}  {myOrchestrationDtoBeginRequestMessage} {myConfigurationName}.",
            Constants.MyActivityTwo, myOrchestrationDto.BeginRequest.Message, _myConfiguration.Name);

        return $"{Constants.MyActivityTwo} {myOrchestrationDto.BeginRequest.Message} {_myConfiguration.Name}!";
    }
}