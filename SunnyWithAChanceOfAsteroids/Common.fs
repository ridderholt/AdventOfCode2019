namespace SunnyWithAChanceOfAsteroids

open System
open SunnyWithAChanceOfAsteroids.Types

module Common =

    let getInput (path: string) =
        System.IO.File.ReadAllText path

    let parseInput (content: string) =
        content.Split(",")
        |> Array.map (fun num ->
            match Int32.TryParse num with
            | (true, n) -> n
            | _ -> failwith "Unable to parse input to int")
        |> Array.toList

    let replaceAt (index: int) (replacement: int) (list: int list) =
        list
        |> List.mapi (fun i n ->
            match i = index with
            | true -> replacement
            | false -> n)

    let getValueAt (list: int list) (i: int) = list.[i]

    let toSingleNumbers (num: int) =
        num.ToString().ToCharArray()
            |> Array.map (fun c ->
                match Int32.TryParse(c.ToString()) with
                | (true, n) -> n
                | _ -> failwith "Unable to parse number")
            |> Array.toList

    let getValueByMode (list: int list) (mode: Mode) (parameterPosition: int) =
        let valueAt = getValueAt list
        match mode with
        | Mode.Position -> valueAt list.[parameterPosition]
        | Mode.Immediate -> valueAt parameterPosition
        | _ -> failwith "Invalid value for Mode"

    let createMode n =
        match n with
        | 1 -> Mode.Immediate
        | 0 -> Mode.Position
        | _ -> failwith (sprintf "Invalid mode %d" n)
        
    let padList (list: int list) =
        match list.Length with
        | 3 -> 0::0::list
        | 4 -> 0::list
        | 5 -> list
        | _ -> failwith "Invalid length"

    let createInstruction (position: int) (list: int list) =
        let valueAt = getValueByMode list Mode.Position
        let valueByMode = getValueByMode list
        match list.[position] with
        | 99 -> (Halt, 0)
        | 1 -> (Add(valueAt (position + 1), valueAt (position + 2), list.[position + 3]), (position + 4))
        | 2 -> (Multiply(valueAt (position + 1), valueAt (position + 2), list.[position + 3]), (position + 4))
        | i when i.ToString().Length >= 3 ->

            match padList (toSingleNumbers i) with
            | [ _; second; first; _; 1 ] ->
                (Add (valueByMode (createMode first) (position + 1),
                      valueByMode (createMode second) (position + 2),
                      list.[position + 3]), (position + 4))
            | [ _; second; first; _; 2 ] ->
                (Multiply (valueByMode (createMode first) (position + 1),
                      valueByMode (createMode second) (position + 2),
                      list.[position + 3]), (position + 4))
            | [ _; _; second; _; 4 ] ->
                (Output (valueByMode (createMode second) (position + 1)), (position + 2))
            | _ -> failwith (sprintf "Unable to create instruction based on %d at position %d" i position)


        | 3 -> (Input (list.[position + 1]), (position + 2))
        | 4 -> (Output (list.[list.[position + 1]]), (position + 2))
        | x -> failwith (sprintf "Invalid instruction: %d at %d" x position)
