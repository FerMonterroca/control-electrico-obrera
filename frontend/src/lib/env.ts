const publicApiUrl = import.meta.env.PUBLIC_API_URL?.trim();

if (!publicApiUrl) {
  throw new Error('PUBLIC_API_URL is required.');
}

export const env = Object.freeze({
  apiUrl: publicApiUrl.replace(/\/$/, ''),
});
