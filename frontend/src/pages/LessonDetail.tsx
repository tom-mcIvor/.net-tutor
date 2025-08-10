import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getLesson } from "../api/lessons";
import type { Lesson } from "../types/lesson";

export default function LessonDetail() {
  const params = useParams<{ id: string }>();
  const id = Number(params.id);
  const [lesson, setLesson] = useState<Lesson | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!Number.isFinite(id)) {
      setError("Invalid lesson id");
      setLoading(false);
      return;
    }
    getLesson(id)
      .then(setLesson)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p>Loading lesson...</p>;
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;
  if (!lesson) return <p>Not found</p>;

  return (
    <div>
      <Link to="/">← Back</Link>
      <h1 style={{ marginTop: 12 }}>{lesson.title}</h1>
      <p style={{ color: "#666" }}>{lesson.description}</p>
      <pre style={{ whiteSpace: "pre-wrap", background: "#f7f7f7", padding: 16, borderRadius: 8 }}>
{lesson.content}
      </pre>
    </div>
  );
}
