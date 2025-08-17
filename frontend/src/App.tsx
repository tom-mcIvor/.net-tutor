import { useState } from 'react'
import './App.css'
 
const TABS = [
  'Introduction to .NET',
  'C# Basics',
  'ASP.NET Core Overview',
  'Entity Framework Core',
  '.NET Libraries & Tools',
  'Advanced Topics'
]
 
function App() {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const [activeTab, setActiveTab] = useState(TABS[0])
 
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
 
        <nav className="nav" aria-label="Primary">
          {TABS.map((tab) => (
            <button
              key={tab}
              className={`nav-link ${activeTab === tab ? 'active' : ''}`}
              onClick={() => {
                setActiveTab(tab)
                setSidebarOpen(false)
              }}
              aria-current={activeTab === tab ? 'page' : undefined}
            >
              {tab}
            </button>
          ))}
        </nav>
 
        <div className="sidebar-footer">
          <a className="nav-link small" href="https://learn.microsoft.com/dotnet/" target="_blank" rel="noreferrer">Microsoft .NET</a>
          <a className="nav-link small" href="https://docs.microsoft.com/dotnet/csharp/" target="_blank" rel="noreferrer">C# Docs</a>
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
          <h1 className="page-title">{activeTab}</h1>
        </header>
 
        <main className="main">
          <section className="card">
            <h2>{activeTab}</h2>
            <p>
              This section contains introductory content for <strong>{activeTab}</strong>. Replace
              this placeholder with real lesson content or link to detailed lesson pages.
            </p>
            <p>
              Current tabs are implemented as an in-app navigation. Clicking a tab updates the
              content area and (on small screens) closes the sidebar for a better mobile experience.
            </p>
          </section>
        </main>
      </div>
    </div>
  )
}
 
export default App
