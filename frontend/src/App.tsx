import { useState } from 'react'
import './App.css'

function App() {
  const [sidebarOpen, setSidebarOpen] = useState(false)

  return (
    <div className="layout">
      <aside className={`sidebar ${sidebarOpen ? 'open' : ''}`}>
        <div className="sidebar-header">
          <span className="brand">.NET Tutor</span>
          <button
            className="close-btn"
            aria-label="Close sidebar"
            onClick={() => setSidebarOpen(false)}
          >
            ✕
          </button>
        </div>
        <nav className="nav">
          <a className="nav-link" href="/">Home</a>
          <a className="nav-link" href="/lessons">Lessons</a>
          <a className="nav-link" href="/about">About</a>
        </nav>
        <div className="sidebar-footer">
          <a className="nav-link small" href="https://react.dev" target="_blank" rel="noreferrer">React</a>
          <a className="nav-link small" href="https://vite.dev" target="_blank" rel="noreferrer">Vite</a>
        </div>
      </aside>

      <div className="content">
        <header className="topbar">
          <button
            className="hamburger"
            aria-label="Open sidebar"
            onClick={() => setSidebarOpen(true)}
          >
            ☰
          </button>
          <h1 className="page-title">Welcome</h1>
        </header>

        <main className="main">
          <section className="card">
            <h2>Vite + React</h2>
            <p>This is your app content area. The sidebar is persistent on desktop and toggleable on mobile.</p>
            <p>Edit <code>src/App.tsx</code> to customize this layout.</p>
          </section>
        </main>
      </div>
    </div>
  )
}

export default App
