import React, { useEffect, useState } from 'react'
import { useAuth } from '../contexts/AuthContext'

interface GoogleCallbackProps {
  onSuccess: () => void
  onError: (error: string) => void
}

export const GoogleCallback: React.FC<GoogleCallbackProps> = ({
  onSuccess,
  onError,
}) => {
  const { handleGoogleCallback } = useAuth()
  const [isProcessing, setIsProcessing] = useState(true)

  useEffect(() => {
    const processCallback = async () => {
      try {
        const urlParams = new URLSearchParams(window.location.search)
        const code = urlParams.get('code')
        const state = urlParams.get('state')
        const error = urlParams.get('error')

        if (error) {
          throw new Error(`Google OAuth error: ${error}`)
        }

        if (!code) {
          throw new Error('No authorization code received from Google')
        }

        await handleGoogleCallback(code, state || undefined)
        onSuccess()
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : 'Google OAuth failed'
        onError(errorMessage)
      } finally {
        setIsProcessing(false)
      }
    }

    processCallback()
  }, [handleGoogleCallback, onSuccess, onError])

  if (isProcessing) {
    return (
      <div className="auth-modal">
        <div className="auth-modal-content">
          <div className="auth-header">
            <h2>Completing Google Sign-in</h2>
          </div>
          <div className="auth-form">
            <div style={{ textAlign: 'center', padding: '2rem' }}>
              <div className="loading-spinner"></div>
              <p>Please wait while we complete your sign-in...</p>
            </div>
          </div>
        </div>
      </div>
    )
  }

  return null
}