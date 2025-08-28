import { render, screen, fireEvent } from '@testing-library/react'
import { vi } from 'vitest'
import { ThemeToggle } from '../ThemeToggle'
import { ThemeProvider, useTheme } from '../../contexts/ThemeContext'

// Mock localStorage
const mockLocalStorage = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
}

Object.defineProperty(window, 'localStorage', {
  value: mockLocalStorage,
})

const ThemeToggleWrapper = ({ children }: { children: React.ReactNode }) => {
  return <ThemeProvider>{children}</ThemeProvider>
}

describe('ThemeToggle', () => {
  beforeEach(() => {
    mockLocalStorage.getItem.mockReturnValue('dark')
    mockLocalStorage.setItem.mockClear()
    mockLocalStorage.getItem.mockClear()
  })

  it('renders with dark theme icon initially', () => {
    render(
      <ThemeToggleWrapper>
        <ThemeToggle />
      </ThemeToggleWrapper>
    )
    
    const button = screen.getByRole('button')
    expect(button).toBeInTheDocument()
    expect(button).toHaveTextContent('☀️') // Shows sun when dark mode is active
  })

  it('has correct accessibility attributes', () => {
    render(
      <ThemeToggleWrapper>
        <ThemeToggle />
      </ThemeToggleWrapper>
    )
    
    const button = screen.getByRole('button')
    expect(button).toHaveAttribute('aria-label', 'Switch to light mode')
    expect(button).toHaveAttribute('title', 'Switch to light mode')
    expect(button).toHaveClass('theme-toggle')
  })

  it('toggles theme when clicked', () => {
    render(
      <ThemeToggleWrapper>
        <ThemeToggle />
      </ThemeToggleWrapper>
    )
    
    const button = screen.getByRole('button')
    
    // Initial state (dark theme)
    expect(button).toHaveTextContent('☀️')
    expect(button).toHaveAttribute('aria-label', 'Switch to light mode')
    
    // Click to toggle to light theme
    fireEvent.click(button)
    
    // Should now show moon icon and update aria-label
    expect(button).toHaveTextContent('🌙')
    expect(button).toHaveAttribute('aria-label', 'Switch to dark mode')
    expect(button).toHaveAttribute('title', 'Switch to dark mode')
  })

  it('toggles theme multiple times correctly', () => {
    render(
      <ThemeToggleWrapper>
        <ThemeToggle />
      </ThemeToggleWrapper>
    )
    
    const button = screen.getByRole('button')
    
    // Initial: dark theme (shows sun)
    expect(button).toHaveTextContent('☀️')
    
    // First click: switch to light (shows moon)
    fireEvent.click(button)
    expect(button).toHaveTextContent('🌙')
    
    // Second click: switch back to dark (shows sun)
    fireEvent.click(button)
    expect(button).toHaveTextContent('☀️')
    
    // Third click: switch to light again (shows moon)
    fireEvent.click(button)
    expect(button).toHaveTextContent('🌙')
  })

  it('starts with light theme when localStorage has light', () => {
    mockLocalStorage.getItem.mockReturnValue('light')
    
    render(
      <ThemeToggleWrapper>
        <ThemeToggle />
      </ThemeToggleWrapper>
    )
    
    const button = screen.getByRole('button')
    expect(button).toHaveTextContent('🌙') // Shows moon when light mode is active
    expect(button).toHaveAttribute('aria-label', 'Switch to dark mode')
  })
})

// Test the ThemeProvider separately
describe('ThemeProvider', () => {
  beforeEach(() => {
    mockLocalStorage.getItem.mockClear()
    mockLocalStorage.setItem.mockClear()
    // Clear any attributes from previous tests
    document.documentElement.removeAttribute('data-theme')
  })

  it('provides theme context to children', () => {
    mockLocalStorage.getItem.mockReturnValue('dark')
    
    const TestComponent = () => {
      const { theme, toggleTheme } = useTheme()
      return (
        <div>
          <span data-testid="current-theme">{theme}</span>
          <button onClick={toggleTheme} data-testid="toggle-button">
            Toggle
          </button>
        </div>
      )
    }

    render(
      <ThemeProvider>
        <TestComponent />
      </ThemeProvider>
    )

    expect(screen.getByTestId('current-theme')).toHaveTextContent('dark')
  })

  it('sets data-theme attribute on document root', () => {
    mockLocalStorage.getItem.mockReturnValue('light')
    
    render(
      <ThemeProvider>
        <div>Test</div>
      </ThemeProvider>
    )

    expect(document.documentElement).toHaveAttribute('data-theme', 'light')
  })

  it('saves theme to localStorage when changed', () => {
    mockLocalStorage.getItem.mockReturnValue('dark')
    
    const TestComponent = () => {
      const { toggleTheme } = useTheme()
      return <button onClick={toggleTheme} data-testid="toggle">Toggle</button>
    }

    render(
      <ThemeProvider>
        <TestComponent />
      </ThemeProvider>
    )

    fireEvent.click(screen.getByTestId('toggle'))

    expect(mockLocalStorage.setItem).toHaveBeenCalledWith('theme', 'light')
  })

  it('defaults to dark theme when no localStorage value', () => {
    mockLocalStorage.getItem.mockReturnValue(null)
    
    const TestComponent = () => {
      const { theme } = useTheme()
      return <span data-testid="theme">{theme}</span>
    }

    render(
      <ThemeProvider>
        <TestComponent />
      </ThemeProvider>
    )

    expect(screen.getByTestId('theme')).toHaveTextContent('dark')
  })
})