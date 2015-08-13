namespace UITestDemo.UnitTests

open FsUnit
open NUnit.Framework
open UITestDemo

[<TestFixture>]
type AddTests() = 

    [<Test>]
    member x.``Adding 2 + 2 = 4``() =
        let result = Add.AddItems(2, 2)

        result |> should equal 4

    [<Test>]
    member x.``Adding 1 + 0 = 1``() =
        let result = Add.AddItems(1, 0)

        result |> should be (greaterThan 0)

    [<Test>]
    member x.``Adding 1 + 1 = 2``() =
        let result = Add.AddItems(1, 1)

        result |> should not' (equal 0)