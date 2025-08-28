import { vi, beforeEach, afterEach } from 'vitest'
import { getLessons, getLesson, getAspNetCoreLessons, getAspNetCoreLesson } from '../lessons'
import type { Lesson } from '../../types/lesson'

// Mock the http module
vi.mock('../http', () => ({
  http: vi.fn(),
}))

const mockHttp = vi.mocked(await import('../http')).http

const mockLesson: Lesson = {
  id: 1,
  title: 'Test Lesson',
  description: 'A test lesson',
  content: '# Test Lesson Content',
}

const mockLessons: Lesson[] = [
  mockLesson,
  {
    id: 2,
    title: 'Another Lesson',
    description: 'Another test lesson',
    content: '# Another Test Lesson',
  },
]

describe('Lessons API', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  afterEach(() => {
    vi.restoreAllMocks()
  })

  describe('getLessons', () => {
    it('fetches all lessons successfully', async () => {
      mockHttp.mockResolvedValue(mockLessons)

      const result = await getLessons()

      expect(mockHttp).toHaveBeenCalledWith('/lessons')
      expect(result).toEqual(mockLessons)
    })

    it('handles API errors gracefully', async () => {
      const errorMessage = 'Network error'
      mockHttp.mockRejectedValue(new Error(errorMessage))

      await expect(getLessons()).rejects.toThrow(errorMessage)
      expect(mockHttp).toHaveBeenCalledWith('/lessons')
    })
  })

  describe('getLesson', () => {
    it('fetches a single lesson by id successfully', async () => {
      mockHttp.mockResolvedValue(mockLesson)

      const result = await getLesson(1)

      expect(mockHttp).toHaveBeenCalledWith('/lessons/1')
      expect(result).toEqual(mockLesson)
    })

    it('handles different lesson IDs correctly', async () => {
      const lessonId = 42
      const expectedLesson = { ...mockLesson, id: lessonId }
      mockHttp.mockResolvedValue(expectedLesson)

      const result = await getLesson(lessonId)

      expect(mockHttp).toHaveBeenCalledWith('/lessons/42')
      expect(result).toEqual(expectedLesson)
    })

    it('handles not found errors', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 404: Not Found'))

      await expect(getLesson(999)).rejects.toThrow('HTTP 404: Not Found')
      expect(mockHttp).toHaveBeenCalledWith('/lessons/999')
    })
  })

  describe('getAspNetCoreLessons', () => {
    it('fetches ASP.NET Core lessons successfully', async () => {
      const aspNetLessons = mockLessons.map(lesson => ({
        ...lesson,
        title: `ASP.NET Core: ${lesson.title}`,
      }))
      mockHttp.mockResolvedValue(aspNetLessons)

      const result = await getAspNetCoreLessons()

      expect(mockHttp).toHaveBeenCalledWith('/lessons/aspnetcore')
      expect(result).toEqual(aspNetLessons)
    })

    it('returns empty array when no lessons available', async () => {
      mockHttp.mockResolvedValue([])

      const result = await getAspNetCoreLessons()

      expect(mockHttp).toHaveBeenCalledWith('/lessons/aspnetcore')
      expect(result).toEqual([])
    })
  })

  describe('getAspNetCoreLesson', () => {
    it('fetches a single ASP.NET Core lesson by id successfully', async () => {
      const aspNetLesson = {
        ...mockLesson,
        title: 'ASP.NET Core: MVC Fundamentals',
      }
      mockHttp.mockResolvedValue(aspNetLesson)

      const result = await getAspNetCoreLesson(10)

      expect(mockHttp).toHaveBeenCalledWith('/lessons/aspnetcore/10')
      expect(result).toEqual(aspNetLesson)
    })

    it('handles server errors gracefully', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 500: Internal Server Error'))

      await expect(getAspNetCoreLesson(10)).rejects.toThrow('HTTP 500: Internal Server Error')
      expect(mockHttp).toHaveBeenCalledWith('/lessons/aspnetcore/10')
    })
  })

  describe('API Integration Scenarios', () => {
    it('handles network timeouts', async () => {
      mockHttp.mockRejectedValue(new Error('Fetch timeout'))

      await expect(getLessons()).rejects.toThrow('Fetch timeout')
    })

    it('handles invalid JSON responses', async () => {
      mockHttp.mockRejectedValue(new Error('Invalid JSON response'))

      await expect(getLesson(1)).rejects.toThrow('Invalid JSON response')
    })

    it('makes concurrent requests correctly', async () => {
      // Set up mocks before creating promises
      mockHttp
        .mockResolvedValueOnce({ ...mockLesson, id: 1 })
        .mockResolvedValueOnce({ ...mockLesson, id: 2 })
        .mockResolvedValueOnce(mockLessons)

      const lesson1Promise = getLesson(1)
      const lesson2Promise = getLesson(2)
      const lessonsPromise = getLessons()

      const results = await Promise.all([lesson1Promise, lesson2Promise, lessonsPromise])

      expect(results[0]).toEqual({ ...mockLesson, id: 1 })
      expect(results[1]).toEqual({ ...mockLesson, id: 2 })
      expect(results[2]).toEqual(mockLessons)
      expect(mockHttp).toHaveBeenCalledTimes(3)
    })

    it('handles mixed success and failure responses', async () => {
      // Set up mocks before creating promises
      mockHttp
        .mockResolvedValueOnce(mockLesson)
        .mockRejectedValueOnce(new Error('Not found'))

      const successPromise = getLesson(1)
      const failurePromise = getLesson(999)

      await expect(successPromise).resolves.toEqual(mockLesson)
      await expect(failurePromise).rejects.toThrow('Not found')
    })
  })

  describe('Type Safety', () => {
    it('returns properly typed lesson objects', async () => {
      mockHttp.mockResolvedValue(mockLesson)

      const result = await getLesson(1)

      // TypeScript compiler will catch if these properties don't exist
      expect(typeof result.id).toBe('number')
      expect(typeof result.title).toBe('string')
      expect(typeof result.description).toBe('string')
      expect(typeof result.content).toBe('string')
    })

    it('returns properly typed lesson arrays', async () => {
      mockHttp.mockResolvedValue(mockLessons)

      const result = await getLessons()

      expect(Array.isArray(result)).toBe(true)
      expect(result.length).toBeGreaterThan(0)
      expect(typeof result[0].id).toBe('number')
      expect(typeof result[0].title).toBe('string')
    })
  })
})