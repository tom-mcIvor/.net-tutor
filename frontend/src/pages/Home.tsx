import { useEffect, useState } from "react";
import { getLessons } from "../api/lessons";
import type { Lesson } from "../types/lesson";
import { LessonCard } from "../components/LessonCard";
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
              lessons.map((l) => <LessonCard key={l.id} lesson={l} />)
            )}
          </div>
        </div>
      );
    }

    // For other tabs, show placeholder content
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
          {renderContent()}
        </main>
      </div>
    </div>
  );
}