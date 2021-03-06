QWIQ
=======

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/MicrosoftEdge/Microsoft.Qwiq/blob/master/LICENSE) [![AppVeyor](https://img.shields.io/appveyor/ci/MicrosoftEdge/Microsoft-Qwiq.svg)]() [![qwiq MyGet Build Status](https://www.myget.org/BuildSource/Badge/qwiq?identifier=6c2b6200-0621-4b3f-ba6b-b28d2b785765)](https://www.myget.org/)

[![MyGet](https://img.shields.io/myget/qwiq/v/Microsoft.Qwiq.Core.svg)]() [![MyGet](https://img.shields.io/myget/qwiq/vpre/Microsoft.Qwiq.Core.svg)]()

QWIQ is a **Q**uick **W**ork **I**tem **Q**uery library for Team Foundation Server / Visual Studio Online. If you do a lot of reading or writing of work items, this package is for you! 

## What can it be used for?
Querying Team Foundation Server, of course! Instead of directly using the TFS Client OM, you could use QWIQ! It it made of packages designed to make working with Tfs/Vso a pleasure. Qwiq.Core is the no-frills base package, exposinng the raw types needed to read and write work items. Qwiq.Identity adds methods to simplify converting between your preferred method of identity (display names, user names) and TFS's identity classes. Qwiq.Linq provides a Linq query provider to be able to write Linq to query tfs. Qwiq.Mapper enables converting from IWorkItem, the raw Qwiq.Core type, to your own classes to enable strongly typed access to your WorkItems. Qwiq.Relatives extends Qwiq.Linq to enable slightly more complicated queries allowing for basic queries of related workitems. Qwiq.Mocks provides default implementations for commonly mocked classes within Qwiq, and should allow for getting up and unit testing quickly. Why use this over the Client OM? Glad you asked!

### 1. Easier to consume
Let's be honest, the TFS libraries are a pain to use. There are a lot of them, several are dynamically loaded, and a few are native. While we can't avoid it, you can! Just install the Qwiq.Core package and everything will be in your \bin folder when you need it.

### 2. Easier to test
Qwiq makes testing your apps a breeze. Everything has an interface. Everything uses factories (or factory methods) instead of constructors. Just mock what you need for your tests and go. No more messy, temperamental fakes, or adapters cluttering your code.

### 3. Easier to understand
How often do you update a work item? How often do you create a new security group? We stripped out the rarely used stuff to make interfaces cleaner and the relationships between types simpler. Missing something you can't live without? Send us a pull request!

## How to install it
[Add our MyGet feed to your NuGet clients](https://docs.nuget.org/ndocs/tools/package-manager-ui#package-sources):

 - v3 (VS 2015+ / NuGet 3.x): `https://www.myget.org/F/qwiq/api/v3/index.json`
 - v2 (VS 2013 / NuGet 2.x): `https://www.myget.org/F/qwiq/api/v2`

Once the feed is configured, install via the nuget UI or via the nuget package manager console

### Install Core
From the NuGet package manager console
```
PM> Install-Package Microsoft.Qwiq.Core
```
Or via the UI [Microsoft.Qwiq.Core](https://www.myget.org/feed/qwiq/package/nuget/Microsoft.Qwiq.Core),


### Install Client
We now have two clients: one for SOAP, and one for REST

From the NuGet package manager console
```
PM> Install-Package Microsoft.Qwiq.Client.Soap
```
Or via the UI [Microsoft.Qwiq.Client.Soap](https://www.myget.org/feed/qwiq/package/nuget/Microsoft.Qwiq.Client.Soap),

### Basic Usage
```csharp
using Microsoft.Qwiq;
using Microsoft.Qwiq.Credentials;

using Microsoft.VisualStudio.Services.Client;
...

// We support
//  - OAuth2
//  - Personal Access Token (PAT)
//  - Username and password (BASIC)
//  - Windows credentials (NTLM or Federated with Azure Active Directory)
//  - Anonymous

// Use the full URI, including the collection. Example: https://QWIQ.VisualStudio.com/DefaultCollection
var uri = new Uri("[Tfs Tenant Uri]");
var options = new AuthenticationOptions(uri, AuthenticationType.Windows);
var store = WorkItemStoreFactory
                .Default
                .Create(options);

// Execute WIQL
var items = store.Query(@"
    SELECT [System.Id] 
    FROM WorkItems 
    WHERE [System.WorkItemType] = 'Bug' AND State = 'Active'");
```

```powershell
[Reflection.Assembly]::LoadFrom("E:\Path\To\Microsoft.Qwiq.Core.dll")
# Can use SOAP or REST clients here
[Reflection.Assembly]::LoadFrom("E:\Path\To\Microsoft.Qwiq.Client.Soap.dll")

$uri = [Uri]"[Tfs Tenant Uri]"
$options = New-Object Microsoft.Qwiq.Credentials.AuthenticationOptions $uri,Windows
$store = [Microsoft.Qwiq.Client.Soap.WorkItemStoreFactory]::Default.Create($options)

$items = $store.Query(@"
    SELECT [System.Id] 
    FROM WorkItems 
    WHERE [System.WorkItemType] = 'Bug' AND State = 'Active'", $false)
```

## Contributing
**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide/)
 * [Open an issue](https://github.com/MicrosoftEdge/Microsoft.Qwiq/issues) if you encounter a bug or have a suggestion for improvements/features


Once you're familiar with Git and GitHub, clone the repository and start contributing!

