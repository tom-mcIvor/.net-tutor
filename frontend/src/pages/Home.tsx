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

  const handleLessonComplete = (lessonId: number) => {
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
      return (
        <div>
          <h2>Introduction to .NET</h2>
          <p>Welcome to your .NET learning journey! Master the fundamentals of Microsoft's powerful development platform.</p>
          
          {/* What is .NET Section */}
          <section className="card" style={{ marginTop: 24 }}>
            <h3>🎯 What is .NET?</h3>
            <p>.NET is a free, cross-platform, open-source developer platform for building many different types of applications.</p>
            <ul style={{ marginLeft: 20, marginTop: 12 }}>
              <li><strong>Cross-platform:</strong> Runs on Windows, macOS, and Linux</li>
              <li><strong>Open source:</strong> Available on GitHub with community contributions</li>
              <li><strong>High performance:</strong> Optimized runtime and compilation</li>
              <li><strong>Versatile:</strong> Build web, mobile, desktop, games, IoT, and cloud apps</li>
            </ul>
          </section>

          {/* Key Concepts */}
          <section className="card" style={{ marginTop: 16 }}>
            <h3>🔑 Key Concepts to Master</h3>
            <div style={{ display: 'grid', gap: '16px', marginTop: 12 }}>
              <div style={{ padding: '12px', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '8px', border: '1px solid #3b82f6' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#60a5fa' }}>🏗️ .NET Runtime</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>The execution environment that handles running .NET applications, including garbage collection and JIT compilation.</p>
              </div>
              <div style={{ padding: '12px', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '8px', border: '1px solid #10b981' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#6ee7b7' }}>📚 Base Class Library (BCL)</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>A comprehensive set of classes, interfaces, and value types that provide core functionality.</p>
              </div>
              <div style={{ padding: '12px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '8px', border: '1px solid #f59e0b' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#fbbf24' }}>⚡ Common Language Runtime (CLR)</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Manages memory, executes code, and provides services like security and exception handling.</p>
              </div>
            </div>
          </section>

          {/* Learning Path */}
          <section className="card" style={{ marginTop: 16 }}>
            <h3>🛤️ Your Learning Path</h3>
            <div style={{ marginTop: 12 }}>
              <div style={{ display: 'flex', alignItems: 'center', marginBottom: '12px' }}>
                <span style={{ background: '#3b82f6', color: 'white', borderRadius: '50%', width: '24px', height: '24px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', marginRight: '12px' }}>1</span>
                <span><strong>C# Fundamentals:</strong> Variables, data types, control structures, and OOP principles</span>
              </div>
              <div style={{ display: 'flex', alignItems: 'center', marginBottom: '12px' }}>
                <span style={{ background: '#10b981', color: 'white', borderRadius: '50%', width: '24px', height: '24px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', marginRight: '12px' }}>2</span>
                <span><strong>ASP.NET Core:</strong> Build modern web applications and APIs</span>
              </div>
              <div style={{ display: 'flex', alignItems: 'center', marginBottom: '12px' }}>
                <span style={{ background: '#f59e0b', color: 'white', borderRadius: '50%', width: '24px', height: '24px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', marginRight: '12px' }}>3</span>
                <span><strong>Entity Framework:</strong> Database access and ORM patterns</span>
              </div>
              <div style={{ display: 'flex', alignItems: 'center', marginBottom: '12px' }}>
                <span style={{ background: '#8b5cf6', color: 'white', borderRadius: '50%', width: '24px', height: '24px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '12px', marginRight: '12px' }}>4</span>
                <span><strong>Advanced Topics:</strong> Microservices, testing, and deployment</span>
              </div>
            </div>
          </section>

          {/* Code Example */}
          <section className="card" style={{ marginTop: 16 }}>
            <h3>💻 Your First .NET Program</h3>
            <p>Let's start with the classic "Hello World" example:</p>
            <div style={{
              background: '#1e1e1e',
              padding: '16px',
              borderRadius: '8px',
              marginTop: '12px',
              fontFamily: 'Monaco, Consolas, "Courier New", monospace',
              fontSize: '14px',
              color: '#d4d4d4',
              overflow: 'auto'
            }}>
              <div style={{ color: '#569cd6' }}>using</div>
              <div style={{ color: '#d4d4d4', marginLeft: '20px' }}>System;</div>
              <br />
              <div style={{ color: '#569cd6' }}>namespace</div>
              <div style={{ color: '#4ec9b0', display: 'inline', marginLeft: '8px' }}>HelloWorld</div>
              <div style={{ color: '#d4d4d4' }}>{'{'}</div>
              <div style={{ marginLeft: '20px' }}>
                <div style={{ color: '#569cd6' }}>class</div>
                <div style={{ color: '#4ec9b0', display: 'inline', marginLeft: '8px' }}>Program</div>
                <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                <div style={{ marginLeft: '20px' }}>
                  <div style={{ color: '#569cd6' }}>static</div>
                  <div style={{ color: '#569cd6', display: 'inline', marginLeft: '8px' }}>void</div>
                  <div style={{ color: '#dcdcaa', display: 'inline', marginLeft: '8px' }}>Main</div>
                  <div style={{ color: '#d4d4d4', display: 'inline' }}>{'()'}</div>
                  <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                  <div style={{ marginLeft: '20px' }}>
                    <div style={{ color: '#4ec9b0' }}>Console</div>
                    <div style={{ color: '#d4d4d4', display: 'inline' }}>.</div>
                    <div style={{ color: '#dcdcaa', display: 'inline' }}>WriteLine</div>
                    <div style={{ color: '#d4d4d4', display: 'inline' }}>{'('}</div>
                    <div style={{ color: '#ce9178', display: 'inline' }}>"Hello, .NET World!"</div>
                    <div style={{ color: '#d4d4d4', display: 'inline' }}>{')'}</div>
                    <div style={{ color: '#d4d4d4' }}>;</div>
                  </div>
                  <div style={{ color: '#d4d4d4' }}>{'}'}</div>
                </div>
                <div style={{ color: '#d4d4d4' }}>{'}'}</div>
              </div>
              <div style={{ color: '#d4d4d4' }}>{'}'}</div>
            </div>
            <p style={{ marginTop: '12px', fontSize: '14px', color: '#9ca3af' }}>
              💡 <strong>Try it:</strong> Create a new console app with <code style={{ background: 'rgba(255,255,255,0.1)', padding: '2px 6px', borderRadius: '4px' }}>dotnet new console</code>
            </p>
          </section>

          {/* Interactive Elements */}
          <section className="card" style={{ marginTop: 16 }}>
            <h3>🎮 Interactive Learning</h3>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '12px', marginTop: 12 }}>
              <div style={{ padding: '16px', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '8px', border: '1px solid #3b82f6', textAlign: 'center' }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>🎯</div>
                <h4 style={{ margin: '0 0 8px 0' }}>Practice Exercises</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Hands-on coding challenges to reinforce concepts</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '8px', border: '1px solid #10b981', textAlign: 'center' }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>📊</div>
                <h4 style={{ margin: '0 0 8px 0' }}>Progress Tracking</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Monitor your learning journey and achievements</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '8px', border: '1px solid #f59e0b', textAlign: 'center' }}>
                <div style={{ fontSize: '24px', marginBottom: '8px' }}>🏆</div>
                <h4 style={{ margin: '0 0 8px 0' }}>Achievements</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Earn badges as you complete learning milestones</p>
              </div>
            </div>
          </section>

          {/* API Lessons */}
          {loading && <p>Loading additional lessons...</p>}
          {error && <p style={{ color: "red" }}>Error loading lessons: {error}</p>}
          {lessons.length > 0 && (
            <section style={{ marginTop: 24 }}>
              <h3>📚 Additional Lessons</h3>
              <div style={{ marginTop: 16 }}>
                {lessons.map((l) => (
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
                ))}
              </div>
            </section>
          )}

          {/* Completion Button */}
          {user && !isTopicComplete(activeTab) && (
            <div style={{ marginTop: 24, textAlign: 'center' }}>
              <button
                onClick={handleMarkTopicComplete}
                style={{
                  padding: '16px 32px',
                  background: 'linear-gradient(135deg, #10b981, #059669)',
                  color: 'white',
                  border: 'none',
                  borderRadius: '12px',
                  cursor: 'pointer',
                  fontWeight: 600,
                  fontSize: '16px',
                  boxShadow: '0 4px 12px rgba(16, 185, 129, 0.3)'
                }}
              >
                🎉 Complete Introduction to .NET
              </button>
            </div>
          )}
          {user && isTopicComplete(activeTab) && (
            <div style={{
              marginTop: 24,
              padding: '16px',
              background: 'rgba(16, 185, 129, 0.1)',
              border: '1px solid #10b981',
              borderRadius: '12px',
              textAlign: 'center',
              color: '#6ee7b7'
            }}>
              <div style={{ fontSize: '24px', marginBottom: '8px' }}>🎉</div>
              <h3 style={{ margin: '0 0 8px 0' }}>Congratulations!</h3>
              <p style={{ margin: 0 }}>You've completed the Introduction to .NET! Ready for C# Basics?</p>
            </div>
          )}
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