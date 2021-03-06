﻿namespace EntityFrameworkProjectTests
{
    using System;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    [TestFixture]
    public class TestStaticContext_AddDuplicateFromSeed : IntegrationTestsContextSharedGlobally<BookStoreDbContext>
    {
        protected override Action SeedAction => () =>
        {
            this.Context.Authors.Add(new Author() {Name = "Bill_TestStaticContext_AddDuplicateFromSeed" });
            this.Context.SaveChanges();
        }; //name is unique, but seed will only run one time

        [Test]
        public void TestOne()
        {
            this.Context.Authors.Add(new Author() { Name = "Bill_TestStaticContext_AddDuplicateFromSeed" }); //this guy has been seeded, so error expected
            Assert.That(()=>
            {
                this.Context.SaveChanges();

            }, Throws.Exception.InnerException.InnerException.Message.Contain("Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (Bill_TestStaticContext_AddDuplicateFromSeed)."));
        }

    }
}