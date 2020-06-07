import { useEffect } from "react";
import { gameState } from "../states/game.state";
import { useSetRecoilState } from "recoil";
import { GameStateEnum } from "../enums/game-state.enum";

export function useEventsProcessor() {
    const setGameState = useSetRecoilState(gameState)

    const process = (e: any) => {
        switch (e.Type) {
            case "UpdatedPlayers":
                setGameState(s => { return { ...s, players: e.Players } })
                break;
            case "WaitingForNextLevel":
                setGameState(s => { return { ...s, state: GameStateEnum.WaitingForLevel, maxTime: e.StateTime } })
                break;
            case "NewLevel":
                setGameState(s => { return { ...s, state: GameStateEnum.Level, letters: e.Letters, answers: e.Answers } })
                break;
            case "SummaryLevel":
                setGameState(s => { return { ...s, state: GameStateEnum.SummaryLevel, points: e.Points, attempts: e.GameAttempts, winnerId: e.WinnerId, looserId: e.LooserId } })
                break;
            case "EndGame":
                setGameState(s => { return { ...s, state: GameStateEnum.EndGame, points: e.Points, winnerId: e.WinnerId } })
                break;
        }

    }

    return process
}