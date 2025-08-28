import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { vi } from 'vitest'
import { Login } from '../Login'

// Mock the AuthContext
const mockLogin = vi.fn()
const mockAuthContext = {
  user: null,
  token: null,
  login: mockLogin,
  register: vi.fn(),
  logout: vi.fn(),
  isLoading: false,
}

vi.mock('../../contexts/AuthContext', () => ({
  useAuth: () => mockAuthContext,
}))

const mockProps = {
  onSwitchToRegister: vi.fn(),
  onClose: vi.fn(),
}

describe('Login', () => {
  beforeEach(() => {
    mockLogin.mockClear()
    mockProps.onSwitchToRegister.mockClear()
    mockProps.onClose.mockClear()
    mockAuthContext.isLoading = false
  })

  it('renders login form correctly', () => {
    render(<Login {...mockProps} />)
    
    expect(screen.getByRole('heading', { name: 'Sign In' })).toBeInTheDocument()
    expect(screen.getByLabelText('Email')).toBeInTheDocument()
    expect(screen.getByLabelText('Password')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Sign In' })).toBeInTheDocument()
    expect(screen.getByText("Don't have an account?")).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Sign up' })).toBeInTheDocument()
  })

  it('renders close button', () => {
    render(<Login {...mockProps} />)
    
    const closeButton = screen.getByRole('button', { name: '✕' })
    expect(closeButton).toBeInTheDocument()
  })

  it('calls onClose when close button is clicked', async () => {
    const user = userEvent.setup()
    render(<Login {...mockProps} />)
    
    const closeButton = screen.getByRole('button', { name: '✕' })
    await user.click(closeButton)
    
    expect(mockProps.onClose).toHaveBeenCalledTimes(1)
  })

  it('calls onSwitchToRegister when sign up link is clicked', async () => {
    const user = userEvent.setup()
    render(<Login {...mockProps} />)
    
    const signUpButton = screen.getByRole('button', { name: 'Sign up' })
    await user.click(signUpButton)
    
    expect(mockProps.onSwitchToRegister).toHaveBeenCalledTimes(1)
  })

  it('calls onClose when clicking on overlay', () => {
    render(<Login {...mockProps} />)
    
    const overlay = screen.getByRole('heading', { name: 'Sign In' }).closest('.auth-modal')
    expect(overlay).toBeInTheDocument()
    
    fireEvent.click(overlay!)
    expect(mockProps.onClose).toHaveBeenCalledTimes(1)
  })

  it('does not close when clicking on modal content', () => {
    render(<Login {...mockProps} />)
    
    const modalContent = screen.getByRole('heading', { name: 'Sign In' }).closest('.auth-modal-content')
    expect(modalContent).toBeInTheDocument()
    
    fireEvent.click(modalContent!)
    expect(mockProps.onClose).not.toHaveBeenCalled()
  })

  it('updates email and password fields when typed', async () => {
    const user = userEvent.setup()
    render(<Login {...mockProps} />)
    
    const emailInput = screen.getByLabelText('Email') as HTMLInputElement
    const passwordInput = screen.getByLabelText('Password') as HTMLInputElement
    
    await user.type(emailInput, 'test@example.com')
    await user.type(passwordInput, 'password123')
    
    expect(emailInput.value).toBe('test@example.com')
    expect(passwordInput.value).toBe('password123')
  })

  it('submits form with correct credentials', async () => {
    const user = userEvent.setup()
    mockLogin.mockResolvedValue(undefined)
    
    render(<Login {...mockProps} />)
    
    await user.type(screen.getByLabelText('Email'), 'test@example.com')
    await user.type(screen.getByLabelText('Password'), 'password123')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))
    
    expect(mockLogin).toHaveBeenCalledWith('test@example.com', 'password123')
  })

  it('calls onClose after successful login', async () => {
    const user = userEvent.setup()
    mockLogin.mockResolvedValue(undefined)
    
    render(<Login {...mockProps} />)
    
    await user.type(screen.getByLabelText('Email'), 'test@example.com')
    await user.type(screen.getByLabelText('Password'), 'password123')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))
    
    await waitFor(() => {
      expect(mockProps.onClose).toHaveBeenCalledTimes(1)
    })
  })

  it('displays error message when login fails', async () => {
    const user = userEvent.setup()
    mockLogin.mockRejectedValue(new Error('Invalid credentials'))
    
    render(<Login {...mockProps} />)
    
    await user.type(screen.getByLabelText('Email'), 'test@example.com')
    await user.type(screen.getByLabelText('Password'), 'wrongpassword')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))
    
    await waitFor(() => {
      expect(screen.getByText('Invalid credentials')).toBeInTheDocument()
    })
    
    expect(mockProps.onClose).not.toHaveBeenCalled()
  })

  it('displays generic error message for non-Error exceptions', async () => {
    const user = userEvent.setup()
    mockLogin.mockRejectedValue('Some other error')
    
    render(<Login {...mockProps} />)
    
    await user.type(screen.getByLabelText('Email'), 'test@example.com')
    await user.type(screen.getByLabelText('Password'), 'password123')
    await user.click(screen.getByRole('button', { name: 'Sign In' }))
    
    await waitFor(() => {
      expect(screen.getByText('Login failed')).toBeInTheDocument()
    })
  })

  it('clears error message when form is resubmitted', async () => {
    const user = userEvent.setup()
    mockLogin
      .mockRejectedValueOnce(new Error('First error'))
      .mockResolvedValueOnce(undefined)
    
    render(<Login {...mockProps} />)
    
    const emailInput = screen.getByLabelText('Email')
    const passwordInput = screen.getByLabelText('Password')
    const submitButton = screen.getByRole('button', { name: 'Sign In' })
    
    // First submission - fails
    await user.type(emailInput, 'test@example.com')
    await user.type(passwordInput, 'wrongpassword')
    await user.click(submitButton)
    
    await waitFor(() => {
      expect(screen.getByText('First error')).toBeInTheDocument()
    })
    
    // Clear and retype
    await user.clear(passwordInput)
    await user.type(passwordInput, 'correctpassword')
    await user.click(submitButton)
    
    // Error should be cleared
    await waitFor(() => {
      expect(screen.queryByText('First error')).not.toBeInTheDocument()
    })
  })

  it('shows loading state during login', async () => {
    const user = userEvent.setup()
    mockAuthContext.isLoading = true
    
    render(<Login {...mockProps} />)
    
    expect(screen.getByRole('button', { name: 'Signing In...' })).toBeInTheDocument()
    expect(screen.getByLabelText('Email')).toBeDisabled()
    expect(screen.getByLabelText('Password')).toBeDisabled()
  })

  it('requires email and password fields', () => {
    render(<Login {...mockProps} />)
    
    const emailInput = screen.getByLabelText('Email')
    const passwordInput = screen.getByLabelText('Password')
    
    expect(emailInput).toBeRequired()
    expect(passwordInput).toBeRequired()
  })

  it('has correct input types', () => {
    render(<Login {...mockProps} />)
    
    const emailInput = screen.getByLabelText('Email')
    const passwordInput = screen.getByLabelText('Password')
    
    expect(emailInput).toHaveAttribute('type', 'email')
    expect(passwordInput).toHaveAttribute('type', 'password')
  })
})