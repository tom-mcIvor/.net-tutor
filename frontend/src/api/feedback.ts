import { http } from "./http";
import type { Feedback, CreateFeedbackDto } from "../types/feedback";

export async function submitFeedback(feedbackData: CreateFeedbackDto): Promise<Feedback> {
  return http<Feedback>("/feedback", {
    method: "POST",
    body: JSON.stringify(feedbackData),
  });
}

export async function getFeedback(id: number): Promise<Feedback> {
  return http<Feedback>(`/feedback/${id}`);
}

export async function getAllFeedback(): Promise<Feedback[]> {
  return http<Feedback[]>("/feedback");
}