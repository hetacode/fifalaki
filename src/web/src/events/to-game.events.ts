export class CreateGame {
    Type = "CreateGame";
    ClientId: string = ""
}

export class AddPlayer {
    Type = "AddPlayer";
    GameId = ""
    Name = ""
    Id = ""
}

export class StartGame {
    Type = "StartGame";
    GameId = ""
}

export class GiveAnswer {
    Type = "GiveAnswer";
    PlayerId = ""
    GameId = ""
    AnswerId = -1
}

export class CallEndGame {
    Type = "CallEndGame"
    GameId = ""
}