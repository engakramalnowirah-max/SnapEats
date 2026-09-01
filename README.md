# 🍔 SnapEats Platform - منصة سناب إيتس لإدارة الطلبات والوجبات

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue?style=for-the-badge)
![SignalR](https://img.shields.io/badge/Real--Time-SignalR-FF6F00?style=for-the-badge&logo=signalr)
![Flutter](https://img.shields.io/badge/Mobile-Flutter-02569B?style=for-the-badge&logo=flutter)
![ASP.NET MVC](https://img.shields.io/badge/Admin-ASP.NET%20Core%20MVC-512BD4?style=for-the-badge)

منصة متكاملة وبنية معمارية احترافية تتكون من خلفية نظام متطورة (**ASP.NET Core 9 Clean Architecture**)، لوحة تحكم إدارية تفاعلية (**ASP.NET Core MVC with Kendo UI**)، وتطبيق موجه للعملاء (**Flutter Mobile App**)، مع نظام تواصل لحظي فوري باستغلال **ASP.NET Core SignalR**.

---

## 📐 الهيكلية المعمارية للنظام (Architecture Overview)

يتبع المشروع أفضل الممارسات المعمارية المبنية على **Clean Architecture** والنمط البرمجي **CQRS** باستخدام **MediatR**:

```
 ┌─────────────────────────────────────────────────────────┐
 │                   Flutter Mobile App                    │
 └────────────────────────────┬────────────────────────────┘
                              │ (REST API & SignalR WebSockets)
                              ▼
 ┌─────────────────────────────────────────────────────────┐
 │                      SnapEats.API                       │
 └────────────────────────────┬────────────────────────────┘
                              │
                              ▼
 ┌─────────────────────────────────────────────────────────┐
 │                   SnapEats.Application                  │
 │      (MediatR Commands, Queries, FluentValidation)      │
 └────────────────────────────┬────────────────────────────┘
                              │
                              ▼
 ┌─────────────────────────────────────────────────────────┐
 │                 SnapEats.Infrastructure                 │
 │       (EF Core DbContext, SignalR Hubs, Repositories)   │
 └────────────────────────────┬────────────────────────────┘
                              │
                              ▼
 ┌─────────────────────────────────────────────────────────┐
 │                    SnapEats.Domain                      │
 │      (Entities, Value Objects, Domain Exceptions)       │
 └─────────────────────────────────────────────────────────┘
                              │
                              ▼
 ┌─────────────────────────────────────────────────────────┐
 │                   SnapEats.AdminMVC                     │
 │           (Dashboard, Kendo UI, Real-Time DOM)          │
 └─────────────────────────────────────────────────────────┘
```

---

## ⚡ نظام التواصل اللحظي (SignalR Real-Time Communication)

يدعم النظام التحديث اللحظي المتبادل بين لوحة التحكم وتطبيق الموبايل بدون الحاجة لعمل تحديث للصفحة (**No Page Refresh Required**):

| الحدث (Event Name) | المصدر (Source) | المستقبل (Target) | الوصف والتأثير اللحظي |
| :--- | :--- | :--- | :--- |
| `OrderCreated` | Flutter Mobile / API | Admin MVC | إظهار الطلب فوراً بأعلى الجدول، إشعارات صوتية، وتحديث عدادات اللوحة |
| `OrderStatusChanged` | Admin MVC | Flutter Mobile | تحديث حالة الطلب لحظياً على شاشة العميل في الموبايل |
| `OrderCancelled` | Mobile / MVC | Mobile & MVC | تحديث حالة الإلغاء فوراً في كلاً من المتصفح والموبايل |
| `OrderDeleted` | Admin MVC | Mobile & MVC | حذف الطلب فوراً وإخفائه من الشاشات |
| `CategoryChanged` | Admin MVC | Flutter Mobile | تحديث قائمة التصنيفات فوراً لدى العملاء |
| `MenuItemChanged` | Admin MVC | Flutter Mobile | تحديث الوجبات والأسعار لحظياً على الموبايل |

---

## 🚀 المميزات الرئيسية (Key Features)

1. **إدارة المسؤولين (Admins Management)**:
   - إضافة وحذف وعرض حسابات مدراء النظام والمسؤولين مع التحكم في الصلاحيات.
2. **إدارة التصنيفات والوجبات (Categories & Menu Items)**:
   - إنشاء وتعديل وحذف التصنيفات والمنتجات مع ربط الصور والتأكد من عدم التكرار.
3. **إدارة الطلبات (Orders Management)**:
   - متابعة الطلبات، تغيير الحالات (قيد الانتظار، التجهيز، للتوصيل، تم التسليم)، وحذف الطلبات بأمان.
4. **حذف آمن من الداتابيز حتى اللوحة (End-to-End Safe Delete)**:
   - معالجة عمليات الحذف عبر EF Core للحذف التتابعي للارتباطات (Cascade Delete) ومنع أخطاء Foreign Keys.
5. **تطبيق موبايل فلاتر متكامل (Flutter Mobile Application)**:
   - تصفح المنتجات والتصنيفات، إضافة للسلة، تقديم الطلبات، ومتابعة حالة الطلب لحظياً.

---

## 🛠️ كيفية التشغيل والتجهيز (Getting Started)

### 1. المتطلبات الأساسية
- **.NET 9.0 SDK**
- **SQL Server / LocalDB**
- **Flutter SDK** (للصيانة وتطبيق الموبايل)

### 2. إعداد قاعدة البيانات (Database Setup)
افتح الـ Terminal في مجلد المشروع ونفذ الأمر التالي لإنشاء قاعدة البيانات وجداول النظام:
```bash
dotnet ef database update --project src/SnapEats.Infrastructure --startup-project src/SnapEats.API
```

### 3. تشغيل الخلفية والـ API (`SnapEats.API`)
```bash
cd src/SnapEats.API
dotnet run
```
> **رابط الـ API**: `http://localhost:5065`  
> **Swagger UI**: `http://localhost:5065/swagger`

### 4. تشغيل لوحة التحكم (`SnapEats.AdminMVC`)
```bash
cd src/SnapEats.AdminMVC
dotnet run
```
> **رابط لوحة التحكم**: `http://localhost:5000/Admin`

### 5. تشغيل تطبيق الموبايل (`snapeats_mobile`)
```bash
cd snapeats_mobile
flutter pub get
flutter run
```

---

## 🌐 دليل الـ API Endpoints الرئيسي (API Reference)

### 👑 إدارة المسؤولين (Admin)
- `GET /api/v1/Admin` - جلب جميع المسؤولين (مع دعم التصفح والبحث)
- `GET /api/v1/Admin/{id}` - جلب بيانات مسؤول محدد
- `POST /api/v1/Admin/create` - إنشاء حساب مسؤول جديد
- `DELETE /api/v1/Admin/{id}` - حذف حساب مسؤول

### 🏷️ التصنيفات (Categories)
- `GET /api/v1/Category` - قائمة التصنيفات
- `POST /api/v1/Category` - إضافة تصنيف
- `PUT /api/v1/Category/{id}` - تعديل تصنيف
- `DELETE /api/v1/Category/{id}` - حذف تصنيف

### 🍔 قائمة الطعام (MenuItems)
- `GET /api/v1/MenuItem` - قائمة الوجبات والمنتجات
- `POST /api/v1/MenuItem` - إضافة وجبة جديدة
- `PUT /api/v1/MenuItem/{id}` - تعديل وجبة
- `DELETE /api/v1/MenuItem/{id}` - حذف وجبة

### 🛒 الطلبات (Customer Orders)
- `GET /api/v1/Order` - قائمة جميع الطلبات
- `POST /api/v1/Order` - إنشاء طلب جديد (من الموبايل)
- `PUT /api/v1/Order/{id}/status` - تحديث حالة الطلب
- `DELETE /api/v1/Order/{id}` - حذف طلب

---

## 📝 الترخيص وحقوق الملكية
جميع الحقوق محفوظة منصة **SnapEats** © 2026.
