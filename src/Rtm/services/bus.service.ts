import amqp from "amqplib"

export const busService = async () => {
    let con = await amqp.connect(process.env.RABBIT_SERVER as string)
    let ch = await con.createChannel()
    await ch.assertExchange("game-ex", "fanout")
    await ch.assertQueue("rtm", {
        durable: false,
        autoDelete: false,
        exclusive: false
    })

    ch.consume("rtm", (msg) => {
        console.log(`Event: ${msg?.content.toLocaleString()}`)
        ch.ack(msg!, true)
    })
}