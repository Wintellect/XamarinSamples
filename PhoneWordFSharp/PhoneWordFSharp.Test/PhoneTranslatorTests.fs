namespace PhoneWordFSharp.Test

open NUnit.Framework
open FsUnit
open PhoneWordFSharp.Core

[<TestFixture>]
type ``Given a number to translate``() = 

    [<Test>]
    member this.``When the number is just numerical``() =
        let number = PhoneTranslator.toNumber "1 800"

        number |> should equal number

    [<Test>]
    member this.``When the number is numerica and has a dash``() =
        let number = PhoneTranslator.toNumber "1-800"

        number |> should equal number
