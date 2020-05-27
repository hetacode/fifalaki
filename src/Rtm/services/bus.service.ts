import amqp from "amqplib"

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
        ch.ack(msg!, true)
    })
}