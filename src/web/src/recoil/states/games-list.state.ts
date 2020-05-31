import { atom } from "recoil";
import { GameListItem } from "../../model/game-list-item";

export const gamesListState = atom<GameListItem[]>({
    key: "gamesListState",
    default: []
})