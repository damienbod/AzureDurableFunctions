using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyAzureFunctions.Activities;

public class MyActivities
{
    private readonly MyConfiguration _myConfiguration;
    private readonly MyConfigurationSecrets _myConfigurationSecrets;
    private readonly ILogger<MyActivities> _logger;

    public MyActivities(ILogger<MyActivities> logger, IOptions<MyConfiguration> myConfiguration,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _myConfiguration = myConfiguration.Value;
        _myConfigurationSecrets = myConfigurationSecrets.Value;
        _logger = logger;
    }

    [Function(Constants.MyActivityOne)]
    public string MyActivityOne([ActivityTrigger] string name)
    {
        _logger.LogInformation("Activity {myActivityOne} {name} {myConfigurationName} {myConfigurationSecretsMySecretOne} amount of retries: {myConfigurationAmountOfRetries}.",
            Constants.MyActivityOne, name, _myConfiguration.Name, _myConfigurationSecrets.MySecretOne, _myConfiguration.AmountOfRetries);

        return $"{Constants.MyActivityOne} {name} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
    }

    [Function(Constants.MyActivityTwo)]
    public string MyActivityTwo([ActivityTrigger] string name)
    {
        _logger.LogInformation("Activity {myActivityTwo}  {name} {_myConfiguration.Name}.",
            Constants.MyActivityTwo, name, _myConfiguration.Name);

        return $"{Constants.MyActivityTwo} {name} {_myConfiguration.Name}!";
    }

}