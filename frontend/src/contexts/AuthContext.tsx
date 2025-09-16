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
      console.log('Google OAuth: Sending request with redirectUri:', redirectUri)
      console.log('Google OAuth: Authorization code:', code.substring(0, 20) + '...')

      const response = await fetch('/api/auth/google-oauth', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          code,
          redirectUri,
          state,
        }),
      })

      console.log('Google OAuth: Response status:', response.status)

      if (!response.ok) {
        const errorData = await response.text()
        console.error('Google OAuth: Error response:', errorData)
        throw new Error(errorData || 'Google OAuth failed')
      }

      const data = await response.json()
      console.log('Google OAuth: Success response:', data)

      setToken(data.token)
      const userData = {
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
      }
      setUser(userData)

      localStorage.setItem('authToken', data.token)
      localStorage.setItem('authUser', JSON.stringify(userData))

      console.log('Google OAuth: User data set:', userData)
    } catch (error) {
      console.error('Google OAuth callback error:', error)
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
