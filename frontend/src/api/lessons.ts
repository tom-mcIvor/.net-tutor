import { http } from "./http";
import type { Lesson } from "../types/lesson";

export async function getLessons(): Promise<Lesson[]> {
  return http<Lesson[]>("/lessons");
}

export async function getLesson(id: number): Promise<Lesson> {
  return http<Lesson>(`/lessons/${id}`);
}
