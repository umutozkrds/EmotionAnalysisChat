# Emotion Analysis Chat

Duygu analizi destekli sohbet uygulaması. Bu proje, .NET Core backend, React frontend ve React Native mobil uygulamasından oluşan tam kapsamlı bir chat sistemidir.

## 📁 Proje Yapısı

```
EmotionAnalysisChat/
├── backend/              # .NET Core 7.0 Web API
│   └── Chatapi/         # ASP.NET Core backend uygulaması
│       ├── Controllers/ # API Controller'ları
│       └── Models/      # Veritabanı modelleri
├── frontend/            # React Web Uygulaması
│   └── emotion-chat/    # React TypeScript frontend
├── mobil/               # React Native Mobil Uygulama
│   └── MyFirstApp/      # Expo tabanlı mobil app
└── ai-service/          # AI servis entegrasyonu
```

### Kullanılan Ai Araçları 
- Hata ve Kurulum yardımları için Chatgpt
- Ön yüz Tasarım için Cursor
- Mobil kısmının tamamı frontendden entegre için Cursor


### Backend (.NET Core)

-Kullanıcı kayıt ve giriş sistemi
-SQLite veritabanı ile hafif ve taşınabilir
-RESTful API tasarımı
-Emotion analysis API entegrasyonu
-CORS desteği

### Frontend (React)

-Modern ve responsive UI
-Kullanıcı kimlik doğrulama
-Gerçek zamanlı duygu analizi
-Karanlık tema tasarımı
-TypeScript ile tip güvenliği

### Mobil (React Native)

-Expo tabanlı cross-platform
-Backend API entegrasyonu

- Responsive mobil tasarım

### Backend'i Çalıştırma

```bash
cd backend/Chatapi
dotnet run
```

Backend: http://localhost:8080

### Frontend'i Çalıştırma

```bash
cd frontend/emotion-chat
npm install
npm start
```

Frontend: http://localhost:3000

### Mobil Uygulamayı Çalıştırma

```bash
cd mobil/MyFirstApp
npm install
npm start
```

## API Endpoints

### Kullanıcı İşlemleri

- `POST /api/User/register` - Yeni kullanıcı kaydı
- `GET /api/User/login/{nickname}` - Kullanıcı girişi
- `GET /api/User` - Tüm kullanıcıları listele

### Duygu Analizi

- `POST /api/Test/analyze` - Metin duygu analizi
