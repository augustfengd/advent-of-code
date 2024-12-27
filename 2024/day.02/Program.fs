module String =
    let split (c : char) (s : string) = s.Split(c) 

let curry f = fun x y -> f(x,y)

type State<'s, 'v> = State of ('s -> 's * 'v)

module State =
    let run (State f) state = f state
    let return_ v = State (fun s -> s, v)
    let bind m f = State (fun s -> let s', v = run m s in run (f v) s')
    let get = State (fun s -> s, s)
    let put s = State (fun _ -> s, ())

type StateBuilder () =
    member _.Bind(m,f) = State.bind m f
    member _.Return(v) = State.return_ v
    member _.ReturnFrom(m) = m
    
let state = StateBuilder()

module List =
    module State =
        let traverseM f t =
            let (>>=) = State.bind
            let return_ = State.return_
        
            let init = return_ []
            let folder head tail =
                f head >>= (fun x ->
                tail   >>= (fun xs -> return_ (x :: xs)))
        
            List.foldBack folder t init

module Lib =
    type Direction =
        | Increasing
        | Decreasing

    type Safeness =
        | Safe
        | Unsafe

    type Progress = {
        Safeness: Safeness
        Direction: Direction option
        Level: int option
    }

    let IsIncreasing = function
        | diff when diff > 0 -> Some Decreasing
        | diff when diff < 0 -> Some Increasing
        | _ -> None
    
    let computeDirectionAndDistance = function
        | _, None -> None, None
        | a, Some b -> IsIncreasing (a - b), Some (System.Math.Abs(a - b))

    let checkDistanceSafeness = function
        | Some n -> if n >= 1 && n <= 3 then Safe else Unsafe
        | None -> Safe

    let checkDirectionSafeness  = function
        | Some Increasing, Some Increasing -> Safe
        | Some Decreasing, Some Decreasing -> Safe
        | None, _ -> Safe
        | _ -> Unsafe

    let check n = state {
        let! progress = State.get
        let direction, distance = curry computeDirectionAndDistance n progress.Level

        let directionSafeness = checkDirectionSafeness (progress.Direction, direction)
        let distanceSafeness = checkDistanceSafeness distance
        let safeness = if directionSafeness = Safe && distanceSafeness = Safe then Safe else Unsafe

        do! State.put { progress with Safeness = safeness ; Direction = direction ; Level = Some n }
        return safeness
    }

    let foldSafeness = List.fold (function current -> function | Unsafe -> Unsafe | _ -> current) Safe

    let checkReport levels =
       let init = { Safeness = Safe ; Direction = None ; Level = None }
       let checker = List.State.traverseM check levels
       State.run checker init |> snd |> foldSafeness

    let getInput ()  = System.IO.File.ReadAllText "input.txt"

    let toLines = String.split '\n'

    let parse =
        toLines
        >> Array.map (String.split ' ')
        >> Array.map (Array.map System.Int32.Parse)

    let run  =
        getInput
        >> parse
        >> Array.map (Array.toList >> checkReport)
        >> Array.filter (function | Safe -> true | Unsafe -> false)
        >> Array.length

module App =
    open Lib
    let part1 = run

[<EntryPoint>]
let main _ =
    App.part1 () |> printfn "%A"
    0