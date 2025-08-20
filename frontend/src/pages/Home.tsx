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

    if (activeTab === 'C# Basics') {
      return (
        <div>
          <h2>C# Basics</h2>
          <p>Master the fundamentals of C# programming language - the foundation of .NET development.</p>
          
          {/* Learning Path Overview */}
          <section className="card" style={{ marginTop: 24 }}>
            <h3>🎯 What You'll Learn</h3>
            <p>This comprehensive C# basics course covers everything you need to start programming in C#:</p>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '16px', marginTop: 16 }}>
              <div style={{ padding: '16px', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '8px', border: '1px solid #3b82f6' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#60a5fa' }}>📝 Variables & Data Types</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Learn to store and work with different types of data in C#</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '8px', border: '1px solid #10b981' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#6ee7b7' }}>⚡ Operators & Expressions</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Master arithmetic, comparison, and logical operations</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '8px', border: '1px solid #f59e0b' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#fbbf24' }}>🔀 Control Structures</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Make decisions and repeat code with if statements and loops</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(139, 92, 246, 0.1)', borderRadius: '8px', border: '1px solid #8b5cf6' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#c4b5fd' }}>🏗️ Methods & Functions</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Organize code into reusable, modular functions</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(236, 72, 153, 0.1)', borderRadius: '8px', border: '1px solid #ec4899' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#f9a8d4' }}>📚 Arrays & Collections</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Store and manipulate multiple values efficiently</p>
              </div>
              <div style={{ padding: '16px', background: 'rgba(34, 197, 94, 0.1)', borderRadius: '8px', border: '1px solid #22c55e' }}>
                <h4 style={{ margin: '0 0 8px 0', color: '#86efac' }}>🎯 Object-Oriented Programming</h4>
                <p style={{ margin: 0, fontSize: '14px' }}>Introduction to classes, objects, and OOP principles</p>
              </div>
            </div>
          </section>

          {/* Interactive Lessons */}
          {loading && <p>Loading lessons...</p>}
          {error && <p style={{ color: "red" }}>Error loading lessons: {error}</p>}
          {lessons.length > 0 && (
            <section style={{ marginTop: 24 }}>
              <h3>📖 Interactive Lessons</h3>
              <p style={{ marginBottom: 16, color: '#9ca3af' }}>
                Work through these hands-on lessons to master C# fundamentals. Each lesson includes explanations, code examples, and practical exercises.
              </p>
              <div style={{ display: 'grid', gap: '16px', marginTop: 16 }}>
                {lessons.map((lesson, index) => (
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
                        <LessonCard lesson={lesson} />
                        {user && (
                          <button
                            onClick={() => handleLessonComplete(lesson.id)}
                            style={{
                              marginTop: 12,
                              padding: '10px 20px',
                              background: 'linear-gradient(135deg, #10b981, #059669)',
                              color: 'white',
                              border: 'none',
                              borderRadius: '8px',
                              cursor: 'pointer',
                              fontWeight: '500',
                              fontSize: '14px',
                              transition: 'all 0.2s ease'
                            }}
                          >
                            ✓ Mark as Complete
                          </button>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </section>
          )}

          {/* Quick Reference */}
          <section className="card" style={{ marginTop: 24 }}>
            <h3>🚀 Quick Start Guide</h3>
            <p>Ready to start coding? Here's your first C# program:</p>
            <div style={{
              background: '#1e1e1e',
              padding: '20px',
              borderRadius: '8px',
              marginTop: '16px',
              fontFamily: 'Monaco, Consolas, "Courier New", monospace',
              fontSize: '14px',
              color: '#d4d4d4',
              overflow: 'auto'
            }}>
              <div style={{ color: '#569cd6' }}>using <span style={{ color: '#d4d4d4' }}>System;</span></div>
              <br />
              <div style={{ color: '#569cd6' }}>class <span style={{ color: '#4ec9b0' }}>Program</span></div>
              <div style={{ color: '#d4d4d4' }}>{'{'}</div>
              <div style={{ marginLeft: '20px' }}>
                <div style={{ color: '#569cd6' }}>static <span style={{ color: '#569cd6' }}>void</span> <span style={{ color: '#dcdcaa' }}>Main</span><span style={{ color: '#d4d4d4' }}>()</span></div>
                <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                <div style={{ marginLeft: '20px' }}>
                  <div style={{ color: '#6a9955' }}>// Your first C# program!</div>
                  <div>
                    <span style={{ color: '#4ec9b0' }}>Console</span>
                    <span style={{ color: '#d4d4d4' }}>.</span>
                    <span style={{ color: '#dcdcaa' }}>WriteLine</span>
                    <span style={{ color: '#d4d4d4' }}>(</span>
                    <span style={{ color: '#ce9178' }}>"Hello, C# World!"</span>
                    <span style={{ color: '#d4d4d4' }}>);</span>
                  </div>
                </div>
                <div style={{ color: '#d4d4d4' }}>{'}'}</div>
              </div>
              <div style={{ color: '#d4d4d4' }}>{'}'}</div>
            </div>
            <p style={{ marginTop: '16px', fontSize: '14px', color: '#9ca3af' }}>
              💡 <strong>Try it:</strong> Create a new console app with <code style={{ background: 'rgba(255,255,255,0.1)', padding: '2px 6px', borderRadius: '4px' }}>dotnet new console</code>
            </p>
          </section>

          {/* Test Your Skills Section */}
          <section className="card" style={{ marginTop: 32, background: 'linear-gradient(135deg, rgba(139, 92, 246, 0.1), rgba(59, 130, 246, 0.1))', border: '1px solid #8b5cf6' }}>
            <h3 style={{ color: '#c4b5fd', display: 'flex', alignItems: 'center', gap: '12px' }}>
              🧠 Test Your Skills
            </h3>
            <p style={{ marginBottom: 24 }}>Ready to put your C# knowledge to the test? Try these challenges to see how much you've learned!</p>
            
            {/* Quiz Questions */}
            <div style={{ marginBottom: 32 }}>
              <h4 style={{ color: '#a78bfa', marginBottom: 16 }}>📝 Quick Quiz</h4>
              <div style={{ display: 'grid', gap: '16px' }}>
                
                {/* Question 1 */}
                <div style={{ padding: '16px', background: 'rgba(255, 255, 255, 0.05)', borderRadius: '8px', border: '1px solid rgba(255, 255, 255, 0.1)' }}>
                  <p style={{ fontWeight: '600', marginBottom: 12 }}>1. Which of these is the correct way to declare an integer variable in C#?</p>
                  <div style={{ display: 'grid', gap: '8px', marginLeft: 16 }}>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q1" value="a" />
                      <span>a) int myNumber = 42;</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q1" value="b" />
                      <span>b) integer myNumber = 42;</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q1" value="c" />
                      <span>c) var myNumber = "42";</span>
                    </label>
                  </div>
                </div>

                {/* Question 2 */}
                <div style={{ padding: '16px', background: 'rgba(255, 255, 255, 0.05)', borderRadius: '8px', border: '1px solid rgba(255, 255, 255, 0.1)' }}>
                  <p style={{ fontWeight: '600', marginBottom: 12 }}>2. What will this code output?</p>
                  <div style={{ background: '#1e1e1e', padding: '12px', borderRadius: '6px', marginBottom: 12, fontFamily: 'monospace', fontSize: '14px' }}>
                    <div style={{ color: '#569cd6' }}>int x = 10;</div>
                    <div style={{ color: '#569cd6' }}>int y = 3;</div>
                    <div style={{ color: '#4ec9b0' }}>Console.WriteLine(x % y);</div>
                  </div>
                  <div style={{ display: 'grid', gap: '8px', marginLeft: 16 }}>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q2" value="a" />
                      <span>a) 3</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q2" value="b" />
                      <span>b) 1</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q2" value="c" />
                      <span>c) 30</span>
                    </label>
                  </div>
                </div>

                {/* Question 3 */}
                <div style={{ padding: '16px', background: 'rgba(255, 255, 255, 0.05)', borderRadius: '8px', border: '1px solid rgba(255, 255, 255, 0.1)' }}>
                  <p style={{ fontWeight: '600', marginBottom: 12 }}>3. Which loop is best for iterating through an array when you don't need the index?</p>
                  <div style={{ display: 'grid', gap: '8px', marginLeft: 16 }}>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q3" value="a" />
                      <span>a) for loop</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q3" value="b" />
                      <span>b) while loop</span>
                    </label>
                    <label style={{ display: 'flex', alignItems: 'center', gap: '8px', cursor: 'pointer' }}>
                      <input type="radio" name="q3" value="c" />
                      <span>c) foreach loop</span>
                    </label>
                  </div>
                </div>
              </div>
              
              <button
                onClick={() => {
                  const answers = { q1: 'a', q2: 'b', q3: 'c' };
                  let score = 0;
                  Object.entries(answers).forEach(([question, correct]) => {
                    const selected = document.querySelector(`input[name="${question}"]:checked`)?.value;
                    if (selected === correct) score++;
                  });
                  alert(`You scored ${score}/3! ${score === 3 ? '🎉 Perfect!' : score === 2 ? '👍 Great job!' : '📚 Keep studying!'}`);
                }}
                style={{
                  marginTop: 16,
                  padding: '12px 24px',
                  background: 'linear-gradient(135deg, #8b5cf6, #7c3aed)',
                  color: 'white',
                  border: 'none',
                  borderRadius: '8px',
                  cursor: 'pointer',
                  fontWeight: '600'
                }}
              >
                Check Answers
              </button>
            </div>

            {/* Coding Challenges */}
            <div style={{ marginBottom: 32 }}>
              <h4 style={{ color: '#a78bfa', marginBottom: 16 }}>💻 Coding Challenges</h4>
              <div style={{ display: 'grid', gap: '20px' }}>
                
                {/* Challenge 1 */}
                <div style={{ padding: '20px', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '12px', border: '1px solid #10b981' }}>
                  <h5 style={{ color: '#6ee7b7', margin: '0 0 12px 0' }}>🟢 Beginner: Temperature Converter</h5>
                  <p style={{ marginBottom: 16 }}>Write a method that converts Celsius to Fahrenheit. Formula: F = (C × 9/5) + 32</p>
                  <div style={{ background: '#1e1e1e', padding: '16px', borderRadius: '8px', fontFamily: 'monospace', fontSize: '14px', marginBottom: 12 }}>
                    <div style={{ color: '#6a9955' }}>// Your code here:</div>
                    <div style={{ color: '#569cd6' }}>public static double CelsiusToFahrenheit(double celsius)</div>
                    <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                    <div style={{ color: '#6a9955', marginLeft: '20px' }}>    // TODO: Implement the conversion</div>
                    <div style={{ color: '#d4d4d4' }}>{'}'}</div>
                  </div>
                  <details style={{ marginTop: 12 }}>
                    <summary style={{ cursor: 'pointer', color: '#10b981', fontWeight: '600' }}>💡 Show Solution</summary>
                    <div style={{ background: '#1e1e1e', padding: '16px', borderRadius: '8px', fontFamily: 'monospace', fontSize: '14px', marginTop: 8 }}>
                      <div style={{ color: '#569cd6' }}>public static double CelsiusToFahrenheit(double celsius)</div>
                      <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                      <div style={{ color: '#569cd6', marginLeft: '20px' }}>return (celsius * 9.0 / 5.0) + 32.0;</div>
                      <div style={{ color: '#d4d4d4' }}>{'}'}</div>
                    </div>
                  </details>
                </div>

                {/* Challenge 2 */}
                <div style={{ padding: '20px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '12px', border: '1px solid #f59e0b' }}>
                  <h5 style={{ color: '#fbbf24', margin: '0 0 12px 0' }}>🟡 Intermediate: Number Guessing Game</h5>
                  <p style={{ marginBottom: 16 }}>Create a simple number guessing game that generates a random number between 1-10 and gives hints.</p>
                  <div style={{ background: '#1e1e1e', padding: '16px', borderRadius: '8px', fontFamily: 'monospace', fontSize: '14px', marginBottom: 12 }}>
                    <div style={{ color: '#6a9955' }}>// Challenge: Implement the game logic</div>
                    <div style={{ color: '#6a9955' }}>// Use Random, loops, and conditional statements</div>
                  </div>
                  <details style={{ marginTop: 12 }}>
                    <summary style={{ cursor: 'pointer', color: '#f59e0b', fontWeight: '600' }}>💡 Show Hint</summary>
                    <div style={{ padding: '12px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '6px', marginTop: 8, fontSize: '14px' }}>
                      Use <code>Random random = new Random()</code> and <code>random.Next(1, 11)</code> to generate numbers.
                      Use a while loop and if-else statements for the game logic.
                    </div>
                  </details>
                </div>

                {/* Challenge 3 */}
                <div style={{ padding: '20px', background: 'rgba(239, 68, 68, 0.1)', borderRadius: '12px', border: '1px solid #ef4444' }}>
                  <h5 style={{ color: '#fca5a5', margin: '0 0 12px 0' }}>🔴 Advanced: Student Grade Calculator</h5>
                  <p style={{ marginBottom: 16 }}>Create a Student class with properties for Name and Grades (array), and methods to calculate average and letter grade.</p>
                  <div style={{ background: '#1e1e1e', padding: '16px', borderRadius: '8px', fontFamily: 'monospace', fontSize: '14px', marginBottom: 12 }}>
                    <div style={{ color: '#6a9955' }}>// Challenge: Use OOP principles</div>
                    <div style={{ color: '#6a9955' }}>// Include properties, constructor, and methods</div>
                  </div>
                  <details style={{ marginTop: 12 }}>
                    <summary style={{ cursor: 'pointer', color: '#ef4444', fontWeight: '600' }}>💡 Show Structure</summary>
                    <div style={{ background: '#1e1e1e', padding: '16px', borderRadius: '8px', fontFamily: 'monospace', fontSize: '14px', marginTop: 8 }}>
                      <div style={{ color: '#569cd6' }}>public class Student</div>
                      <div style={{ color: '#d4d4d4' }}>{'{'}</div>
                      <div style={{ color: '#6a9955', marginLeft: '20px' }}>    // Properties: Name, Grades</div>
                      <div style={{ color: '#6a9955', marginLeft: '20px' }}>    // Methods: CalculateAverage(), GetLetterGrade()</div>
                      <div style={{ color: '#d4d4d4' }}>{'}'}</div>
                    </div>
                  </details>
                </div>
              </div>
            </div>

            {/* Practice Resources */}
            <div>
              <h4 style={{ color: '#a78bfa', marginBottom: 16 }}>🚀 Keep Practicing</h4>
              <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '16px' }}>
                <div style={{ padding: '16px', background: 'rgba(59, 130, 246, 0.1)', borderRadius: '8px', border: '1px solid #3b82f6', textAlign: 'center' }}>
                  <div style={{ fontSize: '24px', marginBottom: '8px' }}>🎯</div>
                  <h5 style={{ margin: '0 0 8px 0' }}>LeetCode Easy</h5>
                  <p style={{ margin: 0, fontSize: '14px' }}>Practice basic algorithms and data structures</p>
                </div>
                <div style={{ padding: '16px', background: 'rgba(16, 185, 129, 0.1)', borderRadius: '8px', border: '1px solid #10b981', textAlign: 'center' }}>
                  <div style={{ fontSize: '24px', marginBottom: '8px' }}>📚</div>
                  <h5 style={{ margin: '0 0 8px 0' }}>Microsoft Learn</h5>
                  <p style={{ margin: 0, fontSize: '14px' }}>Official C# tutorials and exercises</p>
                </div>
                <div style={{ padding: '16px', background: 'rgba(245, 158, 11, 0.1)', borderRadius: '8px', border: '1px solid #f59e0b', textAlign: 'center' }}>
                  <div style={{ fontSize: '24px', marginBottom: '8px' }}>💻</div>
                  <h5 style={{ margin: '0 0 8px 0' }}>Build Projects</h5>
                  <p style={{ margin: 0, fontSize: '14px' }}>Create console apps, calculators, and games</p>
                </div>
              </div>
            </div>
          </section>

          {/* Completion Button */}
          {user && !isTopicComplete(activeTab) && (
            <div style={{ marginTop: 32, textAlign: 'center' }}>
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
                🎉 Complete C# Basics
              </button>
            </div>
          )}
          {user && isTopicComplete(activeTab) && (
            <div style={{
              marginTop: 32,
              padding: '20px',
              background: 'rgba(16, 185, 129, 0.1)',
              border: '1px solid #10b981',
              borderRadius: '12px',
              textAlign: 'center',
              color: '#6ee7b7'
            }}>
              <div style={{ fontSize: '32px', marginBottom: '12px' }}>🎉</div>
              <h3 style={{ margin: '0 0 8px 0' }}>Congratulations!</h3>
              <p style={{ margin: 0 }}>You've mastered C# Basics! Ready for ASP.NET Core?</p>
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