using System.Linq;
using DbIntegrationTestHelpers;
using EntityFrameworkSample;
using NUnit.Framework;

namespace EntityFrameworkProjectTests
{
    public class TestStaticPerClassContext_TestDuplicatesBetweenClasses : IntegrationTestsContextSharedPerClass<BookStoreDbContext>
    {
        private int internalCounter = 0;

        [Test]
        public void TestOne()
        {
            this.InsertOrFailBetweenClasses();
            this.InsertOrFailInSameClass();

        }
        [Test]
        public void TestTwo()
        {
            this.InsertOrFailInSameClass();
        }
        private void InsertOrFailBetweenClasses()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestIfUserIsSharedBetweenClasses"))
            {
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedBetweenClasses" });
                this.Context.SaveChanges();
                PerClassChangeTracker.HitCount++;
                return;
            }
            else
            {
                if (PerClassChangeTracker.HitCount > 0)
                {
                    Assert.Fail("Test in both classes were executed and author was found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedBetweenClasses" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestIfUserIsSharedBetweenClasses)."));
            }
        
    }

        private void InsertOrFailInSameClass()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestIfUserIsSharedInClass"))
            {
                if (this.internalCounter > 0)
                {
                    Assert.Fail("Test in both classes were executed but author 'TestIfUserIsSharedInClass' was NOT found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedInClass" });
                this.Context.SaveChanges();
                this.internalCounter++;
                return;
            }
            else
            {
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedInClass" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestIfUserIsSharedInClass)."));
            }

        }

    }

    public static class PerClassChangeTracker
    {
        public static int HitCount { get; set; }
    }

    public class TestStaticPerClassContext_TestDuplicatesBetweenClasses2 : IntegrationTestsContextSharedPerClass<BookStoreDbContext>
    {
        private int internalCounter = 0;

        [Test]
        public void TestOne()
        {
            this.InsertOrFailBetweenClasses();
            this.InsertOrFailInSameClass();
        }
        [Test]
        public void TestTwo()
        {
            this.InsertOrFailInSameClass();
        }
        private void InsertOrFailBetweenClasses()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestIfUserIsSharedBetweenClasses"))
            {
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedBetweenClasses" });
                this.Context.SaveChanges();
                    PerClassChangeTracker.HitCount++;
                return;
            }
            else
            {
                if (PerClassChangeTracker.HitCount > 0)
                {
                    Assert.Fail("Test in both classes were executed and author was found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedBetweenClasses" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestIfUserIsSharedBetweenClasses)."));
            }
        }
        private void InsertOrFailInSameClass()
        {
            //not possible to determine which test runs first, so in both cases same code applies 
            //if there are no authors, add one, if there are any, add same one again and expect failure
            if (!this.Context.Authors.Any(x => x.Name == "TestIfUserIsSharedInClass"))
            {
                if (this.internalCounter > 0)
                {
                    Assert.Fail("Test in both classes were executed but author 'TestIfUserIsSharedInClass' was NOT found");
                }
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedInClass" });
                this.Context.SaveChanges();
                this.internalCounter++;
                return;
            }
            else
            {
                this.Context.Authors.Add(new Author() { Name = "TestIfUserIsSharedInClass" });
                Assert.That(() => { this.Context.SaveChanges(); },
                    Throws.Exception.InnerException.InnerException.Message.Contain(
                        "Cannot insert duplicate key row in object 'dbo.Authors' with unique index 'IX_Name'. The duplicate key value is (TestIfUserIsSharedInClass)."));
            }

        }
    }
}