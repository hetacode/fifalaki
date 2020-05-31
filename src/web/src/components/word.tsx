import React from 'react'
import Letter from './letter'

interface Props {
    word: string
}

const Word = (props: Props) => {
    function* enumerateLetters() {
        for (let i of props.word) {
            yield <Letter letter={i} />
        }
    }
    return (
        <div className="word">
            {Array.from(enumerateLetters())}
        </div>
    )
}

export default Word
