import express from "express"
import http from "http"
import SocketIO from "socket.io"

const PORT=5050

const app = express()
const srv = http.createServer(app)
const io = SocketIO(srv)

io.on( "connection", s => {
    console.log(`Client connected: ${s.client.id}`)
    s.on("disconnect", () => {
        console.log(`Client connected: ${s.client.id}`)
    })
})

srv.listen(PORT, () => {
    console.log(`RTM service is listening on: ${PORT} port`)
})