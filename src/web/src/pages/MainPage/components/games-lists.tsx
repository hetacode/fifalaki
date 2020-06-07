import React from 'react'
import { useRecoilValue } from 'recoil'
import { gamesListState } from '../../../states/games-list.state'
import { useSendEvent } from '../../../hooks/api.hooks'
import { useHistory } from 'react-router-dom'

interface Props {

}

const GamesLists = (props: Props) => {
    const gamesList = useRecoilValue(gamesListState)
   const history = useHistory()

    const join = (gameId: string) => {
        history.push(`/game/${gameId}`)
    }

    return (
        <div className="list">
            {gamesList.map(f => <div className="list-item">
                <div style={{ flex: 1 }}>Gra: {f.id} | Liczba graczy: {f.playersCount}</div>
                <button onClick={() => join(f.id)}>Dołącz</button>
            </div>)}
        </div>
    )
}

export default GamesLists
