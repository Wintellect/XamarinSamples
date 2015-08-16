using NUnit.Framework;
using Xamarin.UITest;
using TechTalk.SpecFlow;

namespace UITestDemo.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseFeature
    {
        protected Platform _platform;

        protected BaseFeature (Platform platform)
        {
            _platform = platform;
        }

        [SetUp]
        public void BeforeEachTest ()
        {
            if (!FeatureContext.Current.ContainsKey ("App")) {
                var app = AppInitializer.StartApp (_platform);
                FeatureContext.Current.Add ("App", app);
            }
        }
    }
}
