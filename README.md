# Introduction ![BuildStatus](https://bartosz-jarmuz.visualstudio.com/_apis/public/build/definitions/2f586b4d-795d-4c87-b2c7-5227ff449c4a/13/badge)
A tiny helper assembly that contains some boilerplate code for creating DB integration tests in EntityFramework.  
Remember - unit tests do not allow you to test everything *that you should* test. 
Integration tests are good, especially if writing them is simple.
Don't take my word for it, have a read, e.g. here: https://blog.kentcdodds.com/write-tests-not-too-many-mostly-integration-5e8c7fff591c or here https://gist.github.com/alistairmgreen/ca3f7baddb737fd91e6bf7399ba6deeb

Why unit tests are not enough?

![Unit tests in all glory](https://cdn-images-1.medium.com/max/1600/1*3NAuvsj75Ir0UNSvJf_8pg.gif)

# Installation 
Available as a Nuget package - https://www.nuget.org/packages/DbIntegrationTestHelpers/

Just run `Install-Package DbIntegrationTestHelpers`

# Usage

## General
Regardless of which approach (instance or static) you choose as the base class for your test class, you gain access to a `this.Context` property, which will contain your DbContext.

**Your DbContext is required to have a constructor which allows passing connection string**.
```c#
 public MyDbContext(string nameOrConnectionString) : base(nameOrConnectionString){}
```


The base class allows also specifying a SeedAction, as follows:
```c#
protected override Action SeedAction => () =>
        {
            SomeStaticClass.DoSomeSeeding(this.Context);
            this.DoSomeMoreSeeding();
            //or even seed directly
            this.Context.Products.Add(new Product(){Name="TheProduct"});
            this.Context.SaveChanges();
        };
```
## Context shared in test class
Add `IntegrationTestsContextSharedPerClasss<T>` as base class for your test class. The type parameter should be your DbContext type.

You now have access to `this.Context` which is instance of your DbContext - the database behind it will be shared for all unit tests within this test class. This is an approach which balances the indenpendence of tests with quick execution.

## New context for each test
Add `IntegrationTestsContextNotShared<T>` as base class for your test class. The type parameter should be your DbContext type.

You now have access to `this.Context` which is instance of your DbContext - the database behind it will be dropped and recreated with each test method run - this means there is an overhead for each test, during which a database is created and seeded. It might be large, depending on machine and database.

## Context shared for all tests in all test classes
Add `IntegrationTestsContextSharedGlobally<T>` as base class for your test class. The type parameter should be your DbContext type.

You now have access to `this.Context` which is a **static** object of your DbContext. 
Due to that, bear in mind that the tests methods you write will have to be structured in a certain way (you cannot have each test reference a mock entity with ID 1).

The tests will use a new, test only database (LocalDb). 
**The database is wiped only when a new test run is exectuted**, so you can have a look at what entries were added etc., anytime after the tests (regardless if succeeded or failed).


# Why not in-memory?
There are some in-memory DbContext providers, such as EFFORT. Why not use them?
While I believe these are great and powerful tools (sure more mature than this project, so far), I ran into problems where an in-memory provider gave me false positives. 
At some point, I have introduced a new entity in my code-first app, and messed a FK relationship. Unit tests with Effort did not detect this change and kept on passing - and that is not what I wanted. I wanted to be sure that my entity relationship is not messed up by a simple bug - hence, this project.
