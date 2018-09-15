using System.Linq;
using DbIntegrationTestHelpers;
using EntityFrameworkSample;
using NUnit.Framework;

namespace EntityFrameworkProjectTests
{
    public class TestStaticContext_TestDuplicatesBetweenClasses : IntegrationTestsContextSharedGlobally<BookStoreDbContext>
    {
        [Test]
        public void TestOne()
        {
            this.InsertOrFail();
        }

        private void InsertOrFail()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestStaticContext_TestDuplicatesBetweenClasses"))
            {
                if (StaticContextChangeTracker.Counter > 0)
                {
                    Assert.Fail("Test in both classes were executed but author not found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestStaticContext_TestDuplicatesBetweenClasses" });
                this.Context.SaveChanges();
                StaticContextChangeTracker.Counter++;
                return;
            }
            else
            {
                this.Context.Authors.Add(new Author() { Name = "TestStaticContext_TestDuplicatesBetweenClasses" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestStaticContext_TestDuplicatesBetweenClasses)."));
            }
        }
    }

    public static class StaticContextChangeTracker
    {
        public static int Counter { get; set; }
    }

    public class TestStaticContext_TestDuplicatesBetweenClasses_Class2 : IntegrationTestsContextSharedGlobally<BookStoreDbContext>
    {
        [Test]
        public void TestOne()
        {
            this.InsertOrFail();
        }

        private void InsertOrFail()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestStaticContext_TestDuplicatesBetweenClasses"))
            {
                if (StaticContextChangeTracker.Counter > 0)
                {
                    Assert.Fail("Test in both classes were executed but author not found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestStaticContext_TestDuplicatesBetweenClasses" });
                this.Context.SaveChanges();
                StaticContextChangeTracker.Counter++;
                return;
            }
            else
            {
                this.Context.Authors.Add(new Author() { Name = "TestStaticContext_TestDuplicatesBetweenClasses" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestStaticContext_TestDuplicatesBetweenClasses)."));
            }
        }
    }
}