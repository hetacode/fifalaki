import { selector } from "recoil";
import { gameState } from "../states/game.state";

export const gameSummarySelector = selector({
    key: "gameSummarySelector",
    get: (g) => {
        let state = g.get(gameState)
        let points = state.players.map(m => {
            return {
                name: m.Name,
                points: state.points ? state.points[m.Id] : 0
            } as { name: string, points: number }
        })
        let result = {
            winner: state.winnerId ? state.players.filter(f => f.Id ===state.winnerId)[0].Name : null,
            looser: state.looserId ? state.players.filter(f => f.Id ===state.looserId)[0].Name : null,
            points: points
        }
        return result
    }
})