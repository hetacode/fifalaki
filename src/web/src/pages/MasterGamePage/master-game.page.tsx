import React, { useState, useEffect } from 'react'
import { Player } from '../../model/player'
import { useRecoilValue } from 'recoil'
import { gameState } from '../../states/game.state'
import Counter from '../../components/counter'
import Word from '../../components/word'

interface Props {
}

const MasterGamePage = (props: Props) => {
    let timer: NodeJS.Timeout

    const state = useRecoilValue(gameState);

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

    return (
        <div>
            {level()}
        </div>
    )
}

export default MasterGamePage
