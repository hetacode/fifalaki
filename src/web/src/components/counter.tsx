import React from 'react'

interface Props {
    time: number
}

const Counter = (props: Props) => {
    return (
        <div className="counter">
            {(props.time <= 0) ? "--" : props.time}
        </div>
    )
}

export default Counter
