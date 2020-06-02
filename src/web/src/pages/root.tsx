import React, { useEffect } from 'react'
import io from "socket.io-client"
import PlayerGamePage from './PlayerGamePage/player-game.page'

interface Props {

}

const Root = (props: Props) => {
    useEffect(() => {
        let socket = io(`${process.env.REACT_APP_RTM_ENDPOINT}`)
        console.log(socket.id)
        socket.on("event", (e: any) => {
            console.log(e)
        })
        socket.on("connect", () => {
            console.log(socket.id)
        })
    }, [])
    return (
        <div>
            <PlayerGamePage />
            {/* <MasterGamePage/> */}
            {/* <MainPage /> */}
        </div>
    )
}

export default Root
