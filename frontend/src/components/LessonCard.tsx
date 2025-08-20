import type { Lesson } from "../types/lesson";
import { useNavigate } from "react-router-dom";

export function LessonCard({ lesson }: { lesson: Lesson }) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate("/lesson/" + lesson.id);
  };

  return (
    <div
      onClick={handleClick}
      style={{
        border: "1px solid rgba(255, 255, 255, 0.2)",
        padding: 16,
        borderRadius: 8,
        marginBottom: 12,
        cursor: 'pointer',
        transition: 'all 0.2s ease',
        background: 'rgba(255, 255, 255, 0.05)'
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.transform = 'translateY(-2px)';
        e.currentTarget.style.boxShadow = '0 4px 12px rgba(0, 0, 0, 0.1)';
        e.currentTarget.style.borderColor = 'rgba(59, 130, 246, 0.5)';
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.transform = 'translateY(0)';
        e.currentTarget.style.boxShadow = 'none';
        e.currentTarget.style.borderColor = 'rgba(255, 255, 255, 0.2)';
      }}
    >
      <h3 style={{ margin: 0, color: '#f3f4f6' }}>{lesson.title}</h3>
      <p style={{ marginTop: 8, color: "#9ca3af", marginBottom: 0 }}>{lesson.description}</p>
      <div style={{
        marginTop: 12,
        fontSize: '14px',
        color: '#60a5fa',
        fontWeight: '500'
      }}>
        Click to open lesson →
      </div>
    </div>
  );
}
