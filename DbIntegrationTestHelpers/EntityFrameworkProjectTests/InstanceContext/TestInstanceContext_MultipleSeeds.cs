namespace EntityFrameworkProjectTests
{
    using System;
    using System.Linq;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    [TestFixture]
    public class TestInstanceContext_MultipleSeeds : IntegrationTestsContextNotShared<BookStoreDbContext>
    {
        #region Overrides of HelpersBase<MyTestDbContext>
        protected override Action SeedAction => () =>
        {
            this.Context.Authors.Add(new Author() {Name = "Bill"});
            this.Context.SaveChanges();
        }; //name is unique, but each test method will have a clear db and seed will run
        #endregion

        [Test]
        public void TestOne()
        {
            Assert.AreEqual("Bill", this.Context.Authors.Single().Name);
        }

        [Test]
        public void TestTwo()
        {
            Assert.AreEqual("Bill", this.Context.Authors.Single().Name);

        }
    }
}