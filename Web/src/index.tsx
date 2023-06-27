import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

const ENV = {
  CELESTIALS_PLANET_ENDPOINT: process.env.NODE_ENV === "development" 
    ? "http://localhost:8081/Planet"
    : "https://celestials-web-app.azurewebsites.net/Planet/"
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <App env={ENV} />
  </React.StrictMode>
);


reportWebVitals();
