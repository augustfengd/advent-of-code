let lines = System.IO.File.ReadAllLines "input"

// get all numbers
// for each number, travel around and see if there's an adjacent symbol.
// if there is, sum.

type Part = {
    Number: int
    Positions: int list
    Neighbors: int list
}

module Parse =
    let isDigit = System.Char.IsDigit
    let isPeriod = (=) '.'
    let isSymbol = (fun c -> isDigit c && isPeriod c) >> not 

    let width = List.head >> String.length

    let parts text =
        let findPartNumber x xs = "123"
        let inner acc position rest =
            match rest with
            | x :: xs when isDigit x ->
                let s = findPartNumber x xs |> int
                let positions = List.init (String.length number) (fun i -> position + i)
                let neighbors =
                    let left = 0
                    let right = 0
                    let topleft = 0
                    let topright = 0
                    let botttomleft = 0
                    let bottomright = 0
                    let top = [1,2,3]
                    let bottom = [1,2,3]
                    List.append left bottom
                { Number = s
                  Positions = positions
                  Neighbors = neighbors }
            | _ ->

    let foobar lines =
        { Number = 1
          Row = 1
          Column = [1 ; 2 ; 3] }

    let run = 0

[<EntryPoint>]
let main _ =
    Parse.r "467..114.." |> printfn "%A"
    0
