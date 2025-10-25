# Controllers

API endpoint'lerini tanımlayan controller'lar.

## Dosyalar

### UserController.cs

**Amaç:** Kullanıcı yönetimi ve kimlik doğrulama

**Endpoints:**

- `POST /api/User/register` - Kullanıcı kaydı
- `GET /api/User/login/{nickname}` - Giriş
- `GET /api/User` - Tüm kullanıcılar
- `GET /api/User/{id}` - Kullanıcı bilgisi
- `GET /api/User/check-availability/{nickname}` - Nickname kontrol

**Özellikler:**

- Benzersiz nickname zorunluluğu
- 2-50 karakter uzunluk kontrolü
- Büyük-küçük harf duyarsız karşılaştırma

### TestController.cs

**Amaç:** Duygu analizi API'leri

**Endpoints:**

- `GET /api/Test` - Bağlantı testi
- `POST /api/Test/analyze` - Metin duygu analizi

**Özellikler:**

- Harici Gradio API entegrasyonu
- Asenkron işlem desteği
- Timeout kontrolü
