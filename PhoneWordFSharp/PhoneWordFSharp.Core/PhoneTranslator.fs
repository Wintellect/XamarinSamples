namespace PhoneWordFSharp.Core

open System
open System.Text

module PhoneTranslator =
    
    let digits = ["ABC"; "DEF"; "GHI"; "JKL"; "MNO"; "PQRS"; "TUV"; "WXYZ"]

    let translateToNumber(c:Char) =
        [ for i = 0 to digits.Length - 1 do
            if digits.[i].Contains(string c) then
                yield i + 2 ] 

        |> List.head

    let toNumber(raw:string) =
        let sb = StringBuilder()
        let appendNumber(c:Char) =
          if " -0123456789".Contains(c.ToString()) then sb.Append(c) |> ignore
          else translateToNumber c |> sb.Append |> ignore

        if String.IsNullOrWhiteSpace(raw) then String.Empty
        else 
          raw.ToCharArray() |> Seq.iter appendNumber
          sb.ToString()