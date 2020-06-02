import React, { useState, useEffect } from 'react'
import { Player } from '../../model/player'
import { useRecoilValue } from 'recoil'
import { gameState } from '../../states/game.state'
import Counter from '../../components/counter'

interface Props {
}

const MasterGamePage = (props: Props) => {
    let timer: NodeJS.Timeout

    const state = useRecoilValue(gameState);

    const [time, setTime] = useState(0);

    useEffect(() => {
        timer = setInterval(() => {updateTime()}, 1000)
        return () => {
            clearInterval(timer)
        }
    }, [])

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
            <Counter time={time}/>
        </div>
    }

    return (
        <div>
            {waitingForNextLevel()}
        </div>
    )
}

export default MasterGamePage
