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
        border: "2px solid rgba(59, 130, 246, 0.3)",
        padding: 24,
        borderRadius: 16,
        marginBottom: 16,
        cursor: 'pointer',
        transition: 'all 0.3s ease',
        background: 'linear-gradient(135deg, rgba(59, 130, 246, 0.1), rgba(16, 185, 129, 0.05))',
        boxShadow: '0 4px 16px rgba(0, 0, 0, 0.1)',
        position: 'relative',
        overflow: 'hidden'
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.transform = 'translateY(-8px) scale(1.02)';
        e.currentTarget.style.boxShadow = '0 12px 32px rgba(59, 130, 246, 0.2)';
        e.currentTarget.style.borderColor = 'rgba(59, 130, 246, 0.6)';
        e.currentTarget.style.background = 'linear-gradient(135deg, rgba(59, 130, 246, 0.15), rgba(16, 185, 129, 0.1))';
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.transform = 'translateY(0) scale(1)';
        e.currentTarget.style.boxShadow = '0 4px 16px rgba(0, 0, 0, 0.1)';
        e.currentTarget.style.borderColor = 'rgba(59, 130, 246, 0.3)';
        e.currentTarget.style.background = 'linear-gradient(135deg, rgba(59, 130, 246, 0.1), rgba(16, 185, 129, 0.05))';
      }}
    >
      <div style={{
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        height: '4px',
        background: 'linear-gradient(90deg, #3b82f6, #10b981)',
        borderRadius: '16px 16px 0 0'
      }} />
      <div style={{ paddingTop: '8px' }}>
        <h3 style={{
          margin: 0,
          color: '#f3f4f6',
          fontSize: '18px',
          fontWeight: '600',
          marginBottom: '12px'
        }}>
          📚 {lesson.title}
        </h3>
        <p style={{
          marginTop: 0,
          color: "#d1d5db",
          marginBottom: '16px',
          lineHeight: '1.5'
        }}>
          {lesson.description}
        </p>
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          marginTop: '16px',
          paddingTop: '16px',
          borderTop: '1px solid rgba(255, 255, 255, 0.1)'
        }}>
          <div style={{
            fontSize: '14px',
            color: '#60a5fa',
            fontWeight: '600',
            display: 'flex',
            alignItems: 'center',
            gap: '8px'
          }}>
            <span>🎯</span>
            Start Learning
          </div>
          <div style={{
            fontSize: '24px',
            color: '#60a5fa',
            transition: 'transform 0.2s ease'
          }}>
            →
          </div>
        </div>
      </div>
    </div>
  );
}
