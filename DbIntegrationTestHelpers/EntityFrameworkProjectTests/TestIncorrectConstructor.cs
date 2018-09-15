
namespace EntityFrameworkProjectTests
{
    using System;
    using DbIntegrationTestHelpers;
    using EntityFrameworkSample;
    using NUnit.Framework;
    using NUnit.Framework.Internal;

    [TestFixture]
    public class TestIncorrectConstructor : IntegrationTestsContextNotShared<DbContextWithoutProperConstructor>
    {
        [SetUp]
        public override void SetUp()
        {
            try
            {
                base.SetUp();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Your DB context has to have a constructor which allows setting a connection string");
            }
        }

        [TearDown]
        public override void TearDown()
        {
            try
            {
                base.TearDown();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Your DB context has to have a constructor which allows setting a connection string");
            }
        }


        [Test]
        public void TestConstructorError()
        {
            
            Assert.That(() => this.Context != null, Throws.InvalidOperationException.With.Message.EqualTo("Your DB context has to have a constructor which allows setting a connection string"));
        }
    }
}
