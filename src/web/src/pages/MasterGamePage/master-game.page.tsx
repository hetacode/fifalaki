import React, { useEffect, useState } from 'react'
import { useRecoilValue } from 'recoil'
import Counter from '../../components/counter'
import Word from '../../components/word'
import { gameSummarySelector } from '../../selectors/game-summary.selector'
import { gameState } from '../../states/game.state'
import { GameStateEnum } from '../../enums/game-state.enum'

interface Props {
}

const MasterGamePage = (props: Props) => {
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
            <div className="games-list">
                {state.players.map(m => <div className="player-item">
                    {m.Name}
                </div>)}
            </div>
        </div>
    }

    const waitingForNextLevel = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px" }}>Czekanie na nowy etap...</div>
            <Counter time={time} />
        </div>
    }

    const level = () => {
        return <div className="level-game-container">
            <div className="level-game">
                <Word word={state.letters ?? ""} />
                <Counter time={time} />
            </div>
        </div>
    }

    const summary = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px", fontSize: "30px" }}>PODSUMOWANIE</div>
            <div style={{ paddingBottom: "15px" }}>Liczba prób: {state.attempts}</div>
            {summarySelector.winner
                ? <div>Zwycięzca: {summarySelector.winner}</div>
                : null
            }
            {summarySelector.looser
                ? <div>Przegrany: {summarySelector.looser}</div>
                : null
            }
            <div style={{ paddingTop: "15px" }}>Punktacja:</div>
            {summarySelector.points.map(m => <div className="list-item">
                <div style={{ flex: 1 }}>{m.name}</div>
                <div>{m.points} pkt</div>
            </div>)}
        </div>
    }

    const endGame = () => {
        return <div className="master-game">
            <div className="word" style={{ paddingBottom: "15px", fontSize: "30px" }}>KONIEC GRY</div>

            {summarySelector.winner
                ? <div style={{ color: "green" }}>Zwycięzca: {summarySelector.winner}</div>
                : null
            }
            <button className="new-game" style={{ padding: "10 0" }}>MENU</button>
            <div style={{ paddingTop: "15px" }}>Punktacja:</div>

            {summarySelector.points.map(m => <div className="list-item">
                <div style={{ flex: 1 }}>{m.name}</div>
                <div>{m.points} pkt</div>
            </div>)}
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

export default MasterGamePage
