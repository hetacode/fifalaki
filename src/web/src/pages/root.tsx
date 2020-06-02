import React, { useEffect } from 'react'
import io from "socket.io-client"
import PlayerGamePage from './PlayerGamePage/player-game.page'
import { useSetRecoilState } from 'recoil'
import { appState, IAppState } from '../states/app.state'

interface Props {

}

const Root = (props: Props) => {
    const setAppState = useSetRecoilState(appState)

    useEffect(() => {
        let socket = io(`${process.env.REACT_APP_RTM_ENDPOINT}`)
        console.log(socket.id)
        socket.on("event", (e: any) => {
            console.log(e)
        })
        socket.on("connect", () => {
            console.log(`Connection: ${socket.id}`)
            setAppState((s: IAppState) => { return { ...s, connectionId: socket.id } })
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
