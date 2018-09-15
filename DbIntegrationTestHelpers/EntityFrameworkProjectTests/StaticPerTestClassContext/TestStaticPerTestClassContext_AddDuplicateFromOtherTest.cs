namespace EntityFrameworkProjectTests
{
    using System.Linq;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;

    [TestFixture]
    public class TestStaticPerTestClassContext_AddDuplicateFromOtherTest : IntegrationTestsContextSharedPerClass<BookStoreDbContext>
    {
        private int counter = 0;

        [Test]
        public void TestOne()
        {
            this.InsertOrFail();

        }

        private void InsertOrFail()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "Bill_TestStaticContext_AddDuplicateFromOtherTest"))
            {
                if (this.counter > 0)
                {
                    Assert.Fail("Both tests executed but the user is not found.");
                }
                this.Context.Authors.Add(new Author() { Name = "Bill_TestStaticContext_AddDuplicateFromOtherTest" });
                this.Context.SaveChanges();
                this.counter++;
                return;
            }
            else
            {
                this.Context.Authors.Add(new Author() { Name = "Bill_TestStaticContext_AddDuplicateFromOtherTest" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (Bill_TestStaticContext_AddDuplicateFromOtherTest)."));
            }
        }

        [Test]
        public void TestTwo()
        {
            this.InsertOrFail();
        }
    }
}