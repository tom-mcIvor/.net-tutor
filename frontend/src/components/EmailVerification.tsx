import React, { useState } from 'react'

interface EmailVerificationProps {
  email: string
  onVerificationComplete: () => void
  onClose: () => void
  onBackToLogin: () => void
}

export const EmailVerification: React.FC<EmailVerificationProps> = ({
  email,
  onVerificationComplete,
  onClose,
  onBackToLogin,
}) => {
  const [verificationCode, setVerificationCode] = useState('')
  const [error, setError] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [isResending, setIsResending] = useState(false)
  const [resendMessage, setResendMessage] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setIsLoading(true)

    try {
      const response = await fetch('/api/auth/confirm-signup', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email,
          confirmationCode: verificationCode,
        }),
      })

      if (!response.ok) {
        const errorData = await response.text()
        throw new Error(errorData || 'Verification failed')
      }

      onVerificationComplete()
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Verification failed')
    } finally {
      setIsLoading(false)
    }
  }

  const handleResendCode = async () => {
    setIsResending(true)
    setError('')
    setResendMessage('')

    try {
      const response = await fetch('/api/auth/resend-confirmation', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
      })

      if (!response.ok) {
        const errorData = await response.text()
        throw new Error(errorData || 'Failed to resend code')
      }

      setResendMessage('Verification code sent! Check your email.')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to resend code')
    } finally {
      setIsResending(false)
    }
  }

  const handleOverlayClick = (e: React.MouseEvent) => {
    if (e.target === e.currentTarget) {
      onClose()
    }
  }

  return (
    <div className="auth-modal" onClick={handleOverlayClick}>
      <div className="auth-modal-content">
        <div className="auth-header">
          <h2>Verify Your Email</h2>
          <button className="close-btn" onClick={onClose}>
            ✕
          </button>
        </div>

        <div className="verification-info">
          <p>
            We've sent a verification code to <strong>{email}</strong>
          </p>
          <p>Please enter the code below to verify your account.</p>
        </div>

        <form onSubmit={handleSubmit} className="auth-form">
          {error && <div className="error-message">{error}</div>}
          {resendMessage && <div className="success-message">{resendMessage}</div>}

          <div className="form-group">
            <label htmlFor="verificationCode">Verification Code</label>
            <input
              type="text"
              id="verificationCode"
              value={verificationCode}
              onChange={(e) => setVerificationCode(e.target.value)}
              placeholder="Enter 6-digit code"
              maxLength={6}
              required
              disabled={isLoading}
              style={{ textAlign: 'center', fontSize: '1.2em', letterSpacing: '0.2em' }}
            />
          </div>

          <button
            type="submit"
            className="auth-submit-btn"
            disabled={isLoading || verificationCode.length !== 6}
          >
            {isLoading ? 'Verifying...' : 'Verify Email'}
          </button>
        </form>

        <div className="verification-actions">
          <p>
            Didn't receive the code?{' '}
            <button
              type="button"
              onClick={handleResendCode}
              className="link-btn"
              disabled={isResending}
            >
              {isResending ? 'Sending...' : 'Resend code'}
            </button>
          </p>
          
          <p>
            <button
              type="button"
              onClick={onBackToLogin}
              className="link-btn"
            >
              ← Back to login
            </button>
          </p>
        </div>
      </div>
    </div>
  )
}