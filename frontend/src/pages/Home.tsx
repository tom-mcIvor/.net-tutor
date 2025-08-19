import { useEffect, useState } from "react";
import { getLessons } from "../api/lessons";
import type { Lesson } from "../types/lesson";
import { LessonCard } from "../components/LessonCard";
import { Progress } from "../components/Progress";
import { useProgress } from "../contexts/ProgressContext";
import { useAuth } from "../contexts/AuthContext";
import Sidebar from "../components/Sidebar";
import "../App.css";

const TABS = [
  'Introduction to .NET',
  'C# Basics',
  'ASP.NET Core Overview',
  'Entity Framework Core',
  '.NET Libraries & Tools',
  'Advanced Topics'
];

export default function Home() {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [activeTab, setActiveTab] = useState(TABS[0]);
  const { markTopicComplete, markLessonComplete, isTopicComplete, addTimeSpent } = useProgress();
  const { user } = useAuth();

  useEffect(() => {
    let isMounted = true;

    const fetchLessons = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getLessons();
        if (isMounted) setLessons(data);
      } catch (err) {
        if (!isMounted) return;
        const message = err instanceof Error ? err.message : String(err);
        setError(message);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchLessons();
    return () => {
      isMounted = false;
    };
  }, []);

  // Track time spent when switching tabs
  useEffect(() => {
    const startTime = Date.now();
    return () => {
      if (user) {
        const timeSpent = Math.round((Date.now() - startTime) / 60000); // Convert to minutes
        if (timeSpent > 0) {
          addTimeSpent(timeSpent);
        }
      }
    };
  }, [activeTab, user, addTimeSpent]);

  const handleLessonComplete = (lessonId: string) => {
    markLessonComplete(lessonId.toString());
    // Also mark the current topic as complete if this is the first lesson
    if (!isTopicComplete(activeTab)) {
      markTopicComplete(activeTab);
    }
  };

  const handleMarkTopicComplete = () => {
    markTopicComplete(activeTab);
  };

  const renderContent = () => {
    if (activeTab === 'Introduction to .NET') {
      // Show the lesson cards for Introduction to .NET
      if (loading) return <p>Loading lessons...</p>;
      if (error) return <p style={{ color: "red" }}>Error: {error}</p>;
      
      return (
        <div>
          <h2>Introduction to .NET</h2>
          <p>Learn the .NET framework through guided lessons and examples.</p>
          <div style={{ marginTop: 24 }}>
            {lessons.length === 0 ? (
              <p>No lessons yet.</p>
            ) : (
              lessons.map((l) => (
                <div key={l.id} style={{ marginBottom: 16 }}>
                  <LessonCard lesson={l} />
                  {user && (
                    <button
                      onClick={() => handleLessonComplete(l.id)}
                      style={{
                        marginTop: 8,
                        padding: '8px 16px',
                        background: '#10b981',
                        color: 'white',
                        border: 'none',
                        borderRadius: '6px',
                        cursor: 'pointer'
                      }}
                    >
                      Mark as Complete
                    </button>
                  )}
                </div>
              ))
            )}
          </div>
        </div>
      );
    }

    // For other tabs, show placeholder content with completion button
    return (
      <section className="card">
        <h2>{activeTab}</h2>
        <p>
          This section contains content for <strong>{activeTab}</strong>.
          This is where detailed tutorials, code examples, and interactive content will be displayed.
        </p>
        <p>
          Each tab represents a different learning module in the .NET Tutor curriculum,
          designed to take you from beginner to advanced .NET development.
        </p>
        {user && !isTopicComplete(activeTab) && (
          <button
            onClick={handleMarkTopicComplete}
            style={{
              marginTop: 16,
              padding: '12px 24px',
              background: '#10b981',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              cursor: 'pointer',
              fontWeight: 600
            }}
          >
            Mark Topic as Complete
          </button>
        )}
        {user && isTopicComplete(activeTab) && (
          <div style={{
            marginTop: 16,
            padding: '12px 16px',
            background: 'rgba(16, 185, 129, 0.1)',
            border: '1px solid #10b981',
            borderRadius: '8px',
            color: '#6ee7b7'
          }}>
            ✅ Topic Completed!
          </div>
        )}
      </section>
    );
  };

  return (
    <div className="layout">
      <Sidebar
        tabs={TABS}
        activeTab={activeTab}
        setActiveTab={setActiveTab}
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
          <h1 className="page-title">{activeTab}</h1>
        </header>

        <main className="main">
          <Progress />
          {renderContent()}
        </main>
      </div>
    </div>
  );
}