export interface Feedback {
  id: number;
  message: string;
  userId?: string;
  userEmail?: string;
  createdAt: string;
  pageContext?: string;
}

export interface CreateFeedbackDto {
  message: string;
  pageContext?: string;
}