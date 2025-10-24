import React, { useState, useRef, useEffect } from "react";
import { apiService, EmotionResult } from "../services/api";
import EmotionDisplay from "./EmotionDisplay";
import "./ChatInterface.css";

interface Message {
  id: number;
  text: string;
  timestamp: Date;
  emotion?: EmotionResult;
}

const ChatInterface: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([]);
  const [inputText, setInputText] = useState("");
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [currentEmotion, setCurrentEmotion] = useState<EmotionResult | null>(
    null
  );
  const [connectionStatus, setConnectionStatus] = useState<
    "checking" | "connected" | "disconnected"
  >("checking");
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    checkConnection();
  }, []);

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const checkConnection = async () => {
    try {
      const isConnected = await apiService.testConnection();
      setConnectionStatus(isConnected ? "connected" : "disconnected");
    } catch (error) {
      setConnectionStatus("disconnected");
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  const handleSendMessage = async () => {
    if (!inputText.trim() || isAnalyzing) return;

    const newMessage: Message = {
      id: Date.now(),
      text: inputText.trim(),
      timestamp: new Date(),
    };

    setMessages((prev) => [...prev, newMessage]);
    setInputText("");
    setIsAnalyzing(true);

    try {
      const emotion = await apiService.analyzeEmotion(newMessage.text);

      setCurrentEmotion(emotion);
      setMessages((prev) =>
        prev.map((msg) =>
          msg.id === newMessage.id ? { ...msg, emotion } : msg
        )
      );
    } catch (error) {
      console.error("Failed to analyze emotion:", error);
      setCurrentEmotion(null);
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
    }
  };

  const formatTime = (date: Date): string => {
    return date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
  };

  const getEmotionEmoji = (emotion?: EmotionResult): string => {
    if (!emotion) return "";
    switch (emotion.label.toLowerCase()) {
      case "positive":
        return "😊";
      case "negative":
        return "😔";
      case "neutral":
        return "😐";
      default:
        return "🤔";
    }
  };

  return (
    <div className="chat-interface">
      <EmotionDisplay emotion={currentEmotion} isAnalyzing={isAnalyzing} />

      <div className="chat-header">
        <h1>Emotion Analysis Chat</h1>
        <div className={`connection-status ${connectionStatus}`}>
          <div className="status-dot"></div>
          <span>
            {connectionStatus === "checking" && "Checking connection..."}
            {connectionStatus === "connected" && "Connected"}
            {connectionStatus === "disconnected" && "Disconnected"}
          </span>
        </div>
      </div>

      <div className="messages-container">
        {messages.length === 0 ? (
          <div className="empty-state">
            <div className="empty-icon">💬</div>
            <h3>Start a conversation</h3>
            <p>Send a message to see real-time emotion analysis</p>
          </div>
        ) : (
          messages.map((message) => (
            <div key={message.id} className="message">
              <div className="message-content">
                <div className="message-text">{message.text}</div>
                <div className="message-meta">
                  <span className="message-time">
                    {formatTime(message.timestamp)}
                  </span>
                  {message.emotion && (
                    <span className="message-emotion">
                      {getEmotionEmoji(message.emotion)} {message.emotion.label}
                    </span>
                  )}
                </div>
              </div>
            </div>
          ))
        )}
        <div ref={messagesEndRef} />
      </div>

      <div className="input-container">
        <div className="input-wrapper">
          <textarea
            value={inputText}
            onChange={(e) => setInputText(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder="Type your message here..."
            className="message-input"
            rows={1}
            disabled={isAnalyzing || connectionStatus === "disconnected"}
          />
          <button
            onClick={handleSendMessage}
            disabled={
              !inputText.trim() ||
              isAnalyzing ||
              connectionStatus === "disconnected"
            }
            className="send-button"
          >
            {isAnalyzing ? (
              <div className="button-spinner"></div>
            ) : (
              <svg
                width="20"
                height="20"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
              >
                <line x1="22" y1="2" x2="11" y2="13"></line>
                <polygon points="22,2 15,22 11,13 2,9"></polygon>
              </svg>
            )}
          </button>
        </div>
        {connectionStatus === "disconnected" && (
          <div className="connection-error">
            Unable to connect to the backend. Please make sure the API is
            running.
            <button onClick={checkConnection} className="retry-button">
              Retry Connection
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default ChatInterface;
