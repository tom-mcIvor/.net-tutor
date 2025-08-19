import { useState } from 'react'
import { useAuth } from '../contexts/AuthContext'
import { Login } from './Login'
import { Register } from './Register'

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
  const [showLogin, setShowLogin] = useState(false)
  const [showRegister, setShowRegister] = useState(false)
  const [showUserMenu, setShowUserMenu] = useState(false)
  const { user, logout } = useAuth()

  const filteredTabs = tabs.filter(tab =>
    tab.toLowerCase().includes(searchTerm.toLowerCase())
  )

  const handleAuthClick = () => {
    setShowLogin(true)
  }

  const handleLogout = () => {
    logout()
    setShowUserMenu(false)
  }

  const switchToRegister = () => {
    setShowLogin(false)
    setShowRegister(true)
  }

  const switchToLogin = () => {
    setShowRegister(false)
    setShowLogin(true)
  }

  const closeAuthModals = () => {
    setShowLogin(false)
    setShowRegister(false)
  }

  return (
    <>
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
          {user ? (
            <div className="user-menu">
              <button
                className="user-menu-button"
                onClick={() => setShowUserMenu(!showUserMenu)}
              >
                <span>👤</span>
                <span>{user.firstName || user.email}</span>
                <span>{showUserMenu ? '▲' : '▼'}</span>
              </button>
              {showUserMenu && (
                <div className="user-menu-dropdown">
                  <div className="user-menu-item" style={{ opacity: 0.7, cursor: 'default' }}>
                    {user.email}
                  </div>
                  <button className="user-menu-item" onClick={handleLogout}>
                    Sign Out
                  </button>
                </div>
              )}
            </div>
          ) : (
            <button className="nav-link" onClick={handleAuthClick}>
              Sign In
            </button>
          )}
          <a className="nav-link small" href="https://learn.microsoft.com/dotnet/" target="_blank" rel="noreferrer">Microsoft .NET</a>
          <a className="nav-link small" href="https://docs.microsoft.com/dotnet/csharp/" target="_blank" rel="noreferrer">C# Docs</a>
        </div>
      </aside>

      {showLogin && (
        <Login onSwitchToRegister={switchToRegister} onClose={closeAuthModals} />
      )}

      {showRegister && (
        <Register onSwitchToLogin={switchToLogin} onClose={closeAuthModals} />
      )}
    </>
  )
}