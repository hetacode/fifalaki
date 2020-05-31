import React, { useEffect } from 'react'
import Word from '../../components/word'
import { useGetGamesList } from '../../hooks/api.hooks'
import GamesLists from './components/games-lists'

interface Props {

}

const MainPage = (props: Props) => {
    const gamesList = useGetGamesList()

    useEffect(() => {
        gamesList()
    }, [])

    return (
        <div className="main-page" style={{ height: "100vh"}}>
            <div>
                <Word word="FIFALAKI" />
            </div>
            <button className="new-game">NOWA GRA</button>
            <div className="games-list">
                <GamesLists />
            </div>
        </div>
    )
}

export default MainPage
