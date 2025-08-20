import { useState } from 'react';
import { submitFeedback } from '../api/feedback';

interface FooterProps {
  pageContext: string;
}

export default function Footer({ pageContext }: FooterProps) {
  const [feedbackText, setFeedbackText] = useState('');
  const [feedbackSubmitted, setFeedbackSubmitted] = useState(false);
  const [feedbackSubmitting, setFeedbackSubmitting] = useState(false);

  const handleFeedbackSubmit = async () => {
    if (feedbackText.trim() && !feedbackSubmitting) {
      setFeedbackSubmitting(true);
      try {
        await submitFeedback({
          message: feedbackText.trim(),
          pageContext: pageContext
        });
        
        setFeedbackSubmitted(true);
        setTimeout(() => {
          setFeedbackSubmitted(false);
          setFeedbackText('');
        }, 3000);
      } catch (error) {
        console.error('Error submitting feedback:', error);
        alert('Failed to submit feedback. Please try again.');
      } finally {
        setFeedbackSubmitting(false);
      }
    }
  };

  return (
    <footer style={{
      marginTop: 'auto',
      padding: '32px 24px 24px 24px',
      background: 'rgba(17, 24, 39, 0.8)',
      borderTop: '1px solid rgba(255, 255, 255, 0.1)',
      margin: '48px 0 0 0'
    }}>
      <h3 style={{ 
        color: '#a5b4fc', 
        display: 'flex', 
        alignItems: 'center', 
        gap: '12px', 
        marginBottom: 16,
        fontSize: '18px',
        fontWeight: '600'
      }}>
        💬 Share Your Feedback
      </h3>
      <p style={{ 
        marginBottom: 16, 
        color: '#9ca3af',
        fontSize: '14px',
        lineHeight: '1.5'
      }}>
        Help us improve your learning experience! Share your thoughts, suggestions, or report any issues.
      </p>
      
      {!feedbackSubmitted ? (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
          <textarea
            value={feedbackText}
            onChange={(e) => setFeedbackText(e.target.value)}
            placeholder="Tell us what you think about this lesson, suggest improvements, or report any issues..."
            rows={4}
            style={{
              width: '100%',
              padding: '12px 16px',
              borderRadius: '8px',
              border: '1px solid rgba(255, 255, 255, 0.2)',
              background: 'rgba(255, 255, 255, 0.1)',
              color: 'inherit',
              fontSize: '14px',
              fontFamily: 'inherit',
              outline: 'none',
              resize: 'vertical',
              minHeight: '100px',
              transition: 'all 0.2s ease',
              boxSizing: 'border-box'
            }}
            onFocus={(e) => {
              e.target.style.border = '1px solid #6366f1';
              e.target.style.boxShadow = '0 0 0 3px rgba(99, 102, 241, 0.1)';
            }}
            onBlur={(e) => {
              e.target.style.border = '1px solid rgba(255, 255, 255, 0.2)';
              e.target.style.boxShadow = 'none';
            }}
          />
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <span style={{ fontSize: '12px', color: '#6b7280' }}>
              {feedbackText.length}/500 characters
            </span>
            <button
              onClick={handleFeedbackSubmit}
              disabled={!feedbackText.trim() || feedbackSubmitting}
              style={{
                padding: '10px 20px',
                background: (feedbackText.trim() && !feedbackSubmitting)
                  ? 'linear-gradient(135deg, #6366f1, #8b5cf6)'
                  : 'rgba(107, 114, 128, 0.5)',
                color: 'white',
                border: 'none',
                borderRadius: '8px',
                cursor: (feedbackText.trim() && !feedbackSubmitting) ? 'pointer' : 'not-allowed',
                fontWeight: '600',
                fontSize: '14px',
                transition: 'all 0.2s ease',
                opacity: (feedbackText.trim() && !feedbackSubmitting) ? 1 : 0.6
              }}
              onMouseOver={(e) => {
                if (feedbackText.trim() && !feedbackSubmitting) {
                  (e.target as HTMLButtonElement).style.transform = 'translateY(-1px)';
                  (e.target as HTMLButtonElement).style.boxShadow = '0 4px 12px rgba(99, 102, 241, 0.3)';
                }
              }}
              onMouseOut={(e) => {
                (e.target as HTMLButtonElement).style.transform = 'translateY(0)';
                (e.target as HTMLButtonElement).style.boxShadow = 'none';
              }}
            >
              {feedbackSubmitting ? '⏳ Submitting...' : '📤 Submit Feedback'}
            </button>
          </div>
        </div>
      ) : (
        <div style={{
          padding: '20px',
          background: 'rgba(16, 185, 129, 0.1)',
          border: '1px solid #10b981',
          borderRadius: '8px',
          textAlign: 'center',
          color: '#6ee7b7'
        }}>
          <div style={{ fontSize: '24px', marginBottom: '8px' }}>✅</div>
          <h4 style={{ margin: '0 0 8px 0' }}>Thank you for your feedback!</h4>
          <p style={{ margin: 0, fontSize: '14px' }}>
            Your input helps us create a better learning experience for everyone.
          </p>
        </div>
      )}
    </footer>
  );
}