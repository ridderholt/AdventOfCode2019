﻿namespace CrossedWires
open System
open CrossedWires.Common
open CrossedWires.Types
module Program =

    let move (movement: Movement) (coordinates: Coordinates) =
        let (x, y) = coordinates
        match movement.Direction with
        | Right -> [for n in 1..movement.Steps do yield (x+n, y)] 
        | Left -> [for n in 1..movement.Steps do yield (x-n, y)]
        | Up -> [for n in 1..movement.Steps do yield (x, y+n)]
        | Down -> [for n in 1..movement.Steps do yield (x, y-n)]
    
    let rec calculateCoordinates (coordinates: Coordinates list) (movements: Movement list) =
        match movements with
        | [] -> coordinates
        | x::xs -> calculateCoordinates ( coordinates @ (move x (last coordinates))) xs
    
    [<EntryPoint>]
    let main argv =
        
        let coordinates = getFileContent "input.txt"
                                |> splitIntoTwo
                                |> Array.map (fun str -> str.Split(','))
                                |> Array.map (fun arr -> Array.map toMovement arr)
                                |> Array.map Array.toList
                                |> Array.map (fun movements -> calculateCoordinates [(0, 0)] movements)
                                |> Array.toList
                                
        let matches = match coordinates with
                        | [xs;ys] -> intersect xs ys |> List.filter (fun (x, y) -> x <> 0 && y <> 0) //Get rid of starting point\
                        | _ -> failwith "To many lists in the list"
                        
        let ((x, y), steps) = matches |> List.map (fun (x, y) -> coordinates |> List.map(fun coords -> (x, y, coords |> List.takeWhile (fun (x', y') -> not (x = x' && y = y'))
                                                                                               |> List.fold (fun acc _ -> acc + 1) 0)))
                                      |> List.collect (fun x -> x) // Flat map, break of list of lists
                                      |> List.groupBy (fun (x, y, _) -> (x, y))
                                      |> List.map (fun (key, values) -> (key, values |> List.sumBy (fun (_, _, n) -> n)))
                                      |> List.sortBy (fun (_, n) -> n)
                                      |> List.head
        
        let distance = matches |> List.map (fun (x, y) -> toPositive x + toPositive y)
                               |> List.sort
                               |> List.head
                        
        printf "Distance: %d\r\n" distance
        printf "Minimum steps: %d for coordinates x: %d y: %d" steps x y
                            
        0 // return an integer exit code
