let lines = System.IO.File.ReadAllLines "input"

module Domain =
    type Positions =
        { Rows: int list
          Columns: int list }

    type Token =
        { Char: char
          Row: int
          Column: int }

    type Part =
        { Value: int
          Row: int
          Column: int list }

    type Symbol =
        { Value: char
          Row : int
          Column : int }

    type Thing = | Part of Part
                 | Symbol of Symbol

module Parser =
    open Domain

    let or' a b c = (a c) || (b c)

    let isDigit = System.Char.IsDigit

    let isSymbol = or' System.Char.IsDigit ((=) '.') >> not

    let parse n (s : string) =
        let tokens = Seq.mapi (fun m c -> { Char = c
                                            Row = n
                                            Column = m }) s |> Seq.filter (_.Char >> or' isDigit isSymbol)

        let groups = Seq.fold (fun (groups : Token list list) (token : Token) -> match groups with
                                                                                 | x :: xs
                                                                                      when (=) token.Column (List.head x |> (_.Column) |> (+) 1) &&
                                                                                           isDigit (List.head x |> (_.Char)) &&
                                                                                           isDigit token.Char ->
                                                                                               (token :: x) :: xs
                                                                                 | groups ->
                                                                                     List.singleton token :: groups ) [] tokens |> (List.map List.rev >> List.rev)

        let things = Seq.fold (fun (things : Thing list) (group : Token list) -> match group with
                                                                                 | { Char = c ; Row = n ; Column = _} :: _
                                                                                     when isDigit c ->
                                                                                         Part { Value = List.fold (fun s token -> $"{s}{token.Char}") "" group |> int
                                                                                                Row = n
                                                                                                Column = List.map (fun (token : Token) -> token.Column) group } :: things
                                                                                 | { Char = c ; Row = n ; Column = m} :: []
                                                                                     when isSymbol c ->
                                                                                         Symbol { Value = c
                                                                                                  Row = n
                                                                                                  Column = m } :: things
                                                                                 | fml -> failwith "I give up on safety; it won't reach here anyways. Hit me up when it does.") [] groups
        things |> List.rev


module Neighbors =
    open Domain

    let buildPossibleNeighborsPositions (part : Part) =
        let it = { Rows = [ part.Row - 1 ; part.Row ; part.Row + 1 ]
                   Columns = (part.Column |> List.min |> (-) <| 1) :: (part.Column |> List.max |> (+) 1) :: part.Column }
        it

    let findNeighboringSymbols (symbols : Symbol list) positions =
        List.filter (fun (symbol : Symbol) -> List.contains symbol.Row positions.Rows && List.contains symbol.Column positions.Columns) symbols

    let partHasSymbolNeighbor (symbols: Symbol list) (part : Part) =
        buildPossibleNeighborsPositions part |> findNeighboringSymbols symbols |> (List.isEmpty >> not)

module One =
    let run lines =
        let parts, symbols = Seq.mapi Parser.parse lines
                             |> List.concat
                             |> List.partition (function Domain.Thing.Part _ -> true | Domain.Thing.Symbol _ -> false)
                             |> fun (parts, symbols) -> List.map (fun (Domain.Thing.Part part) -> part) parts, List.map (fun (Domain.Thing.Symbol symbol) -> symbol) symbols // is there a better way to do this? compiler is giving warnings.
        let values = List.filter (Neighbors.partHasSymbolNeighbor symbols) parts |> List.map (_.Value)
        List.sum values

module Two =
    let run lines =
        0

[<EntryPoint>]
let main _ =
    One.run lines |> printfn "%A"
    Two.run lines |> printfn "%A"
    0
