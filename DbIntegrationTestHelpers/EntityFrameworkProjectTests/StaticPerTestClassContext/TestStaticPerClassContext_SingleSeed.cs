namespace EntityFrameworkProjectTests
{
    using System;
    using System.Linq;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    [TestFixture]
    public class TestStaticPerClassContext_SingleSeed : IntegrationTestsContextSharedPerClass<BookStoreDbContext>
    {
        private int counter = 0;
        protected override Action SeedAction => () =>
        {
            this.Context.Authors.Add(new Author() { Name = "Bill_TestStaticContext_SingleSeed" + this.counter });
            this.Context.SaveChanges();
        }; //name is unique, but seed will only run one time

        [Test]
        public void TestOne()
        {
            this.ValidateUser();
        }

        private void ValidateUser()
        {
            if (this.counter == 0)
            {
                Assert.IsNotNull(this.Context.Authors.FirstOrDefault(x => x.Name == "Bill_TestStaticContext_SingleSeed0"));

                Assert.IsNull(this.Context.Authors.FirstOrDefault(x => x.Name == "Bill_TestStaticContext_SingleSeed1"));
            }
            else
            {
                Assert.IsNotNull(this.Context.Authors.FirstOrDefault(x => x.Name == "Bill_TestStaticContext_SingleSeed0"));

                Assert.IsNull(this.Context.Authors.FirstOrDefault(x => x.Name == "Bill_TestStaticContext_SingleSeed1"));
            }

            this.counter++;
        }

        [Test]
        public void TestTwo()
        {
            this.ValidateUser();
        }
    }
}