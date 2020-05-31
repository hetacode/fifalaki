import React, { useEffect } from 'react'
import Word from '../../components/word'
import { useGetGamesList } from '../../hooks/api.hooks'

interface Props {

}

const MainPage = (props: Props) => {
    const gamesList = useGetGamesList()

    useEffect(() => {
        gamesList()
    }, [])

    return (
        <div className="main-page">
            <Word word="FIFALAKI" />
            <button className="new-game">NOWA GRA</button>
            <div className="games-list">

            </div>
        </div>
    )
}

export default MainPage
