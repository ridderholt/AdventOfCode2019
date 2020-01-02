// Learn more about F# at http://fsharp.org

open System

let toSingleNumbers (num: int) =
    num.ToString().ToCharArray() |> Array.map(fun c -> match Int32.TryParse (c.ToString()) with
                                                        | (true, n) -> n
                                                        | _ -> failwith "Unable to parse number")
                                 |> Array.toList
    
    
let rec hasTwoAdjacentDuplicates (numbers: int list): bool =
    match numbers with
    | a::b::_ when a = b -> true
    | _::b::tail -> hasTwoAdjacentDuplicates (b::tail)
    | _ -> false

let rec noDecreasingNumbers (numbers: int list) =
    match numbers with
    | a::b::_ when a > b -> false
    | _::b::tail -> noDecreasingNumbers (b::tail)
    | _ -> true

[<EntryPoint>]
let main argv =
    
    let input = [123257..647015]
//    let input = [111111;223450;123789;224553]
    
    let matchingNumbers = input |> List.map toSingleNumbers
                                |> List.filter hasTwoAdjacentDuplicates
                                |> List.filter noDecreasingNumbers
                                
    printf "%d numbers matches criterias" matchingNumbers.Length
    
    0 // return an integer exit code
