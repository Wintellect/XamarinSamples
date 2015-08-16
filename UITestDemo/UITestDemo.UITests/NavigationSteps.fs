namespace UITestDemo.UITests

open NUnit.Framework
open TechTalk.SpecFlow
open UITestDemo
open Xamarin.UITest

[<Binding>]
type NavigationSteps() = 
    let app = FeatureContext.Current.Get<IApp>("App")

    let [<Given>] ``I am in the app``() =
        app.Query(fun n -> n.Marked(""))

    let [<When>] ``I press the detail button``() =
        app.Tap(fun b -> b.Text("Go to detail page"))

    let [<Then>] ``I should be on the detail screen``() =
        app.Query(fun n -> n.Marked(""))