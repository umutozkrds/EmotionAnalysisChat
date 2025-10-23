# Emotion Analysis Chat

A React-based chat interface that analyzes the emotional sentiment of messages in real-time using a backend API.

## Features

- 💬 Real-time chat interface
- 🎭 Emotion analysis for each message
- 📊 Visual emotion display with confidence scores
- 🎨 Modern, responsive UI with glassmorphism design
- 🔄 Connection status monitoring
- 📱 Mobile-friendly responsive design

## Getting Started

### Prerequisites

Make sure the backend API is running on `https://localhost:7193`. The backend should have:

- `/api/test` endpoint for connection testing
- `/api/test/analyze` endpoint for emotion analysis

### Installation

1. Navigate to the project directory:

   ```bash
   cd frontend/emotion-chat
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

## Usage

1. **Send Messages**: Type your message in the input field and press Enter or click the send button
2. **View Emotions**: Emotion analysis results appear in the top-right corner with:
   - Emotion label (Positive, Negative, Neutral)
   - Confidence score
   - Visual progress bar
   - Appropriate emoji
3. **Message History**: All messages are displayed with their timestamps and emotion analysis results

## API Integration

The app communicates with the backend through:

- **POST** `/api/test/analyze` - Analyzes text emotion
- **GET** `/api/test` - Tests API connection

## Technologies Used

- React 18 with TypeScript
- Axios for API communication
- CSS3 with modern features (backdrop-filter, gradients)
- Responsive design principles

## Architecture

- `ChatInterface.tsx` - Main chat component with message handling
- `EmotionDisplay.tsx` - Floating emotion analysis display
- `api.ts` - API service layer for backend communication
- Modern CSS with glassmorphism effects and smooth animations
