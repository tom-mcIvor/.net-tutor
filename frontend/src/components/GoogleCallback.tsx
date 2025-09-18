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
  const [hasProcessed, setHasProcessed] = useState(false)

  useEffect(() => {
    // Prevent multiple simultaneous OAuth processing attempts
    if (hasProcessed) {
      console.log('GoogleCallback: Already processed, skipping')
      return
    }

    const processCallback = async () => {
      try {
        setHasProcessed(true) // Mark as processing to prevent duplicates
        
        const urlParams = new URLSearchParams(window.location.search)
        const code = urlParams.get('code')
        const state = urlParams.get('state')
        const error = urlParams.get('error')

        console.log('GoogleCallback: Processing OAuth callback', {
          code: !!code,
          state,
          error,
          hasProcessed,
        })

        if (error) {
          console.error('GoogleCallback: OAuth error from URL params:', error)
          throw new Error(`Google OAuth error: ${error}`)
        }

        if (!code) {
          console.error('GoogleCallback: No authorization code in URL')
          throw new Error('No authorization code received from Google')
        }

        console.log('GoogleCallback: Calling handleGoogleCallback...')
        await handleGoogleCallback(code, state || undefined)
        console.log('GoogleCallback: OAuth callback completed successfully')

        // Add a small delay to ensure the auth state is fully updated
        setTimeout(() => {
          onSuccess()
        }, 100)
      } catch (err) {
        console.error('GoogleCallback: Error during OAuth processing:', err)
        const errorMessage =
          err instanceof Error ? err.message : 'Google OAuth failed'

        // Add a delay before showing error to avoid race conditions
        setTimeout(() => {
          onError(errorMessage)
        }, 500)
      } finally {
        setIsProcessing(false)
      }
    }

    // Add a small delay before processing to ensure the component is fully mounted
    const timer = setTimeout(processCallback, 100)
    return () => clearTimeout(timer)
  }, [handleGoogleCallback, onSuccess, onError, hasProcessed])

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
