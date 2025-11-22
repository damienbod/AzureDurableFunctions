using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DurableRetrySubOrchestrations.Activities;

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
    public string MyActivityOne([ActivityTrigger] string input)
    {
        _logger.LogInformation("Activity {myActivityOne} {input} {myConfigurationName} {myConfigurationSecretsMySecretOne} amount of retries: {myConfigurationAmountOfRetries}.",
            Constants.MyActivityOne, input, _myConfiguration.Name, _myConfigurationSecrets.MySecretOne, _myConfiguration.AmountOfRetries);

        //throw new System.Exception("something went wrong");
        return $"{Constants.MyActivityOne} {input} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
    }

    [Function(Constants.MyActivityTwo)]
    public string MyActivityTwo([ActivityTrigger] string input)
    {
        _logger.LogInformation("Activity {myActivityTwo}  {input} {myConfigurationName}.", Constants.MyActivityTwo, input, _myConfiguration.Name);

        return $"{Constants.MyActivityTwo} {input} {_myConfiguration.Name}!";
    }

    [Function(Constants.MyActivityThree)]
    public string MyActivityThree([ActivityTrigger] string input)
    {
        _logger.LogInformation("Activity {myActivityThree} {input} {myConfigurationName}.",
            Constants.MyActivityThree, input, _myConfiguration.Name);

        return $"{Constants.MyActivityThree} {input} {_myConfiguration.Name}!";
    }

    [Function(Constants.MyActivityFour)]
    public string MyActivityFour([ActivityTrigger] string input)
    {
        _logger.LogInformation("Activity {myActivityFour}  {input} {myConfigurationName}.",
            Constants.MyActivityFour, input, _myConfiguration.Name);

        //throw new System.Exception("something went wrong");
        return $"{Constants.MyActivityFour} {input} {_myConfiguration.Name}!";
    }
}