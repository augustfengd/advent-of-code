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
          Columns: int list }

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

    let isSymbol = or' isDigit ((=) '.') >> not

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
                                                                                                Columns = List.map (fun (token : Token) -> token.Column) group } :: things
                                                                                 | { Char = c ; Row = n ; Column = m} :: []
                                                                                     when isSymbol c ->
                                                                                         Symbol { Value = c
                                                                                                  Row = n
                                                                                                  Column = m } :: things
                                                                                 | fml -> failwith "I give up on safety; it won't reach here anyways. Hit me up when it does.") [] groups
        things |> List.rev


module Adjacent =
    open Domain

    let buildAdjacentPositionsForPart (part : Part) =
        let it = { Rows = [ part.Row - 1 ; part.Row ; part.Row + 1 ]
                   Columns = (part.Columns |> List.min |> (-) <| 1) :: (part.Columns |> List.max |> (+) 1) :: part.Columns }
        it

    let findAdjacentSymbols (symbols : Symbol list) positions =
        List.filter (fun (symbol : Symbol) -> List.contains symbol.Row positions.Rows && List.contains symbol.Column positions.Columns) symbols

    let partHasAdjacentSymbol (symbols: Symbol list) (part : Part) =
        buildAdjacentPositionsForPart part |> findAdjacentSymbols symbols |> (List.isEmpty >> not)

    let buildAdjacentPositionsForSymbol (symbol: Symbol) =
        let it =
            { Rows = [ symbol.Row - 1; symbol.Row; symbol.Row + 1 ]
              Columns = (symbol.Column |> (-) <| 1) :: (symbol.Column |> (+) 1) :: symbol.Column :: [] }
        it

    let findAdjacentParts (parts: Part list) positions =
        let listIntersect a b = Set.intersect (Set.ofList a) (Set.ofList b) |> Set.toList
        List.filter (fun (part: Part) -> List.contains part.Row positions.Rows && listIntersect part.Columns positions.Columns |> List.isEmpty |> not) parts

    let symbolHasAdjacentParts (parts: Part list) (n : int) (symbol: Symbol) =
        buildAdjacentPositionsForSymbol symbol |> findAdjacentParts parts |> List.length |> (>=) n |> not

module One =
    let run lines =
        let parts, symbols = Seq.mapi Parser.parse lines
                             |> List.concat
                             |> List.partition (function Domain.Thing.Part _ -> true | Domain.Thing.Symbol _ -> false)
                             |> fun (parts, symbols) -> List.map (fun (Domain.Thing.Part p) -> p) parts, List.map (fun (Domain.Thing.Symbol s) -> s) symbols // is there a better way to do this? compiler is giving warnings.
        List.filter (Adjacent.partHasAdjacentSymbol symbols) parts |> List.sumBy (_.Value)

module Two =
    let run lines =
        let parts, symbols = Seq.mapi Parser.parse lines
                             |> List.concat
                             |> List.partition (function Domain.Thing.Part _ -> true | Domain.Thing.Symbol _ -> false)
                             |> fun (parts, symbols) -> List.map (fun (Domain.Thing.Part part) -> part) parts, List.map (fun (Domain.Thing.Symbol symbol) -> symbol) symbols // is there a better way to do this? compiler is giving warnings.
        List.filter (Adjacent.symbolHasAdjacentParts parts 2) symbols |> List.sumBy (_.Value)

[<EntryPoint>]
let main _ =
    One.run lines |> printfn "%A"
    Two.run lines |> printfn "%A"
    0
