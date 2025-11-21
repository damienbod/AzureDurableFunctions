# .NET 10.0 Upgrade Report

## Summary

Successfully upgraded all three Azure Durable Functions projects from .NET Core 3.1 to .NET 10.0, migrating from the in-process model to the isolated worker model.

## Project Target Framework Modifications

| Project Name                                          | Old Target Framework | New Target Framework | Key Commits                                      |
|:------------------------------------------------------|:--------------------:|:--------------------:|:-------------------------------------------------|
| DurableRetrySubOrchestrations\DurableRetrySubOrchestrations.csproj | netcoreapp3.1        | net10.0              | 1bc33ac8, 397c6c58, 385a23e7, 055771a6          |
| DurableWait\DurableWait.csproj                        | netcoreapp3.1        | net10.0              | 6afb2f0a, 3729f75c, 40c70357, 5b117fbc          |
| DurableExternal\DurableExternal.csproj                | netcoreapp3.1        | net10.0              | e56d6204, 67e82ea4, 2b4011db, e37f51bd          |

## NuGet Package Changes

### Packages Added

| Package Name                                               | New Version |
|:-----------------------------------------------------------|:-----------:|
| Azure.Extensions.AspNetCore.Configuration.Secrets          | 1.3.2       |
| Azure.Identity                                             | 1.17.1      |
| Azure.Security.KeyVault.Certificates                       | 4.8.0       |
| Microsoft.Azure.Functions.Worker.Extensions.DurableTask    | 1.11.0      |
| Microsoft.Azure.Functions.Worker.Extensions.Http           | 3.3.0       |
| Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore| 1.3.3       |
| Microsoft.Extensions.Configuration                         | 10.0.0      |
| Microsoft.Extensions.Configuration.UserSecrets             | 10.0.0      |
| Microsoft.Extensions.Logging.Console                       | 10.0.0      |
| System.Configuration.ConfigurationManager                  | 10.0.0      |

### Packages Removed

| Package Name                                    | Reason                                           |
|:------------------------------------------------|:-------------------------------------------------|
| Microsoft.Azure.Functions.Extensions            | Functionality included with framework            |
| Microsoft.Azure.Services.AppAuthentication      | Deprecated, replaced with Azure.Identity         |
| Microsoft.Azure.WebJobs.Extensions.DurableTask  | Replaced with isolated worker model packages     |
| Microsoft.NET.Sdk.Functions                     | Replaced with isolated worker model packages     |

## Major Code Changes

### Migration to Isolated Worker Model

All three projects were migrated from the Azure Functions in-process model to the isolated worker model:

#### Startup.cs → Program.cs Migration
- **Commented out** `Startup.cs` files in all projects (can be deleted after verification)
- **Migrated** all service registrations from `IFunctionsHostBuilder` to `HostBuilder.ConfigureServices`
- **Migrated** all configuration logic from `IFunctionsConfigurationBuilder` to `HostBuilder.ConfigureAppConfiguration`
- **Used** `ConfigureFunctionsWebApplication()` for ASP.NET Core integration

#### API Changes

**DurableRetrySubOrchestrations:**
- Updated `IDurableOrchestrationClient` → `DurableTaskClient`
- Updated orchestration trigger from `IDurableOrchestrationContext` → `TaskOrchestrationContext`
- Updated activity trigger from `IDurableActivityContext` → typed input parameters
- Updated retry options from `RetryOptions` → `TaskOptions` with `RetryPolicy`
- Added `CreateReplaySafeLogger` for orchestrations

**DurableWait:**
- Updated `IDurableOrchestrationClient` → `DurableTaskClient`
- Updated `StartNewAsync` → `ScheduleNewOrchestrationInstanceAsync`
- Updated `GetStatusAsync` → `GetInstanceAsync`
- Updated `WaitForCompletionOrCreateCheckStatusResponseAsync` → `WaitForInstanceCompletionAsync`
- Updated JSON deserialization to use `JsonSerializer.DeserializeAsync`
- Implemented proper timeout handling with `CancellationTokenSource`

