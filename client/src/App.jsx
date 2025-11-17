import { useState } from 'react'
import './App.css'
import Join from './components/Join/Join'
import Chat from './components/Chat/Chat'

function App() {
  const [chatVisibility, setChatVisibility] = useState(false)

  return (
    <div className="App">
      {
        chatVisibility ? <Chat /> :  <Join setChatVisibility={setChatVisibility} />
      }
    </div>
  )
}

export default App
