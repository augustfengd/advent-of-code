module Lib =
    let read () = System.IO.File.ReadAllText "input.txt"

[<EntryPoint>]
let main _ =
    0