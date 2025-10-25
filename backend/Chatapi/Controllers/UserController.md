# UserController

Kullanıcı yönetimi ve kimlik doğrulama için REST API endpoint'leri.

### POST /api/User/register

**Amaç:** Yeni kullanıcı kaydı

**Request:**

```json
{ "nickname": "kullaniciadi" }
```

**Response:**

- `200 OK` - Kullanıcı oluşturuldu
- `409 Conflict` - Nickname zaten var
- `400 Bad Request` - Geçersiz format

### GET /api/User/login/{nickname}

**Amaç:** Mevcut kullanıcı girişi

**Response:**

- `200 OK` - Kullanıcı bilgileri
- `404 Not Found` - Kullanıcı bulunamadı

### GET /api/User

**Amaç:** Tüm kullanıcıları listele

**Response:** Kullanıcı listesi (tarihe göre sıralı)

### GET /api/User/{id}

**Amaç:** Kullanıcı detayı

### GET /api/User/check-availability/{nickname}

**Amaç:** Nickname müsaitlik kontrolü

**Response:**

```json
{
  "available": true/false,
  "nickname": "test",
  "message": "Müsait/Müsait değil"
}
```

## 🔐 Validasyon Kuralları

- Nickname: 2-50 karakter
- Büyük-küçük harf duyarsız
- Boşluk otomatik temizlenir
- Veritabanında benzersiz olmalı
