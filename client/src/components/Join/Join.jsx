import React, { useRef } from "react";

export default function Join({setChatVisibility}) {

    const userNameRef = useRef()

    const handleSubmit = () => {
        const userName = userNameRef.current.value
        if (!userName.trim()) return
        console.log('submit')
    }

    return (   
        <div>
            <h1>
                Join Chat
            </h1>
            <input type="text" ref={userNameRef} name="name" id="name" placeholder="Nome de usuário" />
            <button onClick={() => handleSubmit()}>Entrar</button>
        </div>
    )
}