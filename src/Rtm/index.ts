import express from "express"
import http from "http"
import SocketIO from "socket.io"
import dotenv from "dotenv"
import { usersService } from "./services/users.service"
import { busService } from "./services/bus.service"

dotenv.config()

const PORT = process.env.PORT


busService().then(() => {
    const app = express()
    const srv = http.createServer(app)
    const io = SocketIO(srv)

    io.on("connection", s => {
        console.log(`Client connected: ${s.client.id}`)

        usersService.addConnection(s.client.id, s)
        s.on("disconnect", () => {
            console.log(`Client connected: ${s.client.id}`)
            usersService.removeConnection(s.client.id)

            // TODO: send event to game-ex about user disconnection
            // TODO: dockerize this service - remembee node14
            // TODO: gateway rest - for game and games list services
        })
    })


    srv.listen(PORT, () => {
        console.log(`RTM service is listening on: ${PORT} port`)
    })
})