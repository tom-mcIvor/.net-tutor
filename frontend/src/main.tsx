import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import { ProgressProvider } from "./contexts/ProgressContext";
import Home from "./pages/Home";
import LessonDetail from "./pages/LessonDetail";
import "./index.css";

const router = createBrowserRouter([
  { path: "/", element: <Home /> },
  { path: "/lesson/:id", element: <LessonDetail /> },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AuthProvider>
      <ProgressProvider>
        <RouterProvider router={router} />
      </ProgressProvider>
    </AuthProvider>
  </React.StrictMode>
);
