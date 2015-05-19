module iOSTests

open NUnit.Framework
open FsUnit
open Xamarin.UITest

[<TestFixture>]
type ``iOS Tests``() =
    let iOSAppFile = ""

    let app = ConfigureApp.iOS.AppBundle(iOSAppFile).StartApp()

    [<Test>]
    member this.Test() =
        true |> should equal true