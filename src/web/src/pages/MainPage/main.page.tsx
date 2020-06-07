import React, { useEffect } from 'react'
import Word from '../../components/word'
import { useGetGamesList } from '../../hooks/api.hooks'
import GamesLists from './components/games-lists'
import { useHistory } from 'react-router-dom'

interface Props {

}

const MainPage = (props: Props) => {
    const gamesList = useGetGamesList()
    const history = useHistory()

    useEffect(() => {
        gamesList()
    }, [])

    const createGame = () => {
        history.push("/master")
    }

    return (
        <div className="main-page" style={{ height: "100vh" }}>
            <div>
                <Word word="FIFALAKI" />
            </div>
            <button className="new-game" onClick={c => createGame()}>NOWA GRA</button>
            <div className="games-list">
                <GamesLists />
            </div>
        </div>
    )
}

export default MainPage
