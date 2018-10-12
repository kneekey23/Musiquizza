let backendHost;

const hostname = window.location.hostname;

if (hostname === 'w0ez5wni2e.execute-api.us-east-1.amazonaws.com') {
    backendHost = '/Prod';
} else {
    backendHost = process.env.REACT_APP_BACKEND_HOST || 'https://localhost:5001';
}

export const API_ROOT = `${backendHost}/api`;
