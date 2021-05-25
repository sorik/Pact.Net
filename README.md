# Example .NET Core Project for Pact

This projects are for the example of Pact to explain how Pact works. This is based on the [workshop of pact-foundation](https://github.com/pact-foundation/pact-workshop-dotnet-core-v1). The differencies from the original workshop are
-  Both Consumer and Provider are WebApi using controllers
- Using [batect](https://batect.dev/) to run the tests

This changes are to have the nearly same tech stacks with the current financing projects.

# Out of scope

- unit tests
- integration tests

# Notes
[nuget package](https://github.com/pact-foundation/pact-net#usage) is required for Pact. The package is different based on the operating system for .NET Core. In this example, ```PactNet.Linux.x64``` is specified in **.csproj** file because the docker container for this project is **linux** based. 


# Step by step

## Step 1: Understanding the projects

```
> git clone tbd
```

Have a look into Consumer and Provider project. This has a very familiar project structure. 

```
\.batect

\src
  - \Consumer

\tests
  - \Consumer.PactTests

\batect.yml
```

**Consumer** has the endpoint of ```GET /users/{userId}/memberships/fellow``` which returns an object ```{ member: true/false }```. Consumer sends a request to **Provider**'s endpoint ```/users/{userId}/memberships``` which returns an object e.g, ```{ type: fellow }```



## Step 2: Playing with Consumer and Provider

```
> git reset --hard 4b14f6e
```

Use ```./batect runApp``` and ```curl``` commands to see how **Consumer** and **Provider** works. 

**Consumer**
```
> ./batect runApp
> curl -i http://localhost:5000/users/user123/memberships/fellow
```
**Provider**
```
> ./batect runApp
> curl -i http://localhost:5001/users/user1/memberships
```

**Consumer** has ```mock Provider``` which returns static response with ```{ type: fellow }```. **Provider** has local ```dynamodb``` in which there is one item for ```user1``` who has ```fellow``` membership.


## Step 3: Creating Consumer.PactTests project

```
> git reset --hard 009fccf
```

XUnit project has been created with ```PactNet.Linux.x64```.

**ConsumerPactClassFixture** is created to have **Mock HTTP Provider Server**. **PactBuilder** is created with **PactConfig** which specifies Pact SpecificationVersion, the directory where the pact generated and the log directory. 

(Note1: SpecificationVersion should be 2.0.0 for .NET Core)
(Note2: The PactDir should be ```..\..\..\..\..\pacts``` to get the file in the ```{project dir}/pacts```)

The **ConsumerPactClassFixture** has now ```MockProviderServiceBaseUri``` which we will set it up for **Provider**'s baseUri, and ```MockProviderService```which will be used for mocking responses in the tests.

Make sure ```PactBuilder.Build()``` has been called when ```IClassFixure``` is disposed. The pact file is generated with ```PactBuilder.Build()``` is being called.

## Step 4: Adding TestServer

```
> git reset --hard bd31cba
```

**Consumer** is a WebApi which receives HTTP request. To get the Api endpoint invoked, we need TestServer. This is very similar setup that we've done for our integration tests. 

```MockProviderServiceBaseUri``` has been configured for **Provider**'s baseUri here.

## Step 5: Add ConsumerPactTests

```
> git reset --hard afe57c3
```

With the class fixture created, **ConsumerPactTests** class is created conforming ```IClassFixture``` interface with **ConsumerPactClassFixture** taking **Startup**.

Writing a test follows the steps:
1. Mock out an interaction with ```MockProviderService```
2. Send a HTTP request by using ```TestServer```'s Client to invoke **Consumer**'s endpoint
3. Assert the result is what we expect

The code is very similar with the tests that is using mock. In the Pact, the mocking interaction is defined with ```MockProviderService```.

## Step 6: Run Pact tests

Now, we are ready to run the pact tests.
```
> ./batect pactTests
```
The outcome is the **pact file**. It should be under ```Consumer\pacts``` folder.

Note: The pact file will be generated only when all the tests pass.

## Step 7: Share the pact file with Provider

```
> git reset --hard 267093e
```

**Provider** needs to verify the pact, so let's share it with **Provider**. In this example, we will copy the file into the shared directory. In the real world, we can use **Pact Broker**, **Pactflow** or even any remote shared store.

## Step 8: Creating Provider.PactTests project

```
> git reset --hard 1fddd41
```

XUnit project has been created with ```PactNet.Linux.x64```.


## Step 9: Creating a TestStartup

```
> git reset --hard e83f156
```

The Pact tests for **Provider** needs to do two things:
1. Managing the state of the **Provider** as described in the **Consumer**'s pact file
2. Invoking **Provider**'s endpoint and verify the response from **Provider** for the HTTP request in the pact file matches with the expected response. 

To do this, we need to setup the ```TestStartup```.

The ```TestStartup``` uses a middleware ```ProviderStateMiddleware``` to manage the state based on what the pact file needs for the test request.

## Step 10: Adding Repository Class

```
> git reset --hard f5d27c3
```

The **Provider** reads the data from ```dynamodb``` and the state of **Provider** is depends on the item in the ```dynamodb```. With that, we need to able to add/delete items from the ```ProviderStateMiddleware```. 
The ```MembershipLocalRepository``` has very simple functionaility of ```AddUser``` and ```DeleteAllUsers``` for tearing down the test data.


## Step 11: Managing the state

```
> git reset --hard a610786
```

From the state definition in the pact file, the action is added.


## Step 12: Adding batect tasks to run the tests

```
> git reset --hard 6de1624
```

We need to start up **Provider** and ```dynamodb``` in the local before running the pact tests, because ```TestServer``` sends a HTTP request to **Provider** and needs to manage the state in the ```dynamodb```


## Step 13: Adding ProviderApiTests

```
> git reset --hard d86ff79
```


In here, ```WebHost``` is started with ```TestStartup``` in ```http://localhost:9001```. 

The ```PactVerifier``` is created with config specifying where the output will be written, the endpoint to send a request to manipulte the state (in this example ```http://localhost:9001/pact-states```), the uri of **Provider**, the directory where the pact file is.

Make sure ```WebHost``` is stopped and disposed when the tests is done.

Let's run pact tests.

```
> ./batect verifyPact
```

## Step 14: Adding a new test in Consumer

```
> git reset --hard 1da7579
```

## Step 15: Adding a new state and action in Provider

```
> git reset --hard 259d000
```










