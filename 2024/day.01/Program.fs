module String =
    let split (c : char) (s :string) = s.Split(c)

type Pair =
    { Left : int
      Right : int }

let getInput =
    System.IO.File.ReadAllText("input.txt")

let parseToPair (s :string) =
    let lines = String.split '\n' s
    let pair = Seq.map (fun s -> String.split ' ' s) lines
    42

[<EntryPoint>]
let main _ =
    0