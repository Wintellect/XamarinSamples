namespace PhoneWordFSharp.Core

open System
open System.Text

type PhoneTranslator() =
    let sb = StringBuilder()
    let digits = ["ABC"; "DEF"; "GHI"; "JKL"; "MNO"; "PQRS"; "TUV"; "WXYZ"]

    let translateToNumber(c:Char) =
        [ for i = 0 to digits.Length - 1 do
            if digits.[i].ToString().Contains(c.ToString()) then
                yield i + 2 ] 

        |> List.head

    let appendNumber(c:Char) =
        match " -0123456789".Contains(c.ToString()) with
        | true -> sb.Append(c) |> ignore
        | false -> sb.Append(translateToNumber c) |> ignore

    member this.toNumber(raw:string) =
        sb.Clear() |> ignore

        match String.IsNullOrWhiteSpace(raw) with
        | true -> String.Empty
        | false -> raw.ToCharArray() |> Seq.ofArray |> Seq.iter(fun c -> appendNumber c)
                   sb.ToString()