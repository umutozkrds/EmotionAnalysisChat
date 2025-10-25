# Backend API - Chatapi

.NET Core 7.0 tabanlı web API servisi. Kullanıcı yönetimi ve duygu analizi API'leri sağlar.

### Controllers/

- **UserController.cs** - Kullanıcı kayıt, giriş ve yönetim
- **TestController.cs** - Duygu analizi servisi

### Models/

- **ChatContext.cs** - Entity Framework DbContext
- **User.cs** - Kullanıcı modeli
- **Message.cs** - Mesaj modeli

### Kullanıcı Controller

- `POST /api/User/register` - Yeni kullanıcı kaydı
- `GET /api/User/login/{nickname}` - Kullanıcı girişi
- `GET /api/User` - Tüm kullanıcılar
- `GET /api/User/{id}` - Kullanıcı detayı
- `GET /api/User/check-availability/{nickname}` - Kullanıcı adı kontrolü

### Test Controller

- `GET /api/Test` - API durum kontrolü
- `POST /api/Test/analyze` - Metin duygu analizi

**SQLite** dosya tabanlı veritabanı:

- Kullanıcılar tablosu (Nickname ile unique)
- Mesajlar tablosu
- Otomatik ilişkiler ve cascade delete

## Gereksinimler

- .NET SDK 7.0 veya üzeri
- SQLite (otomatik paketlenir)
