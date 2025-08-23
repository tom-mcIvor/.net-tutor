import { useState } from 'react'
import './App.css'
import Sidebar from './components/Sidebar'
 
const TABS = [
  'Introduction to .NET',
  'C# Basics',
  'ASP.NET Core Overview',
  'Entity Framework Core',
  '.NET Libraries & Tools',
  'Advanced Topics',
  'from node.js to .NET'
]
 
function App() {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const [activeTab, setActiveTab] = useState(TABS[0])
 
  return (
    <div className="layout">
      <Sidebar
        tabs={TABS}
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        sidebarOpen={sidebarOpen}
        setSidebarOpen={setSidebarOpen}
      />
 
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
