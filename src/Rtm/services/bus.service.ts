import amqp from "amqplib"
import { IEvent } from "../event"
import { usersService } from "./users.service"

export const busService = async () => {
    let con = await amqp.connect(process.env.RABBIT_SERVER as string)
    let ch = await con.createChannel()
    await ch.assertExchange("rtm-ex", "fanout", {
        durable: false,
        autoDelete: false
    })

    await ch.assertQueue("rtm", {
        durable: false,
        autoDelete: false,
        exclusive: false
    })

    await ch.bindQueue("rtm", "rtm-ex", "")

    ch.consume("rtm", (msg) => {
        console.log(`Event: ${msg?.content?.toLocaleString()}`)

        let e = JSON.parse(msg?.content?.toLocaleString() ?? "") as IEvent
        
        e.PlayersIds.forEach(f => {
            let conn = usersService.user.get(f)
            conn?.emit("event", e)
        })

        ch.ack(msg!, true)
    })
}