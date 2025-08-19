import React from 'react';
import { useProgress } from '../contexts/ProgressContext';
import { useAuth } from '../contexts/AuthContext';

export const Progress: React.FC = () => {
  const { user } = useAuth();
  const { progress, getCompletionPercentage } = useProgress();

  if (!user) {
    return (
      <div className="progress-container">
        <div className="progress-card">
          <h3>📊 Your Progress</h3>
          <p>Sign in to track your learning progress!</p>
        </div>
      </div>
    );
  }

  const completionPercentage = getCompletionPercentage();
  const formatTime = (minutes: number) => {
    if (minutes < 60) return `${minutes}m`;
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;
    return `${hours}h ${remainingMinutes}m`;
  };

  const formatLastActivity = (date: Date | null) => {
    if (!date) return 'Never';
    const now = new Date();
    const diffInHours = Math.floor((now.getTime() - date.getTime()) / (1000 * 60 * 60));
    
    if (diffInHours < 1) return 'Just now';
    if (diffInHours < 24) return `${diffInHours}h ago`;
    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays === 1) return 'Yesterday';
    if (diffInDays < 7) return `${diffInDays} days ago`;
    return date.toLocaleDateString();
  };

  return (
    <div className="progress-container">
      <div className="progress-card">
        <div className="progress-header">
          <h3>📊 Your Progress</h3>
          <span className="progress-percentage">{completionPercentage}%</span>
        </div>

        <div className="progress-bar-container">
          <div className="progress-bar">
            <div 
              className="progress-bar-fill" 
              style={{ width: `${completionPercentage}%` }}
            />
          </div>
          <span className="progress-text">
            {progress.completedTopics.length} of 6 topics completed
          </span>
        </div>

        <div className="progress-stats">
          <div className="stat-item">
            <span className="stat-icon">📚</span>
            <div className="stat-content">
              <span className="stat-value">{progress.completedLessons.length}</span>
              <span className="stat-label">Lessons Completed</span>
            </div>
          </div>

          <div className="stat-item">
            <span className="stat-icon">⏱️</span>
            <div className="stat-content">
              <span className="stat-value">{formatTime(progress.totalTimeSpent)}</span>
              <span className="stat-label">Time Spent</span>
            </div>
          </div>

          <div className="stat-item">
            <span className="stat-icon">🕒</span>
            <div className="stat-content">
              <span className="stat-value">{formatLastActivity(progress.lastActivity)}</span>
              <span className="stat-label">Last Activity</span>
            </div>
          </div>
        </div>

        {progress.completedTopics.length > 0 && (
          <div className="completed-topics">
            <h4>Completed Topics:</h4>
            <div className="topic-badges">
              {progress.completedTopics.map((topic, index) => (
                <span key={index} className="topic-badge">
                  ✅ {topic}
                </span>
              ))}
            </div>
          </div>
        )}

        {completionPercentage === 100 && (
          <div className="completion-celebration">
            🎉 Congratulations! You've completed all topics!
          </div>
        )}
      </div>
    </div>
  );
};