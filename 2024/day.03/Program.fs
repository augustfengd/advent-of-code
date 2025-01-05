let tuple2 x y = x,y

let tuple2mapItem1 f = fun (x,y) -> f x,y

let uncurry f = fun (x,y) -> f x y 

let cons head tail = head :: tail

type Parser<'a> = Parser of (string -> Result<'a * string,string>)

module Parser =
    let run (Parser parse) s = parse s

    let bind f m = Parser (run m >> Result.bind (tuple2mapItem1 f >> uncurry run))
    
    let (>>=) m f = bind f m

    let return_ x = Parser (tuple2 x >> Ok)

    let apply a b =
        a >>= (fun f ->
        b >>= (fun x -> return_ (f x)))

    let (<*>) = apply

    let map f = apply (return_ f) 

    let andThen a b =
        a >>= (fun x ->
        b >>= (fun y -> return_ (x,y)))

    let (.>>.) = andThen

    let (.>>) a b = a .>>. b |>  map fst
    
    let (>>.) a b = a .>>. b |>  map snd

    let traverseM f t =
        let folder a b =
            f a >>= (fun x ->
            b   >>= (fun xs -> return_ (x :: xs)))
        List.foldBack folder t (return_ [])
    
    let orElse a b =
        let fn s = match run a s with
                   | Ok v -> Ok v
                   | _ -> run b s
        Parser fn

    let (<|>) = orElse
    
    let satisfy predicate =
        let fn = function
                 | null | "" -> Error "Missing input."
                 | s when predicate s.[0] -> Ok (s.[0], s.[1..])
                 | s -> Error $"Unexpected '{s.[0]}'"
        Parser fn
    
    let many parser =
        let rec parseZeroOrMore parser s = match run parser s with
                                           | Ok (parsed, remaining) -> parseZeroOrMore parser remaining |> tuple2mapItem1 (cons parsed)
                                           | Error _ -> [],s
        Parser (parseZeroOrMore parser >> Ok)

    let many1 parser =
        parser      >>= (fun x ->
        many parser >>= (fun xs -> return_ (x :: xs)))

    let between a b c =
        a >>. b .>> c

    module Parse =
        let char = satisfy System.Char.IsAscii
        let digit = satisfy System.Char.IsDigit
        let number = many1 digit
        let string_ s = 0

open Parser

[<EntryPoint>]
let main _ =
    match run Parse.char "asdf" with
    | Ok v -> printfn $"{v}"
    | Error e -> printfn $"{e}"
    0