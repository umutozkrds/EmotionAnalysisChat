import axios from "axios";

const API_BASE_URL =
  process.env.NODE_ENV === "development"
    ? "https://emotionanalysischat-5.onrender.com/api"
    : "https://emotionanalysischat-5.onrender.com/api";

export interface AnalyzeRequest {
  text: string;
}

export interface EmotionResult {
  label: string;
  score: number;
}

class ApiService {
  private axiosInstance;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: API_BASE_URL,
      timeout: 30000, // 30 seconds timeout for emotion analysis
      headers: {
        "Content-Type": "application/json",
      },
    });
  }

  async analyzeEmotion(text: string): Promise<EmotionResult> {
    try {
      const response = await this.axiosInstance.post("/Test/analyze", {
        text: text,
      });

      const data = response.data;
      console.log("Raw response:", data);

      if (typeof data === "string") {
        if (data.includes("event: complete") && data.includes("data: ")) {
          const lines = data.split("\n");
          const dataLine = lines.find((line) => line.startsWith("data: "));
          if (dataLine) {
            const jsonStr = dataLine.replace("data: ", "");
            const parsed = JSON.parse(jsonStr);
            if (Array.isArray(parsed) && parsed[0]) {
              return parsed[0];
            }
          }
        } else {
          const parsed = JSON.parse(data);
          if (parsed.data && parsed.data[0]) {
            return parsed.data[0];
          } else if (Array.isArray(parsed) && parsed[0]) {
            return parsed[0];
          }
        }
      } else if (data.data && data.data[0]) {
        return data.data[0];
      } else if (Array.isArray(data) && data[0]) {
        return data[0];
      }

      throw new Error("Unexpected response format from emotion analysis");
    } catch (error) {
      console.error("Error analyzing emotion:", error);
      throw error;
    }
  }

  async testConnection(): Promise<boolean> {
    try {
      const response = await this.axiosInstance.get("/Test");
      return response.status === 200;
    } catch (error) {
      console.error("Error testing connection:", error);
      return false;
    }
  }
}

export const apiService = new ApiService();
