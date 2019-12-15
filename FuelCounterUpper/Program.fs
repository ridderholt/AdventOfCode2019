// Learn more about F# at http://fsharp.org

open System


let getInput (path: string) =
       System.IO.File.ReadAllText path
       
let parseInput (content: string) =
    content.Split("\r\n") |> Array.map(fun num -> match Int32.TryParse num with
                                                  | (true, n) -> n
                                                  | _ -> failwith "Unable to parse input to int")


let subtractTwo n = n - 2

let calculateFuelMass n =
    (n / 3) |> Convert.ToDecimal |> Math.Floor |> Convert.ToInt32 |> subtractTwo
    
let rec calculateTotalFuelMass mass list =
    match calculateFuelMass mass with
    | n when n > 0 -> calculateTotalFuelMass (n) (n::list)
    | _ -> 0::list

[<EntryPoint>]
let main argv =
        
    let sum = getInput "input.txt"
                |> parseInput
                |> Array.map(calculateFuelMass)
                |> Array.map(fun mass -> calculateTotalFuelMass mass [mass] |> List.sum)
                |> Array.sum
    
    printf "Total sum: %d" sum
    
    0 // return an integer exit code
