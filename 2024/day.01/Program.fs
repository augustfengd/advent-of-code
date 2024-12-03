module String =
    let split (c : char) (s :string) = s.Split(c)

module Lib =
    // part 1
    let getInput () = System.IO.File.ReadAllText "input.txt"
    let toLines = String.split '\n'
    let toTuple =
        String.split ' '
        >> Array.filter (System.String.IsNullOrEmpty >> not)
        >> fun pair -> (pair.[0] , pair.[1])
        

    let tuple2ToList (left,right) = [ left ; right ]
    let parse = toLines
             >> Array.map toTuple
             >> Array.unzip
             >> tuple2ToList
             >> List.map (Array.map System.Int32.Parse >> Array.toList)
             >> List.map (List.sort)
             >> fun pair -> pair.[0], pair.[1]

    // part 2
    let compareAndAdd sum left right=
        if left > right then
            sum + (left - right)
        else
            sum + (right - left)

    let incrementOccurrences acc k = match Map.tryFind k acc with
                                                            | Some v -> Map.add k (v + 1) acc
                                                            | None -> Map.add k 1 acc
    let buildOccurrences locations = List.fold incrementOccurrences Map.empty locations
    
    let calculateSimilarityScore map k =
        match Map.tryFind k map with
        | Some v -> k * v
        | None -> 0

module App =
    open Lib
    
    let part1 () =
        let s = getInput ()
        let left, right = parse s
        List.fold2 compareAndAdd 0 left right

    let part2 () =
        let s = getInput ()
        let left, right = parse s
        let getSimilarityScore = calculateSimilarityScore <| buildOccurrences right
        List.sumBy getSimilarityScore left

[<EntryPoint>]
let main _ =
    App.part1 () |> printfn "%d"
    App.part2 () |> printfn "%d"
    0