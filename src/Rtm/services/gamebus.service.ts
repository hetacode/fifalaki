import amqp, { Channel } from "amqplib"

 class GameBusService {
    channel?: Channel

    init = async () => {
        let con = await amqp.connect(process.env.RABBIT_SERVER as string)
        let ch = await con.createChannel()
        await ch.assertExchange("game-ex", "fanout", {
            durable: false,
            autoDelete: false
        })
        this.channel = ch
    }

    publish = (event: string) => {
        let bufMsg = Buffer.alloc(event.length, event, "utf-8")
        let res = this.channel?.publish("game-ex", "", bufMsg) ?? false
        console.log(`Event: {event} is published: {res}`)
    }
}

export const gameBusService = new GameBusService()