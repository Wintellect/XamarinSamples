using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace FakeDemo.iOS.UITests
{
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp();
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            var results = app.WaitForElement(c => c.Marked("Built using FAKE!"));

            Assert.IsTrue(results.Any());
        }
    }
}