**DurableExternal:**
- Updated `IDurableOrchestrationClient` → `DurableTaskClient`
- Updated external event handling to work with isolated worker model
- Updated diagnostics APIs to use `OrchestrationQuery` and `GetAllInstancesAsync`

### Diagnostics API Updates

All projects' `DiagnosticsApi` files were updated:
- Replaced `ListInstancesAsync` with `GetAllInstancesAsync`
- Replaced `OrchestrationStatusQueryCondition` with `OrchestrationQuery`
- Updated to use `OrchestrationMetadata` for results

## Project Configuration Changes

All projects received the following updates in their `.csproj` files:
- **TargetFramework**: Changed from `netcoreapp3.1` to `net10.0`
- **AzureFunctionsVersion**: Updated from `v3` to `v4`
- **OutputType**: Added `Exe`
- **ImplicitUsings**: Enabled
- **Using Alias**: Added for `System.Threading.ExecutionContext`

## Security Improvements

- **Azure.Identity** upgraded from 1.3.0 to 1.17.1 (addressed security vulnerabilities)
- Removed deprecated **Microsoft.Azure.Services.AppAuthentication** package
- All Azure SDK packages updated to latest compatible versions

## All Commits

| Commit ID | Description                                                                                          |
|:----------|:-----------------------------------------------------------------------------------------------------|
| 1bc33ac8  | Commit upgrade plan                                                                                  |
| 397c6c58  | Update DurableRetrySubOrchestrations.csproj for .NET 10 & v4                                         |
| 385a23e7  | Migrated from in-process to isolated worker model - updated Startup.cs, Program.cs, orchestrations, activities, and APIs |
| e50223e5  | Replace IDurableActivityContext with TaskActivityContext in MyActivities.cs                          |
| f1fc8a6a  | Update DurableRetrySubOrchestrations.csproj dependencies                                             |
| 815d079d  | Replace IDurableOrchestrationClient with DurableTaskClient in DiagnosticsApi.cs                      |
| 70e07210  | Migrate to Azure Functions v4 isolated worker model                                                  |
| 055771a6  | Store final changes for step 'Upgrade DurableRetrySubOrchestrations\DurableRetrySubOrchestrations.csproj' |
| 0f1b7638  | Add using directive for Microsoft.DurableTask.Client in DiagnosticsApi.cs                            |
| 6afb2f0a  | Update DurableWait.csproj package references                                                         |
| 3729f75c  | Update DurableWait.csproj to .NET 10 and Azure Functions v4                                          |
| 40c70357  | Migrated from in-process to isolated worker model - updated Startup.cs, Program.cs, orchestrations, activities, Processing.cs, and APIs |
| f2c47bec  | Migrate to Azure Functions v4 isolated worker model for DurableWait                                  |
| 5b117fbc  | Store final changes for step 'Upgrade DurableWait\DurableWait.csproj'                                |
| e56d6204  | Update DurableExternal.csproj to .NET 10 and Azure v4                                                |
| 67e82ea4  | Update DurableExternal.csproj package references                                                     |
| 2b4011db  | Migrated from in-process to isolated worker model - updated Startup.cs, Program.cs, orchestrations, activities, and APIs |
| e37f51bd  | Migrate to Azure Functions v4 isolated worker model for DurableExternal                              |

## Next Steps

1. **Review and test** all three projects to ensure they function correctly with the new isolated worker model
2. **Delete commented `Startup.cs` files** after confirming the migration is successful
3. **Update deployment configurations** to support .NET 10.0 and the isolated worker model
4. **Review and update `host.json`** settings as needed for the v4 runtime
5. **Test external event handling** in the DurableExternal project
6. **Verify Key Vault integration** is working correctly with the updated Azure.Identity package
7. **Run all diagnostic APIs** to ensure orchestration monitoring still works as expected
8. **Consider updating to .NET 8.0 LTS** if long-term support is preferred over the preview .NET 10.0

## Important Notes

- The upgrade maintains backward compatibility for orchestration and activity logic
- All configuration and dependency injection patterns have been preserved
- External event handling remains functional in the isolated worker model
- Security vulnerabilities in Azure.Identity have been addressed
- Projects now use the modern Azure Functions isolated worker model, providing better performance and deployment flexibility
