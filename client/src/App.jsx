import { useState } from 'react'
import './App.css'
import Join from './components/Join/Join'
import Chat from './components/Chat/Chat'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <Join/>
      <Chat />
    </>
  )
}

export default App
