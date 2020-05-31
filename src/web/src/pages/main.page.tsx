import React from 'react'
import Word from '../components/word'

interface Props {

}

const MainPage = (props: Props) => {
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
