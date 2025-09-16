import { useState, useEffect } from 'react'
import './App.css'
import Sidebar from './components/Sidebar'
import { GoogleCallback } from './components/GoogleCallback'
import { useAuth } from './contexts/AuthContext'

const TABS = [
  'Introduction to .NET',
  'C# Basics',
  'ASP.NET Core Overview',
  'Entity Framework Core',
  '.NET Libraries & Tools',
  'Advanced Topics',
  'from node.js to .NET',
]

function App() {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const [activeTab, setActiveTab] = useState(TABS[0])
  const [showGoogleCallback, setShowGoogleCallback] = useState(false)
  const { user, isLoading } = useAuth()

  useEffect(() => {
    // Check if this is a Google OAuth callback and user is not already logged in
    const urlParams = new URLSearchParams(window.location.search)
    const code = urlParams.get('code')

    console.log('App: Checking OAuth callback', {
      code,
      user,
      isLoading,
      showGoogleCallback,
    })

    if (code && !user) {
      console.log('App: Setting showGoogleCallback to true')
      setShowGoogleCallback(true)
    } else if (user && showGoogleCallback) {
      // User is now authenticated, hide the callback and clear URL
      console.log('App: User authenticated, hiding callback')
      setShowGoogleCallback(false)
      window.history.replaceState({}, document.title, window.location.pathname)
    }
  }, [user, isLoading, showGoogleCallback])

  const handleGoogleSuccess = () => {
    setShowGoogleCallback(false)
    // Clear the URL parameters
    window.history.replaceState({}, document.title, window.location.pathname)
  }

  const handleGoogleError = (error: string) => {
    setShowGoogleCallback(false)
    console.error('Google OAuth error:', error)
    // Clear the URL parameters
    window.history.replaceState({}, document.title, window.location.pathname)
    alert('Google sign-in failed: ' + error)
  }

  if (showGoogleCallback) {
    return (
      <GoogleCallback
        onSuccess={handleGoogleSuccess}
        onError={handleGoogleError}
      />
    )
  }

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
              This section contains introductory content for{' '}
              <strong>{activeTab}</strong>. Replace this placeholder with real
              lesson content or link to detailed lesson pages.
            </p>
            <p>
              Current tabs are implemented as an in-app navigation. Clicking a
              tab updates the content area and (on small screens) closes the
              sidebar for a better mobile experience.
            </p>
          </section>
        </main>
      </div>
    </div>
  )
}

export default App
