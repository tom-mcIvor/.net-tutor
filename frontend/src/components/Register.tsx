import React, { useState } from 'react'
import { useAuth } from '../contexts/AuthContext'
import { EmailVerification } from './EmailVerification'

interface RegisterProps {
  onSwitchToLogin: () => void
  onClose: () => void
}

export const Register: React.FC<RegisterProps> = ({
  onSwitchToLogin,
  onClose,
}) => {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [error, setError] = useState('')
  const [showVerification, setShowVerification] = useState(false)
  const {
    register,
    loginWithGoogle,
    isLoading,
    requiresVerification,
    verificationEmail,
  } = useAuth()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')

    try {
      await register(email, password, firstName, lastName)

      // If verification is required, show the verification component
      if (requiresVerification && verificationEmail) {
        setShowVerification(true)
      } else {
        onClose()
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Registration failed')
    }
  }

  const handleVerificationComplete = () => {
    setShowVerification(false)
    onClose()
  }

  const handleBackToRegister = () => {
    setShowVerification(false)
    setError('')
  }

  const handleOverlayClick = () => {
    // Removed click-outside-to-close behavior
    // Modal can only be closed via the X button
  }

  // Show email verification component if needed
  if (showVerification && verificationEmail) {
    return (
      <EmailVerification
        email={verificationEmail}
        onVerificationComplete={handleVerificationComplete}
        onClose={onClose}
        onBackToLogin={handleBackToRegister}
      />
    )
  }

  return (
    <div className="auth-modal" onClick={handleOverlayClick}>
      <div className="auth-modal-content">
        <div className="auth-header">
          <h2>Sign Up</h2>
          <button className="close-btn" onClick={onClose}>
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="auth-form">
          {error && <div className="error-message">{error}</div>}

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="firstName">First Name</label>
              <input
                type="text"
                id="firstName"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                disabled={isLoading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="lastName">Last Name</label>
              <input
                type="text"
                id="lastName"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                disabled={isLoading}
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              disabled={isLoading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              minLength={6}
              disabled={isLoading}
            />
            <small>Password must be at least 6 characters long</small>
          </div>

          <button
            type="submit"
            className="auth-submit-btn"
            disabled={isLoading}
          >
            {isLoading ? 'Creating Account...' : 'Sign Up'}
          </button>
        </form>

        <div className="auth-divider">
          <span>or</span>
        </div>

        <div className="google-auth-section">
          <button
            type="button"
            className="google-signin-btn"
            onClick={async () => {
              try {
                await loginWithGoogle()
                onClose()
              } catch (err) {
                setError(
                  err instanceof Error ? err.message : 'Google sign-up failed'
                )
              }
            }}
            disabled={isLoading}
          >
            <svg
              className="google-icon"
              viewBox="0 0 24 24"
              width="20"
              height="20"
            >
              <path
                fill="#4285F4"
                d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"
              />
              <path
                fill="#34A853"
                d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"
              />
              <path
                fill="#FBBC05"
                d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"
              />
              <path
                fill="#EA4335"
                d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"
              />
            </svg>
            Continue with Google
          </button>
        </div>

        <div className="auth-switch">
          <p>
            Already have an account?{' '}
            <button
              type="button"
              onClick={onSwitchToLogin}
              className="link-btn"
            >
              Sign in
            </button>
          </p>
        </div>
      </div>
    </div>
  )
}
