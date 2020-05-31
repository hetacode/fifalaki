import React from 'react'
import { motion } from "framer-motion"
interface Props {
    letter: string
}

const Letter = (props: Props) => {
    return (
        <motion.div animate={{ y: [20, -20, 20] }} initial={false} transition={{ ease: "easeInOut", loop: Infinity, duration: 1.2, delay: Math.random() }}>
            <div className="letter">
                {props.letter}
            </div>
        </motion.div>
    )
}

export default Letter
