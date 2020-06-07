import { atom } from "recoil";
import { Player } from "../model/player";
import { GameStateEnum } from "../enums/game-state.enum";

export const gameState = atom<IGameState>({
    key: "gameState",
    default: {
        players: [],
        state: GameStateEnum.WaitingForPlayers,
        maxTime: Number.MAX_VALUE,
        letters: "",
        attempts: 5,
        winnerId: "",
        points: {},
        answers: []
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