import React, { useState } from 'react'

type SidebarProps = {
  tabs: string[]
  activeTab: string
  setActiveTab: (tab: string) => void
  sidebarOpen: boolean
  setSidebarOpen: (open: boolean) => void
}

export default function Sidebar({
  tabs,
  activeTab,
  setActiveTab,
  sidebarOpen,
  setSidebarOpen
}: SidebarProps) {
  const [searchTerm, setSearchTerm] = useState('')

  const filteredTabs = tabs.filter(tab =>
    tab.toLowerCase().includes(searchTerm.toLowerCase())
  )

  return (
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

      <div className="search-container">
        <input
          type="text"
          placeholder="Search topics..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="search-input"
          aria-label="Search tutorial topics"
        />
      </div>

      <nav className="nav" aria-label="Primary">
        {filteredTabs.map((tab) => (
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
  )
}