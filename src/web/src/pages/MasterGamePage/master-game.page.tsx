import React from 'react'
import { Player } from '../../model/player'
import { useRecoilValue } from 'recoil'
import { gameState } from '../../states/game.state'

interface Props {
}

const MasterGamePage = (props: Props) => {
    const state = useRecoilValue(gameState);

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

    return (
        <div>
            {waitingForPlayersState()}
        </div>
    )
}

export default MasterGamePage
