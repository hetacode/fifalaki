import { atom } from "recoil";
import { Player } from "../model/player";

export const gameState = atom<IGameState>({
    key: "gameState",
    default: {
        players: [
            { Id: "123", Name: "Zdzich" },
            { Id: "122", Name: "Dzon" }
        ]
    }
});

interface IGameState {
    players: Player[]
}