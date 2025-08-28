import { vi, beforeEach, afterEach } from 'vitest'
import { submitFeedback, getFeedback, getAllFeedback } from '../feedback'
import type { Feedback, CreateFeedbackDto } from '../../types/feedback'

// Mock the http module
vi.mock('../http', () => ({
  http: vi.fn(),
}))

const mockHttp = vi.mocked(await import('../http')).http

const mockFeedback: Feedback = {
  id: 1,
  message: 'Great lesson!',
  userId: 'user123',
  userEmail: 'user@example.com',
  createdAt: '2023-12-01T10:00:00Z',
  pageContext: '/lessons/1',
}

const mockCreateFeedbackDto: CreateFeedbackDto = {
  message: 'Great lesson!',
  pageContext: '/lessons/1',
}

const mockFeedbackList: Feedback[] = [
  mockFeedback,
  {
    id: 2,
    message: 'Could use more examples',
    userId: 'user456',
    userEmail: 'another@example.com',
    createdAt: '2023-12-01T11:00:00Z',
    pageContext: '/lessons/2',
  },
]

describe('Feedback API', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  afterEach(() => {
    vi.restoreAllMocks()
  })

  describe('submitFeedback', () => {
    it('submits feedback successfully', async () => {
      mockHttp.mockResolvedValue(mockFeedback)

      const result = await submitFeedback(mockCreateFeedbackDto)

      expect(mockHttp).toHaveBeenCalledWith('/feedback', {
        method: 'POST',
        body: JSON.stringify(mockCreateFeedbackDto),
      })
      expect(result).toEqual(mockFeedback)
    })

    it('handles feedback submission without page context', async () => {
      const feedbackWithoutContext: CreateFeedbackDto = {
        message: 'General feedback',
      }
      const expectedResponse: Feedback = {
        ...mockFeedback,
        message: 'General feedback',
        pageContext: undefined,
      }

      mockHttp.mockResolvedValue(expectedResponse)

      const result = await submitFeedback(feedbackWithoutContext)

      expect(mockHttp).toHaveBeenCalledWith('/feedback', {
        method: 'POST',
        body: JSON.stringify(feedbackWithoutContext),
      })
      expect(result).toEqual(expectedResponse)
    })

    it('handles submission errors gracefully', async () => {
      const errorMessage = 'Validation failed'
      mockHttp.mockRejectedValue(new Error(`HTTP 400: ${errorMessage}`))

      await expect(submitFeedback(mockCreateFeedbackDto)).rejects.toThrow(`HTTP 400: ${errorMessage}`)
      expect(mockHttp).toHaveBeenCalledWith('/feedback', {
        method: 'POST',
        body: JSON.stringify(mockCreateFeedbackDto),
      })
    })

    it('handles empty message submission', async () => {
      const emptyFeedback: CreateFeedbackDto = {
        message: '',
        pageContext: '/lessons/1',
      }
      const response: Feedback = {
        ...mockFeedback,
        message: '',
      }

      mockHttp.mockResolvedValue(response)

      const result = await submitFeedback(emptyFeedback)

      expect(result.message).toBe('')
      expect(mockHttp).toHaveBeenCalledWith('/feedback', {
        method: 'POST',
        body: JSON.stringify(emptyFeedback),
      })
    })
  })

  describe('getFeedback', () => {
    it('fetches feedback by ID successfully', async () => {
      mockHttp.mockResolvedValue(mockFeedback)

      const result = await getFeedback(1)

      expect(mockHttp).toHaveBeenCalledWith('/feedback/1')
      expect(result).toEqual(mockFeedback)
    })

    it('handles different feedback IDs correctly', async () => {
      const feedbackId = 42
      const expectedFeedback = { ...mockFeedback, id: feedbackId }
      mockHttp.mockResolvedValue(expectedFeedback)

      const result = await getFeedback(feedbackId)

      expect(mockHttp).toHaveBeenCalledWith('/feedback/42')
      expect(result).toEqual(expectedFeedback)
    })

    it('handles not found errors', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 404: Not Found'))

      await expect(getFeedback(999)).rejects.toThrow('HTTP 404: Not Found')
      expect(mockHttp).toHaveBeenCalledWith('/feedback/999')
    })
  })

  describe('getAllFeedback', () => {
    it('fetches all feedback successfully', async () => {
      mockHttp.mockResolvedValue(mockFeedbackList)

      const result = await getAllFeedback()

      expect(mockHttp).toHaveBeenCalledWith('/feedback')
      expect(result).toEqual(mockFeedbackList)
    })

    it('returns empty array when no feedback exists', async () => {
      mockHttp.mockResolvedValue([])

      const result = await getAllFeedback()

      expect(mockHttp).toHaveBeenCalledWith('/feedback')
      expect(result).toEqual([])
    })

    it('handles server errors gracefully', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 500: Internal Server Error'))

      await expect(getAllFeedback()).rejects.toThrow('HTTP 500: Internal Server Error')
      expect(mockHttp).toHaveBeenCalledWith('/feedback')
    })
  })

  describe('API Integration Scenarios', () => {
    it('handles concurrent feedback operations', async () => {
      const submitPromise = submitFeedback(mockCreateFeedbackDto)
      const getPromise = getFeedback(1)
      const getAllPromise = getAllFeedback()

      mockHttp
        .mockResolvedValueOnce(mockFeedback) // for submit
        .mockResolvedValueOnce(mockFeedback) // for get
        .mockResolvedValueOnce(mockFeedbackList) // for getAll

      const results = await Promise.all([submitPromise, getPromise, getAllPromise])

      expect(results[0]).toEqual(mockFeedback)
      expect(results[1]).toEqual(mockFeedback)
      expect(results[2]).toEqual(mockFeedbackList)
      expect(mockHttp).toHaveBeenCalledTimes(3)
    })

    it('handles network errors', async () => {
      mockHttp.mockRejectedValue(new Error('Network error'))

      await expect(submitFeedback(mockCreateFeedbackDto)).rejects.toThrow('Network error')
      await expect(getFeedback(1)).rejects.toThrow('Network error')
      await expect(getAllFeedback()).rejects.toThrow('Network error')
    })

    it('handles rate limiting errors', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 429: Too Many Requests'))

      await expect(submitFeedback(mockCreateFeedbackDto)).rejects.toThrow('HTTP 429: Too Many Requests')
    })

    it('handles unauthorized access', async () => {
      mockHttp.mockRejectedValue(new Error('HTTP 401: Unauthorized'))

      await expect(getAllFeedback()).rejects.toThrow('HTTP 401: Unauthorized')
    })
  })

  describe('Data Validation', () => {
    it('properly serializes feedback data for submission', async () => {
      const feedbackData: CreateFeedbackDto = {
        message: 'Test feedback with special characters: áéíóú 你好 🎉',
        pageContext: '/lessons/special-chars',
      }

      mockHttp.mockResolvedValue(mockFeedback)

      await submitFeedback(feedbackData)

      expect(mockHttp).toHaveBeenCalledWith('/feedback', {
        method: 'POST',
        body: JSON.stringify(feedbackData),
      })
    })

    it('handles feedback with long messages', async () => {
      const longMessage = 'A'.repeat(5000)
      const longFeedback: CreateFeedbackDto = {
        message: longMessage,
        pageContext: '/lessons/1',
      }

      const response: Feedback = {
        ...mockFeedback,
        message: longMessage,
      }

      mockHttp.mockResolvedValue(response)

      const result = await submitFeedback(longFeedback)

      expect(result.message).toBe(longMessage)
      expect(result.message.length).toBe(5000)
    })
  })

  describe('Type Safety', () => {
    it('returns properly typed feedback objects', async () => {
      mockHttp.mockResolvedValue(mockFeedback)

      const result = await getFeedback(1)

      // TypeScript compiler will catch if these properties don't exist or have wrong types
      expect(typeof result.id).toBe('number')
      expect(typeof result.message).toBe('string')
      expect(typeof result.createdAt).toBe('string')
      expect(result.userId === undefined || typeof result.userId === 'string').toBe(true)
      expect(result.userEmail === undefined || typeof result.userEmail === 'string').toBe(true)
      expect(result.pageContext === undefined || typeof result.pageContext === 'string').toBe(true)
    })

    it('returns properly typed feedback arrays', async () => {
      mockHttp.mockResolvedValue(mockFeedbackList)

      const result = await getAllFeedback()

      expect(Array.isArray(result)).toBe(true)
      if (result.length > 0) {
        expect(typeof result[0].id).toBe('number')
        expect(typeof result[0].message).toBe('string')
        expect(typeof result[0].createdAt).toBe('string')
      }
    })
  })
})