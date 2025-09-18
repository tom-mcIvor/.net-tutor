import React, { createContext, useContext, useState, useEffect } from 'react'
import type { ReactNode } from 'react'

interface User {
  email: string
  firstName?: string
  lastName?: string
}

interface AuthContextType {
  user: User | null
  token: string | null
  login: (email: string, password: string) => Promise<void>
  register: (
    email: string,
    password: string,
    firstName?: string,
    lastName?: string
  ) => Promise<void>
  confirmSignUp: (email: string, confirmationCode: string) => Promise<void>
  resendConfirmationCode: (email: string) => Promise<void>
  loginWithGoogle: () => Promise<void>
  handleGoogleCallback: (code: string, state?: string) => Promise<void>
  logout: () => void
  isLoading: boolean
  requiresVerification: boolean
  verificationEmail: string | null
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export const useAuth = () => {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}

interface AuthProviderProps {
  children: ReactNode
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null)
  const [token, setToken] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)
  const [requiresVerification, setRequiresVerification] = useState(false)
  const [verificationEmail, setVerificationEmail] = useState<string | null>(
    null
  )

  useEffect(() => {
    // Check for existing token on app load
    const savedToken = localStorage.getItem('authToken')
    const savedUser = localStorage.getItem('authUser')

    console.log('AuthContext: Checking saved auth data on load')
    console.log('AuthContext: Saved token exists:', !!savedToken)
    console.log('AuthContext: Saved user exists:', !!savedUser)

    if (savedToken && savedUser) {
      try {
        const userData = JSON.parse(savedUser)
        setToken(savedToken)
        setUser(userData)
        console.log('AuthContext: Restored user data:', userData)
      } catch (error) {
        console.error('AuthContext: Error parsing saved user data:', error)
        localStorage.removeItem('authToken')
        localStorage.removeItem('authUser')
      }
    }
    setIsLoading(false)
  }, [])

  const login = async (email: string, password: string) => {
    setIsLoading(true)
    setRequiresVerification(false)
    setVerificationEmail(null)

    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      })

      if (!response.ok) {
        const errorData = await response.text()

        // Check if the error is about user not being confirmed
        if (
          errorData.includes('User is not confirmed') ||
          errorData.includes('Please verify your email')
        ) {
          setRequiresVerification(true)
          setVerificationEmail(email)
          throw new Error('Please verify your email address before signing in.')
        }

        throw new Error(errorData || 'Login failed')
      }

      const data = await response.json()

      setToken(data.token)
      const userData = {
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
      }
      setUser(userData)

      localStorage.setItem('authToken', data.token)
      localStorage.setItem('authUser', JSON.stringify(userData))
    } catch (error) {
      console.error('Login error:', error)
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const register = async (
    email: string,
    password: string,
    firstName?: string,
    lastName?: string
  ) => {
    setIsLoading(true)
    setRequiresVerification(false)
    setVerificationEmail(null)

    try {
      const response = await fetch('/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password, firstName, lastName }),
      })

      if (!response.ok) {
        const errorData = await response.text()
        throw new Error(errorData || 'Registration failed')
      }

      const data = await response.json()

      // If registration requires verification, set the verification state
      if (data.requiresVerification) {
        setRequiresVerification(true)
        setVerificationEmail(email)
        return
      }

      setToken(data.token)
      const userData = {
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
      }
      setUser(userData)

      localStorage.setItem('authToken', data.token)
      localStorage.setItem('authUser', JSON.stringify(userData))
    } catch (error) {
      console.error('Registration error:', error)
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const confirmSignUp = async (email: string, confirmationCode: string) => {
    setIsLoading(true)
    try {
      const response = await fetch('/api/auth/confirm-signup', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, confirmationCode }),
      })

      if (!response.ok) {
        const errorData = await response.text()
        throw new Error(errorData || 'Verification failed')
      }

      // After successful verification, clear the verification state
      setRequiresVerification(false)
      setVerificationEmail(null)
    } catch (error) {
      console.error('Confirmation error:', error)
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const resendConfirmationCode = async (email: string) => {
    setIsLoading(true)
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
        throw new Error(errorData || 'Failed to resend confirmation code')
      }
    } catch (error) {
      console.error('Resend confirmation error:', error)
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const loginWithGoogle = async () => {
    setIsLoading(true)
    try {
      const redirectUri = `${window.location.origin}/`
      const response = await fetch(
        `/api/auth/google-auth-url?redirectUri=${encodeURIComponent(
          redirectUri
        )}`
      )

      if (!response.ok) {
        throw new Error('Failed to get Google auth URL')
      }

      const data = await response.json()
      window.location.href = data.authUrl
    } catch (error) {
      console.error('Google login error:', error)
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const handleGoogleCallback = async (code: string, state?: string) => {
    setIsLoading(true)
    try {
      const redirectUri = `${window.location.origin}/`
      console.log('=== GOOGLE OAUTH CALLBACK DEBUG START ===')
      console.log('Google OAuth: Sending request with redirectUri:', redirectUri)
      console.log('Google OAuth: Authorization code length:', code?.length || 0)
      console.log('Google OAuth: Authorization code prefix:', code?.substring(0, 20) + '...')
      console.log('Google OAuth: State:', state || 'null')
      console.log('Google OAuth: Current URL:', window.location.href)

      const requestBody = {
        code,
        redirectUri,
        state,
      }
      console.log('Google OAuth: Request body:', requestBody)

      // Make single OAuth request (no retries - OAuth codes are single-use)
      console.log('Google OAuth: Making single request to backend')
      
      const response = await fetch('/api/auth/google-oauth', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
      })

      console.log('Google OAuth: Response status:', response.status)
      console.log('Google OAuth: Response headers:', Object.fromEntries(response.headers.entries()))

      const responseText = await response.text()
      console.log('Google OAuth: Raw response:', responseText)

      if (!response.ok) {
        console.error('=== GOOGLE OAUTH CALLBACK ERROR ===')
        console.error('Status:', response.status)
        console.error('Response:', responseText)
        
        // Try to parse as JSON to get more details
        try {
          const errorJson = JSON.parse(responseText)
          console.error('Parsed error:', errorJson)
          throw new Error(errorJson.message || responseText || 'Google OAuth failed')
        } catch {
          console.error('Could not parse error response as JSON')
          throw new Error(responseText || 'Google OAuth failed')
        }
      }

      let data
      try {
        data = JSON.parse(responseText)
        console.log('Google OAuth: Parsed success response:', data)
      } catch {
        console.error('Could not parse success response as JSON:', responseText)
        throw new Error('Invalid response format from server')
      }

      if (!data.token || !data.email) {
        console.error('Google OAuth: Missing required fields in response:', data)
        throw new Error('Invalid response: missing token or email')
      }

      setToken(data.token)
      const userData = {
        email: data.email,
        firstName: data.firstName || '',
        lastName: data.lastName || '',
      }
      setUser(userData)

      localStorage.setItem('authToken', data.token)
      localStorage.setItem('authUser', JSON.stringify(userData))

      console.log('Google OAuth: User data set successfully:', userData)
      console.log('=== GOOGLE OAUTH CALLBACK DEBUG END - SUCCESS ===')
    } catch (error) {
      console.error('=== GOOGLE OAUTH CALLBACK DEBUG END - ERROR ===')
      console.error('Google OAuth callback error:', error)
      console.error('Error type:', typeof error)
      console.error('Error message:', error instanceof Error ? error.message : String(error))
      throw error
    } finally {
      setIsLoading(false)
    }
  }

  const logout = () => {
    setUser(null)
    setToken(null)
    setRequiresVerification(false)
    setVerificationEmail(null)
    localStorage.removeItem('authToken')
    localStorage.removeItem('authUser')
  }

  const value = {
    user,
    token,
    login,
    register,
    confirmSignUp,
    resendConfirmationCode,
    loginWithGoogle,
    handleGoogleCallback,
    logout,
    isLoading,
    requiresVerification,
    verificationEmail,
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
