/**
 * Resolve API base from Vite env with sane defaults and normalization.
 * In dev, we expect VITE_API_BASE to be "/api" to use the Vite proxy (see vite.config.ts).
 */
const RAW_API_BASE = import.meta.env.VITE_API_BASE ?? "/api";

// Normalize: remove trailing slashes; ensure leading slash for relative proxy base
const API_BASE = (() => {
  let b = String(RAW_API_BASE);
  // Trim whitespace
  b = b.trim();
  // If it's a relative path (no scheme), ensure single leading slash
  const isAbsolute = /^https?:\/\//i.test(b);
  if (!isAbsolute) {
    b = "/" + b.replace(/^\/+/, "");
  }
  // Remove trailing slashes
  b = b.replace(/\/+$/, "");
  return b;
})();

// Log once for clarity
if (typeof window !== "undefined") {
  console.log("[HTTP] API_BASE resolved to:", API_BASE);
}

function joinUrl(base: string, path: string): string {
  const p = String(path || "");
  if (!p) return base;
  const left = base.replace(/\/+$/, "");
  const right = p.replace(/^\/+/, "");
  return `${left}/${right}`;
}

export async function http<T>(path: string, init?: RequestInit): Promise<T> {
  const url = joinUrl(API_BASE, path);
  const method = init?.method || "GET";

  // Console instrumentation for visibility
  console.log("[HTTP] ->", method, url, init ? { ...init } : undefined);

  try {
    const res = await fetch(url, {
      headers: { "Content-Type": "application/json" },
      ...init,
    });

    console.log("[HTTP] <-", method, url, res.status, res.statusText);

    if (!res.ok) {
      const text = await res.text().catch(() => "<unreadable body>");
      console.error("[HTTP] !!", method, url, "HTTP", res.status, text);
      throw new Error(`HTTP ${res.status}: ${text || res.statusText}`);
    }

    // Try to parse JSON with error guard
    const json = (await res.json().catch((e) => {
      console.error("[HTTP] JSON parse error", method, url, e);
      throw new Error("Invalid JSON response");
    })) as T;

    return json;
  } catch (err) {
    console.error("[HTTP] EX", method, url, err);
    throw err;
  }
}

export { API_BASE };
