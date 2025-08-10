import type { Lesson } from "../types/lesson";
import { Link } from "react-router-dom";

export function LessonCard({ lesson }: { lesson: Lesson }) {
  return (
    <div style={{ border: "1px solid #ddd", padding: 16, borderRadius: 8, marginBottom: 12 }}>
      <h3 style={{ margin: 0 }}>{lesson.title}</h3>
      <p style={{ marginTop: 8, color: "#555" }}>{lesson.description}</p>
      <Link to={"/lesson/" + lesson.id}>Open lesson</Link>
    </div>
  );
}
