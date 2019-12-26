namespace CrossedWires
    open CrossedWires.Types
    open System

    module Common =
    
        let getFileContent (path: string) =
            System.IO.File.ReadAllText path
            
        let splitIntoTwo (fileContents: string) =
            fileContents.Split("\r\n")
            
        let toMovement (input: string): Movement =
            let firstChar = input.ToCharArray() |> Array.head
            let rest = input.Substring(1, input.Length - 1)
            
            match Int32.TryParse rest with
            | (true, x) -> match firstChar with
                           | 'R' -> { Direction = Direction.Right; Steps = x }
                           | 'L' -> { Direction = Direction.Left; Steps = x }
                           | 'U' -> { Direction = Direction.Up; Steps = x }
                           | 'D' -> { Direction = Direction.Down; Steps = x }
                           | _ -> failwith "Unable to parse direction"
            | _ -> failwith "Could not parse steps"
            
        let last list =
            list |> List.rev |> List.head
            
        let rec intersect xs ys =
            Set.intersect (Set.ofList xs) (Set.ofList ys) |> Set.toList
            
        let toPositive (n:int) =
            match n with
            | i when i < 0 -> (-1)*n
            | _ -> n
