namespace EntityFrameworkProjectTests
{
    using System.Linq;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;

    [TestFixture]
    public class TestInstanceContext_AddDuplicateFromOtherTest : IntegrationTestsContextNotShared<BookStoreDbContext>
    {
        [Test]
        public void TestOne()
        {
            this.Context.Authors.Add(new Author() { Name = "Bill" });
            this.Context.SaveChanges();
            Assert.AreEqual(1, this.Context.Authors.Single().Id);
        }

        [Test]
        public void TestTwo()
        {
            this.Context.Authors.Add(new Author() { Name = "Bill" });
            this.Context.SaveChanges();
            Assert.AreEqual(1, this.Context.Authors.Single().Id);
        }
    }
}