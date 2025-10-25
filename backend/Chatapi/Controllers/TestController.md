# TestController

Duygu analizi API servisi ve sistem test endpoint'leri.

### GET /api/Test

**Amaç:** API bağlantı testi

**Response:**

- `200 OK` - "API çalışıyor!"

### POST /api/Test/analyze

**Amaç:** Metin duygu analizi yap

**Request:**

```json
{ "text": "Ben çok mutluyum!" }
```

**Response:** Duygu analizi sonucu

```json
{
  "label": "positive/negative/neutral",
  "score": 0.95
}
```

**Harici Servis:**

- Gradio API (Hugging Face Spaces)
- URL: `umutt000-ai-sentiment-service.hf.space`

**Süreç:**

1. Event ID al
2. Sonuç için poll
3. Analiz sonucunu döndür

## ⏱ Timeout

- 20 deneme (her biri 1 saniye)
- Toplam ~20 saniye timeout
