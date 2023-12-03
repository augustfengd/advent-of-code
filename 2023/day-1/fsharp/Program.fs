let lines = System.IO.File.ReadAllLines "input"

module One =
    let find fn = Seq.toList >> fn System.Char.IsDigit >> System.Char.GetNumericValue >> int

    let calculate s = find List.find s * 10 + find List.findBack s

    let run = Seq.sumBy calculate

module Two =
    let startsWith (a : string) (b : string) = a.StartsWith(b)
    let endsWith (a: string) (b: string) = a.EndsWith(b)
    let find fn = function | s when fn s "one" -> Some 1
                           | s when fn s "two" -> Some 2
                           | s when fn s "three" -> Some 3
                           | s when fn s "four" -> Some 4
                           | s when fn s "five" -> Some 5
                           | s when fn s "six" -> Some 6
                           | s when fn s "seven" -> Some 7
                           | s when fn s "eight" -> Some 8
                           | s when fn s "nine" -> Some 9
                           | _ -> None

    let tryParseDigit c = if System.Char.IsDigit c then Some (System.Char.GetNumericValue c |> int) else None

    let head (s : string) = s[0]
    let tail (s : string) = s[1 .. String.length s]
    let inline last (s : string) = s[String.length s - 1]
    let inline untilLast (s: string) = s[0 .. String.length s - 2]

    let rec a s = find startsWith s |> Option.defaultValue (tryParseDigit (head s) |> Option.defaultWith (fun () -> a (tail s)))
    let rec b s = find endsWith s |> Option.defaultValue (tryParseDigit (last s) |> Option.defaultWith (fun () -> b (untilLast s)))

    let calculate s = a s * 10 + b s

    let run = Seq.sumBy calculate

[<EntryPoint>]
let main _ =
    One.run lines |> printfn "%d"
    Two.run lines |> printfn "%d"
    0
