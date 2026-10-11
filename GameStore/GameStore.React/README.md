# Game Store React Frontend

A simple React application for managing games with basic CRUD operations.

## Features
- View games in a table format
- Add new games
- Edit existing games
- Delete games

## Prerequisites
- Node.js (v18+ recommended)
- Game Store API running on http://localhost:5274

## Setup
1. Install dependencies:
   ```bash
   npm install
   ```

2. Start the development server:
   ```bash
   npm run dev
   ```

3. Open http://localhost:5173 in your browser

## Docker Deployment
Build the Docker image:
```bash
docker build -t gamestore-react .
```

## Technology Stack
- React 18
- TypeScript
- Bootstrap 5
- Vite
- React Router