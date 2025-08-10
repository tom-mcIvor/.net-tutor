const API_BASE = import.meta.env.VITE_API_BASE || "http://localhost:5187"; // default .NET dev port if using Kestrel
export async function http<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(\`\${API_BASE}\${path}\`, {
    headers: { "Content-Type": "application/json" },
    ...init,
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(\`HTTP \${res.status}: \${text}\`);
  }
  return (await res.json()) as T;
}
export { API_BASE };
