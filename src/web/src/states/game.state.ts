import { atom } from "recoil";
import { Player } from "../model/player";
import { GameStateEnum } from "../enums/game-state.enum";

export const gameState = atom<IGameState>({
    key: "gameState",
    default: {
        players: [
            { Id: "123", Name: "Zdzich" },
            { Id: "122", Name: "Dzon" }
        ],
        state: GameStateEnum.WaitingForPlayers,
        maxTime: Number.MAX_VALUE,
        letters: "SPZDAA",
        attempts: 5,
        winnerId: "123",
        points: {
            "123": 150,
            "122": 14
        },
        answers: [
            { Id: 1, Value: "SZPADA" },
            { Id: 2, Value: "CHOCO" }
        ]
    }
});

interface IGameState {
    players: Player[],
    state: GameStateEnum,
    maxTime: number,
    letters?: string,
    attempts?: number,
    winnerId?: string,
    looserId?: string,
    points?: { [key: string]: number; }
    answers?: { Id: number, Value: string }[]
}