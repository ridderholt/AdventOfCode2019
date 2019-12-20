// Learn more about F# at http://fsharp.org

open System

type Opcode =
    | Add = 1
    | Multiply = 2
    | Halt = 99

type Instruction = {
    Code: Opcode
    Parameters: int * int * int
 }

let intToOpcode n =
    match n with
    | 1 -> Opcode.Add
    | 2 -> Opcode.Multiply
    | 99 -> Opcode.Halt
    | _ -> failwith "Invalid Opcode"

let getInput (path: string) =
       System.IO.File.ReadAllText path

let parseInput (content: string) =
    content.Split(",") |> Array.map (fun num -> match Int32.TryParse num with
                                                  | (true, n) -> n
                                                  | _ -> failwith "Unable to parse input to int")
                       |> Array.toList

let replaceAt (index: int) (replacement: int) (list: int list) =
    list |> List.mapi (fun i n -> match i = index with
                                  | true -> replacement
                                  | false -> n)

let executeOperation (list: int list) (instruction: Instruction) =
    match instruction with
    | { Code = Opcode.Add; Parameters = (x, y, r); } -> replaceAt r (list.[x] + list.[y]) list
    | { Code = Opcode.Multiply; Parameters = (x, y, r); } -> replaceAt r (list.[x] * list.[y]) list
    | _ -> list

let createInstruction (code: Opcode) (position: int) (list: int list) =
    { Code = code; Parameters = (list.[position + 1], list.[position + 2], list.[position + 3]); }

let rec execute (position: int) (list: int list): int list =
    match intToOpcode list.[position] with
    | Opcode.Halt -> list
    | opcode -> createInstruction opcode position list |> executeOperation list |> execute (position + 4)

[<EntryPoint>]
let main argv =

    let nounsAndVerbs = [ for n in 0..99 do yield (n, [ 0..99 ]) ]
    let inputValues = getInput "input.txt" |> parseInput

    nounsAndVerbs |> List.map (fun (noun, verbs) -> verbs |> List.map (fun verb -> (
                                                                                   inputValues
                                                                                       |> replaceAt 1 noun
                                                                                       |> replaceAt 2 verb
                                                                                       |> execute 0
                                                                                       |> List.head
                                                                                   , (noun, verb)))
                                                   |> List.filter (fun (output, _) -> output = 19690720)
                              )
                  |> List.filter (fun list -> list.Length > 0)
                  |> List.map List.head
                  |> List.iter (fun (output, (noun, verb)) -> printf "%d and %d gave %d and (100 * noun + verb) is: %d" noun verb output (100 * noun + verb))


    0 // return an integer exit code
