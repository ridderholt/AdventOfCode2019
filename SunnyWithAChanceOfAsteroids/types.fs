namespace SunnyWithAChanceOfAsteroids.Types

type Opcode =
    | Add = 1
    | Multiply = 2
    | Halt = 99

//type Instruction = {
//    Code: Opcode
//    Parameters: int * int * int
// }

type Mode =
    | Position = 0
    | Immediate = 1

type Instruction =
    | Add of int * int * int
    | Multiply of int * int * int
    | Halt
    | Input of int
    | Output of int
