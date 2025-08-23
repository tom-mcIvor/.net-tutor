import { useAuth } from "../contexts/AuthContext";
import { useProgress } from "../contexts/ProgressContext";
import { useState, useEffect } from "react";
import { getLessons } from "../api/lessons";
import type { Lesson } from "../types/lesson";
import Sidebar from "../components/Sidebar";
import "../App.css";

const TABS = [
  'Introduction to .NET',
  'C# Basics',
  'ASP.NET Core Overview',
  'Entity Framework Core',
  '.NET Libraries & Tools',
  'Advanced Topics',
  'from node.js to .NET'
];

export default function Profile() {
  const { user, logout } = useAuth();
  const { progress, getCompletionPercentage } = useProgress();
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLessons = async () => {
      try {
        const data = await getLessons();
        setLessons(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load lessons');
      } finally {
        setLoading(false);
      }
    };

    fetchLessons();
  }, []);

  if (!user) {
    return (
      <div className="layout">
        <Sidebar
          tabs={TABS}
          activeTab="Profile"
          setActiveTab={() => {}}
          sidebarOpen={sidebarOpen}
          setSidebarOpen={setSidebarOpen}
        />
        <div className="content">
          <header className="topbar">
            <button
              className="hamburger"
              aria-label="Open sidebar"
              onClick={() => setSidebarOpen(true)}
            >
              ☰
            </button>
            <h1 className="page-title">Profile</h1>
          </header>
          <main className="main">
            <div className="card" style={{ textAlign: 'center', padding: '40px' }}>
              <h2>👤 Profile</h2>
              <p>Please sign in to view your profile and progress.</p>
            </div>
          </main>
        </div>
      </div>
    );
  }

  const completedLessonsData = lessons.filter(lesson =>
    progress.completedLessons.includes(lesson.id.toString())
  );

  const progressPercentage = getCompletionPercentage();

  return (
    <div className="layout">
      <Sidebar
        tabs={TABS}
        activeTab="Profile"
        setActiveTab={() => {}}
        sidebarOpen={sidebarOpen}
        setSidebarOpen={setSidebarOpen}
      />

      <div className="content">
        <header className="topbar">
          <button
            className="hamburger"
            aria-label="Open sidebar"
            onClick={() => setSidebarOpen(true)}
          >
            ☰
          </button>
          <h1 className="page-title">Profile</h1>
          
          {/* User Section */}
          <div style={{ marginLeft: 'auto', display: 'flex', alignItems: 'center', gap: '12px' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
              <span style={{
                fontSize: '14px',
                color: '#9ca3af',
                display: 'flex',
                alignItems: 'center',
                gap: '6px'
              }}>
                👤 {user.email}
              </span>
              <button
                onClick={logout}
                style={{
                  padding: '8px 16px',
                  background: 'rgba(239, 68, 68, 0.1)',
                  color: '#f87171',
                  border: '1px solid rgba(239, 68, 68, 0.3)',
                  borderRadius: '6px',
                  cursor: 'pointer',
                  fontSize: '14px',
                  fontWeight: '500',
                  transition: 'all 0.2s ease'
                }}
                onMouseOver={(e) => {
                  (e.target as HTMLButtonElement).style.background = 'rgba(239, 68, 68, 0.2)';
                }}
                onMouseOut={(e) => {
                  (e.target as HTMLButtonElement).style.background = 'rgba(239, 68, 68, 0.1)';
                }}
              >
                Sign Out
              </button>
            </div>
          </div>
        </header>

        <main className="main">
          {/* Profile Header */}
          <div className="card" style={{ marginBottom: 24 }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '20px', marginBottom: 20 }}>
              <div style={{
                width: '80px',
                height: '80px',
                borderRadius: '50%',
                background: 'linear-gradient(135deg, #3b82f6, #8b5cf6)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '32px',
                color: 'white',
                fontWeight: 'bold'
              }}>
                {user.firstName ? user.firstName.charAt(0).toUpperCase() : user.email.charAt(0).toUpperCase()}
              </div>
              <div>
                <h2 style={{ margin: '0 0 8px 0' }}>
                  {user.firstName && user.lastName 
                    ? `${user.firstName} ${user.lastName}` 
                    : user.email.split('@')[0]
                  }
                </h2>
                <p style={{ margin: '0 0 4px 0', color: '#9ca3af' }}>{user.email}</p>
                <p style={{ margin: 0, color: '#6ee7b7', fontSize: '14px' }}>
                  🎯 {progressPercentage}% Complete
                </p>
              </div>
            </div>
          </div>

          {/* Progress Overview */}
          <div className="card" style={{ marginBottom: 24 }}>
            <h3>📊 Learning Progress</h3>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '16px', marginTop: 16 }}>
              <div style={{ 
                padding: '16px', 
                background: 'rgba(59, 130, 246, 0.1)', 
                borderRadius: '8px', 
                border: '1px solid #3b82f6',
                textAlign: 'center'
              }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>📚</div>
                <div style={{ fontSize: '24px', fontWeight: 'bold', color: '#60a5fa' }}>
                  {progress.completedLessons.length}
                </div>
                <div style={{ fontSize: '14px', color: '#9ca3af' }}>Lessons Completed</div>
              </div>
              
              <div style={{ 
                padding: '16px', 
                background: 'rgba(16, 185, 129, 0.1)', 
                borderRadius: '8px', 
                border: '1px solid #10b981',
                textAlign: 'center'
              }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>🎯</div>
                <div style={{ fontSize: '24px', fontWeight: 'bold', color: '#6ee7b7' }}>
                  {progress.completedTopics.length}
                </div>
                <div style={{ fontSize: '14px', color: '#9ca3af' }}>Topics Completed</div>
              </div>
              
              <div style={{ 
                padding: '16px', 
                background: 'rgba(245, 158, 11, 0.1)', 
                borderRadius: '8px', 
                border: '1px solid #f59e0b',
                textAlign: 'center'
              }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>⏱️</div>
                <div style={{ fontSize: '24px', fontWeight: 'bold', color: '#fbbf24' }}>
                  {progress.totalTimeSpent}
                </div>
                <div style={{ fontSize: '14px', color: '#9ca3af' }}>Minutes Studied</div>
              </div>
            </div>
          </div>

          {/* Completed Topics */}
          {progress.completedTopics.length > 0 && (
            <div className="card" style={{ marginBottom: 24 }}>
              <h3>🏆 Completed Topics</h3>
              <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px', marginTop: 16 }}>
                {progress.completedTopics.map((topic: string) => (
                  <div
                    key={topic}
                    style={{
                      padding: '8px 12px',
                      background: 'rgba(16, 185, 129, 0.1)',
                      border: '1px solid #10b981',
                      borderRadius: '20px',
                      fontSize: '14px',
                      color: '#6ee7b7',
                      fontWeight: '500'
                    }}
                  >
                    ✅ {topic}
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* Completed Lessons */}
          <div className="card">
            <h3>📖 Completed Lessons</h3>
            {loading && <p>Loading lessons...</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}
            
            {!loading && !error && (
              <>
                {completedLessonsData.length === 0 ? (
                  <div style={{
                    textAlign: 'center',
                    padding: '40px 20px',
                    color: '#9ca3af'
                  }}>
                    <div style={{ fontSize: '48px', marginBottom: '16px' }}>📚</div>
                    <h4 style={{ margin: '0 0 8px 0' }}>No lessons completed yet</h4>
                    <p style={{ margin: 0 }}>Start learning to see your completed lessons here!</p>
                  </div>
                ) : (
                  <div style={{ display: 'grid', gap: '16px', marginTop: 16 }}>
                    {completedLessonsData.map((lesson, index) => (
                      <div key={lesson.id} style={{
                        padding: '20px',
                        background: 'rgba(255, 255, 255, 0.05)',
                        borderRadius: '12px',
                        border: '1px solid rgba(255, 255, 255, 0.1)',
                        transition: 'all 0.2s ease'
                      }}>
                        <div style={{ display: 'flex', alignItems: 'flex-start', gap: '16px' }}>
                          <div style={{
                            background: `linear-gradient(135deg, ${['#3b82f6', '#10b981', '#f59e0b', '#8b5cf6', '#ec4899', '#22c55e', '#06b6d4', '#f97316'][index % 8]}, ${['#1d4ed8', '#059669', '#d97706', '#7c3aed', '#be185d', '#16a34a', '#0891b2', '#ea580c'][index % 8]})`,
                            color: 'white',
                            borderRadius: '50%',
                            width: '40px',
                            height: '40px',
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            fontSize: '18px',
                            fontWeight: 'bold',
                            flexShrink: 0
                          }}>
                            {lesson.id}
                          </div>
                          <div style={{ flex: 1 }}>
                            <h4 style={{ margin: '0 0 8px 0', fontSize: '18px', fontWeight: '600' }}>
                              {lesson.title}
                            </h4>
                            <p style={{ margin: '0 0 12px 0', color: '#9ca3af', fontSize: '14px' }}>
                              {lesson.description}
                            </p>
                            <div style={{
                              padding: '12px 16px',
                              background: 'rgba(16, 185, 129, 0.1)',
                              border: '1px solid #10b981',
                              borderRadius: '8px',
                              display: 'flex',
                              alignItems: 'center',
                              gap: '8px',
                              fontSize: '14px',
                              color: '#6ee7b7',
                              fontWeight: '500'
                            }}>
                              <span style={{ fontSize: '16px' }}>✅</span>
                              Completed
                            </div>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </>
            )}
          </div>

        </main>
      </div>
    </div>
  );
}