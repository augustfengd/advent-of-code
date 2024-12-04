open System

type State<'s, 'v> = State of ('s -> 'v * 's)

module State =
    let run (State f) state = f state
    
    let return_ v =
        let run state = v, state
        State run
    
    let bind f m =
        let run' state =
            let v, state' = run m state
            run (f v) state'
        State run'
    
    let get =
        let run' state = state, state
        State run'
    
    let put state =
        let run _ =
            (), state
        State run

    let zero =
        let run state =
            (), state
        State run
        

type StateBuilder () =
    member _.Return(v) = State.return_ v
    member _.ReturnFrom(m) = m
    member _.Bind(m,f) = State.bind f m
    member _.Zero() = State.zero
    member _.For(sequence: seq<'T>, body: 'T -> State<'s, unit>) : State<'s, unit> =
        sequence
        |> Seq.fold (fun acc elem ->
            State.bind (fun v -> State.bind v) acc
            ) (State.return_ ())

let state = StateBuilder()

module String =
    let split (c : char) (s : string) = s.Split(c) 

module Lib =
    
    type Direction =
        | Increasing
        | Decreasing

    type Safety =
        | Safe
        | Unsafe

    type Level = int32 list
        (*
        { One : int32
          Two : int32
          Three : int32
          Four : int32
          Five : int32
          Six : int32 }
          *)

    let IsIncreasing a b =
        match a - b with
        | diff when diff > 0 -> Some Decreasing
        | diff when diff < 0 -> Some Increasing
        | _ -> None
    
    let validDifference n = n >= 1 && n <= 3 
    
    let validate (direction, difference) = state {
        let! state = State.get
        match state with
        | Some (v, Safe) ->
            if (direction = v || direction = None) && validDifference difference then
                do! Some (v, Safe) |> State.put
            else
                do! Some (v, Unsafe) |> State.put
        | Some (_, Unsafe)
        | None -> ()
    }
    
    let computeDirectionAndDifference a b = (IsIncreasing a b, System.Math.Abs(a - b))

    // XXX: not the best usage of the state monad? i'm not using the first value of the computed tuple.
    let check level = state {
        let pairs = List.pairwise levels
        for (a,b) in pairs do
            do! validate (computeDirectionAndDifference a b)
    }

    let getInput ()  = System.IO.File.ReadAllText "input.txt"
    
    let toLines = String.split '\n'

    let parse =
        toLines
        >> Array.map (String.split ' ')
        >> Array.map (Array.map System.Int32.Parse)
        >> Array.map (fun v -> { One = v.[0]
                                 Two = v.[1]
                                 Three = v.[2]
                                 Four = v.[3]
                                 Five = v.[4]
                                 Six = v.[5] })

    let runCheck level =
        State.run (check level) None

module App =
    open Lib

    let part1 () =
        let s = getInput ()
        let levels = parse s
        let runs = Array.map runCheck levels
        runs
        |> Array.map snd
        |> Array.choose id
        |> Array.map snd
        |> Array.sumBy (function | Safe -> 1 | Unsafe -> 0)

[<EntryPoint>]
let main _ =
    App.part1 () |> printfn "%d" 
    0