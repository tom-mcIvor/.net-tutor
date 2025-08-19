import React, { createContext, useContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';
import { useAuth } from './AuthContext';

interface ProgressData {
  completedLessons: string[];
  completedTopics: string[];
  totalTimeSpent: number; // in minutes
  lastActivity: Date | null;
}

interface ProgressContextType {
  progress: ProgressData;
  markLessonComplete: (lessonId: string) => void;
  markTopicComplete: (topicId: string) => void;
  addTimeSpent: (minutes: number) => void;
  getCompletionPercentage: () => number;
  isLessonComplete: (lessonId: string) => boolean;
  isTopicComplete: (topicId: string) => boolean;
}

const ProgressContext = createContext<ProgressContextType | undefined>(undefined);

export const useProgress = () => {
  const context = useContext(ProgressContext);
  if (context === undefined) {
    throw new Error('useProgress must be used within a ProgressProvider');
  }
  return context;
};

interface ProgressProviderProps {
  children: ReactNode;
}

const TOTAL_TOPICS = 6; // Based on our navigation tabs

export const ProgressProvider: React.FC<ProgressProviderProps> = ({ children }) => {
  const { user } = useAuth();
  const [progress, setProgress] = useState<ProgressData>({
    completedLessons: [],
    completedTopics: [],
    totalTimeSpent: 0,
    lastActivity: null,
  });

  // Load progress from localStorage when user changes
  useEffect(() => {
    if (user) {
      const savedProgress = localStorage.getItem(`progress_${user.email}`);
      if (savedProgress) {
        const parsed = JSON.parse(savedProgress);
        setProgress({
          ...parsed,
          lastActivity: parsed.lastActivity ? new Date(parsed.lastActivity) : null,
        });
      }
    } else {
      // Reset progress when user logs out
      setProgress({
        completedLessons: [],
        completedTopics: [],
        totalTimeSpent: 0,
        lastActivity: null,
      });
    }
  }, [user]);

  // Save progress to localStorage whenever it changes
  useEffect(() => {
    if (user) {
      localStorage.setItem(`progress_${user.email}`, JSON.stringify(progress));
    }
  }, [progress, user]);

  const markLessonComplete = (lessonId: string) => {
    setProgress(prev => ({
      ...prev,
      completedLessons: prev.completedLessons.includes(lessonId) 
        ? prev.completedLessons 
        : [...prev.completedLessons, lessonId],
      lastActivity: new Date(),
    }));
  };

  const markTopicComplete = (topicId: string) => {
    setProgress(prev => ({
      ...prev,
      completedTopics: prev.completedTopics.includes(topicId)
        ? prev.completedTopics
        : [...prev.completedTopics, topicId],
      lastActivity: new Date(),
    }));
  };

  const addTimeSpent = (minutes: number) => {
    setProgress(prev => ({
      ...prev,
      totalTimeSpent: prev.totalTimeSpent + minutes,
      lastActivity: new Date(),
    }));
  };

  const getCompletionPercentage = () => {
    return Math.round((progress.completedTopics.length / TOTAL_TOPICS) * 100);
  };

  const isLessonComplete = (lessonId: string) => {
    return progress.completedLessons.includes(lessonId);
  };

  const isTopicComplete = (topicId: string) => {
    return progress.completedTopics.includes(topicId);
  };

  const value = {
    progress,
    markLessonComplete,
    markTopicComplete,
    addTimeSpent,
    getCompletionPercentage,
    isLessonComplete,
    isTopicComplete,
  };

  return <ProgressContext.Provider value={value}>{children}</ProgressContext.Provider>;
};