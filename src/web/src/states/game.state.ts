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
        letters: "SPZDAA"
    }
});

interface IGameState {
    players: Player[],
    state: GameStateEnum,
    maxTime: number,
    letters?: string
}