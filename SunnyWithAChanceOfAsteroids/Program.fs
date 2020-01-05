
open System
open SunnyWithAChanceOfAsteroids.Types
open SunnyWithAChanceOfAsteroids.Common

let output value =
    printf "%d\r\n" value
    
let getListInput = 1
//    printf "Please enter an input: "
//    match Int32.TryParse (stdin.ReadLine()) with
//    | (true, value) -> value
//    | _ -> failwith "User did not input a valid integer"
    
let executeOperation (list: int list) (instruction: Instruction) =
    match instruction with
    | Add (x, y, i) -> replaceAt i (x + y) list
    | Multiply (x, y, i) -> replaceAt i (x * y) list
    | Input i -> replaceAt i (getListInput) list
    | Output i -> output i
                  list
    | _ -> list

let rec execute (position: int) (list: int list): int list =
    let (instruction, continueAtPosition) = createInstruction position list
    match instruction with
    | Halt -> list
    | _ -> executeOperation list instruction |> execute continueAtPosition

[<EntryPoint>]
let main argv =

    let inputValues = getInput "input.txt" |> parseInput
    
    inputValues |> execute 0 |> ignore
    
    0 // return an integer exit code