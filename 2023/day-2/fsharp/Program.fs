open FSharpPlus

let lines = System.IO.File.ReadAllLines "input"

type Set =
    { Red : int
      Green: int
      Blue: int }

type Game =
    { Id : int
      Sets : Set list }

module Parse =
    let buildSets s : Set list =
         let raw = String.split [";"] s
         let parse (s : string) =
             let parts = String.split [ "," ] s
             let contains (a : string) (b : string) = b.Contains(a)
             let tryFind s = Seq.tryFind (contains s) >> Option.bind (trySscanf (PrintfFormat<string -> int, _, _, int, _>("%d " + s))) >> Option.defaultValue 0
             { Red = tryFind "red" parts
               Green = tryFind "green" parts
               Blue = tryFind "blue" parts }
         Seq.map parse raw |> Seq.toList

    let buildGame line =
        let i, rest = sscanf "Game %d:%s" line
        { Id = i
          Sets = buildSets rest }

module One =
    let resources = { Red = 12; Green = 13; Blue = 14 }
    let exceeds set = set.Red > resources.Red  || set.Green > resources.Green || set.Blue > resources.Blue
    let impossible = List.exists exceeds

    let run lines =
        let games = List.map Parse.buildGame (Seq.toList lines)
        List.sumBy (fun game -> if impossible game.Sets then 0 else game.Id) games

module Two =
    let findMax sets =
        { Red = List.map (_.Red) sets |> List.max
          Green = List.map (_.Green) sets |> List.max
          Blue = List.map (_.Blue) sets |> List.max }

    let power max =
        max.Red * max.Green * max.Blue

    let run lines =
        let games = List.map Parse.buildGame (Seq.toList lines)
        List.sumBy (fun game -> findMax game.Sets |> power) games

[<EntryPoint>]
let main _ =
    One.run lines |> printfn "%d"
    Two.run lines |> printfn "%d"
    0
