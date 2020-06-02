import React from 'react'
import { useRecoilValue } from 'recoil'
import { gamesListState } from '../../../states/games-list.state'

interface Props {

}

const GamesLists = (props: Props) => {
    const gamesList = useRecoilValue(gamesListState)

    return (
        <div className="list">
            {gamesList.map(f => <div className="list-item">
                <div style={{ flex: 1 }}>Gra: {f.id} | Liczba graczy: {f.playersCount}</div>
                <button>Dołącz</button>
            </div>)}
        </div>
    )
}

export default GamesLists
