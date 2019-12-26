namespace CrossedWires.Types

type Direction = Right | Left | Up | Down

type Movement = {
    Direction: Direction
    Steps: int
}

type Coordinates = int * int
