import { useEffect, useState } from "react";
import { getLessons } from "../api/lessons";
import type { Lesson } from "../types/lesson";
import { LessonCard } from "../components/LessonCard";

export default function Home() {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getLessons()
      .then(setLessons)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading lessons...</p>;
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div>
      <h1>.NET Tutor</h1>
      <p>Learn the .NET framework through guided lessons and examples.</p>
      <div style={{ marginTop: 24 }}>
        {lessons.length === 0 ? <p>No lessons yet.</p> : lessons.map((l) => <LessonCard key={l.id} lesson={l} />)}
      </div>
    </div>
  );
}
