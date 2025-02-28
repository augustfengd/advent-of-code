﻿let tuple2 = fun x y -> x,y

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
                                           | Error e ->
                                               printfn $"{e}"
                                               [],s
        Parser (parseZeroOrMore parser >> Ok)

    let many1 parser =
        parser      >>= (fun x ->
        many parser >>= (fun xs -> return_ (x :: xs)))

    let between a b c = a >>. b .>> c

    let opt c = satisfy ((=) c) |> map Some <|> return_ None

    let parseDigit = satisfy System.Char.IsDigit |> map (string >> int)

    let parseNumber = many1 parseDigit |> map (List.reduce (fun a b -> a * 10 + b))

    let parse : string -> Parser<string> = Seq.toList >> traverseM (fun c -> satisfy ((=) c)) >> map (Seq.toArray >> System.String)

module Lib =
    open Parser

    let parseManyCharExceptM = many (satisfy ((<>) 'm'))
    
    let parseCharM = satisfy ((=) 'm')

    let parseMul = parse "mul" >>. parse "(" >>. parseNumber .>> parse "," .>>. parseNumber .>> parse ")"

    let parse = many (parseManyCharExceptM >>. parseMul <|> (parseManyCharExceptM >>. parseCharM >>. return_ (0,0)))
                      
    let read () = System.IO.File.ReadAllText "input.txt"

    let run =
        read
        >> run parse
        >> Result.map fst
        >> Result.map (List.map (uncurry (*)))
        >> Result.map List.sum

open Lib

[<EntryPoint>]
let main _ =
    match run () with
    | Ok v -> printfn $"{v}"
    | Error e -> eprintfn $"{e}"
    0