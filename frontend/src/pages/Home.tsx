import { useEffect, useState } from "react";
import { getLessons } from "../api/lessons";
import type { Lesson } from "../types/lesson";
import { LessonCard } from "../components/LessonCard";

export default function Home() {
const [lessons, setLessons] = useState<Lesson[]>([]);
const [error, setError] = useState<string | null>(null);
const [loading, setLoading] = useState(true);

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

if (loading) return <p>Loading lessons...</p>;
if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

return (
<div>
<h1>.NET Tutor</h1>
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