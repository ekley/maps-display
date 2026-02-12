# Local Authority Districts Map

This project is designed to demonstrate modern software engineering practices, focusing on geographical data visualization, clean architecture, and interactive UI components.

---

## 📍 Project Purpose

The goal of this application is to visualize **Local Authority Districts** in England using an interactive web map.

---

## 💡 Features

- 🗺️ Displays all Local Authority Districts on an interactive map at initial load.
- 🔍 Allows users to **filter specific areas** by name (e.g., "Oxford", "Westminster").
- 👁️ Supports toggling to hide all displayed data.

---

## 🏗️ Tech Stack

| Layer                            | Technology                             |
|----------------------------------|----------------------------------------|
| Hybrid Rendering                 |                                        |
| └─ Client Rendering Components   | Blazor (WebAssembly)                   |
| └─ Backend Rendering Components  | Blazor (Server rendering)              |
| Backend                          | .NET Core 8 (C#)                       |
| Database                         | SQLite                                 |
| Mapping                          | Azure Maps                             |
| Additional                       | JavaScript (for interop where needed)  |

---

## ⚙️ Architecture Overview

- **Blazor Hybrid** is used to deliver a rich, interactive UI.
- **.NET Core 8 Web API** serves spatial and filtered data to the frontend.
- **SQLite** stores and processes the Local Authority District dataset.
- **Azure Maps with Turf.js** handles the rendering of spatial boundaries.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022+ or VS Code
- SQLite CLI / DB Browser(SQLite) [optional for inspection]

### Installation


1. Clone this repository:

```bash
git clone https://github.com/beneki/maps-display.git
cd maps-display
```

2. Before everything you need to create required Azure services (like Azure Maps and ...) and set secrets.json (right click on MapsDisplay solution -> Mangae User Secrets) of your app as follows:

```bash
{
  "Authentication": {
    "Microsoft": {
      "Instance": "https://login.microsoftonline.com",
      "Domain": "YOUR_AZURE_DOMAIN_FOR_REGISTERED_APP",
      "ClientId": "YOUR_CLIENT_ID_FOR_REGISTERED_APP",
      "ClientSecret": "YOUR_CLIENT_SECRET_FOR_REGISTERED_APP",
      "TenantId": "YOUR_TENANT_ID_FOR_REGISTERED_APP",
      "CallbackPath": "/signin-microsoft",
      "AuthorizationEndpoint": "https://login.microsoftonline.com/YOUR_TENANT_ID_FOR_REGISTERED_APP/oauth2/v2.0/authorize",
      "TokenEndpoint": "https://login.microsoftonline.com/YOUR_TENANT_ID_FOR_REGISTERED_APP/oauth2/v2.0/token"
    }
  },
  "Maps": {
    "SubscriptionKey": "YOUR_AZURE_MAPS_SUBSCRIPTION_ID"
  }
}
```

Now the app's ready to use
