import React, { useState, useEffect } from 'react'
import { useRecoilValue } from 'recoil'
import { gameSummarySelector } from '../../selectors/game-summary.selector'
import { gameState } from '../../states/game.state'
import Counter from '../../components/counter'
import { GameStateEnum } from '../../enums/game-state.enum'

interface Props {

}

const PlayerGamePage = (props: Props) => {
    let timer: NodeJS.Timeout

    const state = useRecoilValue(gameState)
    const summarySelector = useRecoilValue(gameSummarySelector)

    const [time, setTime] = useState(0);

    useEffect(() => {
        timer = setInterval(() => { updateTime() }, 1000)
        return () => {
            clearInterval(timer)
        }
    }, [])
    useEffect(() => {
        setTime(0)
    }, [state.state])
    useEffect(() => {
        if (time >= state.maxTime) {
            setTime(time => 0)
            clearInterval(timer)
        }
    }, [time])


    function updateTime() {
        setTime(time => time + 1)
    }

    const waitingForPlayersState = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px" }}>Czekanie na graczy...</div>
            <button className="new-game">START</button>
        </div>
    }

    const waitingForNextLevel = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px" }}>Czekanie na nowy etap...</div>
            <Counter time={time} />
        </div>
    }

    const summary = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px", fontSize: "30px" }}>PODSUMOWANIE</div>
        </div>
    }

    const endGame = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px", fontSize: "30px" }}>KONIEC GRY</div>
        </div>
    }

    const level = () => {
        return <div className="level-game-container">
            <div className="level-game">
                <Counter time={time} />
                {state.answers?.map(m => <button className="answer-button">{m.Value}</button>)}
            </div>
        </div>
    }


    const renderState = () => {
        switch (state.state) {
            case GameStateEnum.WaitingForPlayers:
                return waitingForPlayersState()
            case GameStateEnum.WaitingForLevel:
                return waitingForNextLevel()
            case GameStateEnum.SummaryLevel:
                return summary()
            case GameStateEnum.Level:
                return level()
            case GameStateEnum.EndGame:
                return endGame()
        }
    }

    return (
        <div>
            {renderState()}
        </div>
    )
}

export default PlayerGamePage
