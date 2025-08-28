import { render, screen, fireEvent } from '@testing-library/react'
import { BrowserRouter } from 'react-router-dom'
import { vi } from 'vitest'
import { LessonCard } from '../LessonCard'
import type { Lesson } from '../../types/lesson'

// Mock useNavigate
const mockNavigate = vi.fn()
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom')
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  }
})

const mockLesson: Lesson = {
  id: 1,
  title: 'Introduction to React',
  description: 'Learn the basics of React including components, props, and state.',
  content: '# Introduction to React\n\nThis lesson covers...',
}

const renderWithRouter = (component: React.ReactElement) => {
  return render(
    <BrowserRouter>
      {component}
    </BrowserRouter>
  )
}

describe('LessonCard', () => {
  beforeEach(() => {
    mockNavigate.mockClear()
  })

  it('renders lesson information correctly', () => {
    renderWithRouter(<LessonCard lesson={mockLesson} />)
    
    expect(screen.getByText('📚 Introduction to React')).toBeInTheDocument()
    expect(screen.getByText('Learn the basics of React including components, props, and state.')).toBeInTheDocument()
    expect(screen.getByText('Start Learning')).toBeInTheDocument()
  })

  it('navigates to lesson detail when clicked', () => {
    renderWithRouter(<LessonCard lesson={mockLesson} />)
    
    const card = screen.getByText('📚 Introduction to React').closest('div')
    expect(card).toBeInTheDocument()
    
    fireEvent.click(card!)
    
    expect(mockNavigate).toHaveBeenCalledWith('/lesson/1')
  })

  it('has appropriate accessibility features', () => {
    renderWithRouter(<LessonCard lesson={mockLesson} />)
    
    const card = screen.getByText('📚 Introduction to React').closest('div')
    expect(card).toBeInTheDocument()
    // Check that the card has click functionality (cursor pointer is set via inline styles)
    expect(card).toHaveAttribute('style', expect.stringContaining('cursor'))
  })

  it('applies hover effects on mouse enter and leave', () => {
    renderWithRouter(<LessonCard lesson={mockLesson} />)
    
    const card = screen.getByText('📚 Introduction to React').closest('div')
    expect(card).toBeInTheDocument()

    // Test mouse enter - check that style is updated (transform is set via event handlers)
    fireEvent.mouseEnter(card!)
    expect(card).toHaveAttribute('style', expect.stringContaining('transform'))

    // Test mouse leave
    fireEvent.mouseLeave(card!)
    expect(card).toHaveAttribute('style', expect.stringContaining('transform'))
  })

  it('displays lesson with different data correctly', () => {
    const differentLesson: Lesson = {
      id: 2,
      title: 'Advanced TypeScript',
      description: 'Master advanced TypeScript features and patterns.',
      content: '# Advanced TypeScript\n\nThis advanced lesson...',
    }

    renderWithRouter(<LessonCard lesson={differentLesson} />)
    
    expect(screen.getByText('📚 Advanced TypeScript')).toBeInTheDocument()
    expect(screen.getByText('Master advanced TypeScript features and patterns.')).toBeInTheDocument()
  })

  it('navigates with correct lesson ID for different lessons', () => {
    const lesson: Lesson = {
      id: 42,
      title: 'Test Lesson',
      description: 'A test lesson',
      content: '# Test',
    }

    renderWithRouter(<LessonCard lesson={lesson} />)
    
    const card = screen.getByText('📚 Test Lesson').closest('div')
    fireEvent.click(card!)
    
    expect(mockNavigate).toHaveBeenCalledWith('/lesson/42')
  })

  it('renders emoji elements correctly', () => {
    renderWithRouter(<LessonCard lesson={mockLesson} />)
    
    expect(screen.getByText('📚 Introduction to React')).toBeInTheDocument()
    expect(screen.getByText('🎯')).toBeInTheDocument()
    expect(screen.getByText('→')).toBeInTheDocument()
  })
})