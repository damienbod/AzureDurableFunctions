# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade DurableRetrySubOrchestrations\DurableRetrySubOrchestrations.csproj
4. Upgrade DurableWait\DurableWait.csproj
5. Upgrade DurableExternal\DurableExternal.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                         | Current Version | New Version | Description                                                    |
|:-----------------------------------------------------|:---------------:|:-----------:|:---------------------------------------------------------------|
| Azure.Extensions.AspNetCore.Configuration.Secrets    | 1.0.2           | 1.3.2       | Recommended for .NET 10.0                                      |
| Azure.Identity                                       | 1.3.0           | 1.17.1      | Security vulnerability                                         |
| Azure.Security.KeyVault.Certificates                 | 4.1.0           | 4.8.0       | Recommended for .NET 10.0                                      |
| Microsoft.Azure.Functions.Extensions                 | 1.1.0           |             | Remove (functionality included with framework)                 |
| Microsoft.Azure.Functions.Worker                     |                 | 2.51.0      | Replacement for Microsoft.NET.Sdk.Functions                    |
| Microsoft.Azure.Functions.Worker.Extensions.Http     |                 | 3.3.0       | Replacement for Microsoft.NET.Sdk.Functions                    |
| Microsoft.Azure.Functions.Worker.Sdk                 |                 | 2.0.7       | Replacement for Microsoft.NET.Sdk.Functions                    |
| Microsoft.Azure.Services.AppAuthentication           | 1.6.1           |             | Remove (deprecated, use Azure.Identity instead)                |
| Microsoft.Azure.WebJobs.Extensions.DurableTask       | 2.4.3           |             | Remove (functionality included with framework)                 |
| Microsoft.Extensions.Configuration                   | 3.1.14          | 10.0.0      | Recommended for .NET 10.0                                      |
| Microsoft.Extensions.Configuration.UserSecrets       | 3.1.14          | 10.0.0      | Recommended for .NET 10.0                                      |
| Microsoft.Extensions.Logging.Console                 | 3.1.14          | 10.0.0      | Recommended for .NET 10.0                                      |
| Microsoft.NET.Sdk.Functions                          | 3.0.11          |             | Replace with Microsoft.Azure.Functions.Worker packages         |
| System.Configuration.ConfigurationManager            | 5.0.0           | 10.0.0      | Deprecated - upgrade to 10.0.0                                 |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### DurableRetrySubOrchestrations\DurableRetrySubOrchestrations.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

NuGet packages changes:
  - Azure.Extensions.AspNetCore.Configuration.Secrets should be updated from `1.0.2` to `1.3.2` (*recommended for .NET 10.0*)
  - Azure.Identity should be updated from `1.3.0` to `1.17.1` (*security vulnerability*)
  - Azure.Security.KeyVault.Certificates should be updated from `4.1.0` to `4.8.0` (*recommended for .NET 10.0*)
  - Microsoft.Azure.Functions.Extensions should be removed (*functionality included with framework*)
  - Microsoft.Azure.Functions.Worker version `2.51.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Extensions.Http version `3.3.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Sdk version `2.0.7` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Services.AppAuthentication should be removed (*deprecated, use Azure.Identity instead*)
  - Microsoft.Azure.WebJobs.Extensions.DurableTask should be removed (*functionality included with framework*)
  - Microsoft.Extensions.Configuration should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Configuration.UserSecrets should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Logging.Console should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.NET.Sdk.Functions should be removed and replaced with Worker packages (*deprecated*)
  - System.Configuration.ConfigurationManager should be updated from `5.0.0` to `10.0.0` (*deprecated package*)

#### DurableWait\DurableWait.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

NuGet packages changes:
  - Azure.Extensions.AspNetCore.Configuration.Secrets should be updated from `1.0.2` to `1.3.2` (*recommended for .NET 10.0*)
  - Azure.Identity should be updated from `1.3.0` to `1.17.1` (*security vulnerability*)
  - Azure.Security.KeyVault.Certificates should be updated from `4.1.0` to `4.8.0` (*recommended for .NET 10.0*)
  - Microsoft.Azure.Functions.Extensions should be removed (*functionality included with framework*)
  - Microsoft.Azure.Functions.Worker version `2.51.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Extensions.Http version `3.3.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Sdk version `2.0.7` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Services.AppAuthentication should be removed (*deprecated, use Azure.Identity instead*)
  - Microsoft.Azure.WebJobs.Extensions.DurableTask should be removed (*functionality included with framework*)
  - Microsoft.Extensions.Configuration should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Configuration.UserSecrets should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Logging.Console should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.NET.Sdk.Functions should be removed and replaced with Worker packages (*deprecated*)
  - System.Configuration.ConfigurationManager should be updated from `5.0.0` to `10.0.0` (*deprecated package*)

#### DurableExternal\DurableExternal.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

NuGet packages changes:
  - Azure.Extensions.AspNetCore.Configuration.Secrets should be updated from `1.0.2` to `1.3.2` (*recommended for .NET 10.0*)
  - Azure.Identity should be updated from `1.3.0` to `1.17.1` (*security vulnerability*)
  - Azure.Security.KeyVault.Certificates should be updated from `4.1.0` to `4.8.0` (*recommended for .NET 10.0*)
  - Microsoft.Azure.Functions.Extensions should be removed (*functionality included with framework*)
  - Microsoft.Azure.Functions.Worker version `2.51.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Extensions.Http version `3.3.0` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Functions.Worker.Sdk version `2.0.7` should be added (*replacement for Microsoft.NET.Sdk.Functions*)
  - Microsoft.Azure.Services.AppAuthentication should be removed (*deprecated, use Azure.Identity instead*)
  - Microsoft.Azure.WebJobs.Extensions.DurableTask should be removed (*functionality included with framework*)
  - Microsoft.Extensions.Configuration should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Configuration.UserSecrets should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Logging.Console should be updated from `3.1.14` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.NET.Sdk.Functions should be removed and replaced with Worker packages (*deprecated*)
  - System.Configuration.ConfigurationManager should be updated from `5.0.0` to `10.0.0` (*deprecated package*)
