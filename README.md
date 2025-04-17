# Local Authority Districts Map — Technical Test

This is a technical test project designed to demonstrate modern software engineering practices, focusing on geographical data visualization, clean architecture, and interactive UI components.

---

## 📍 Project Purpose

The goal of this application is to visualize **Local Authority Districts** in England using an interactive web map. This project is designed as part of a technical assessment to reflect real-world coding standards, practices, and problem-solving.

---

## 💡 Features

- 🗺️ Displays all Local Authority Districts on an interactive map at initial load.
- 🔍 Allows users to **filter specific areas** by name (e.g., "Oxford", "Westminster").
- 🙈 Supports toggling to **hide all displayed data**.
- ⚡ Smooth user experience through a modern frontend and optimized backend.

---

## 🏗️ Tech Stack

| Layer          | Technology                             |
|----------------|----------------------------------------|
| Frontend       | Blazor (WebAssembly + Server)          |
| Backend        | Blazor (Server) + .NET Core 8 (C#)     |
| Database       | SQLite                                 |
| Mapping        | Azure Maps                             |
| Additional     | JavaScript (for interop where needed)  |

---

## ⚙️ Architecture Overview

- **Blazor Hybrid** is used to deliver a rich, interactive UI.
- **.NET Core 8 Web API** serves spatial and filtered data to the frontend.
- **SQLite** stores and processes the Local Authority District dataset.
- **Mapbox** handles the rendering of spatial boundaries.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022+ or VS Code
- SQLite CLI / DB Browser (optional for inspection)
- Git

### Installation

1. Clone this repository:

```bash
git clone https://github.com/yourusername/your-repo.git
cd your-repo
```

2. Go to go to Features/LocalAuthority/Data/Datasets path
open power shell (on windows) or terminal (on any other os) and make sure have admin role
and run:

```bash
Pull docker image: metacollin/tippecanoe
```

3. Then run the container interactively:

 ```bash
docker run -it --rm -v "${PWD}:/data" metacollin/tippecanoe /bin/bash
```

3. Run Tippecanoe Inside the Container (to generate .mbtile file from your jeojson datasets):

 ```bash
 tippecanoe --no-tile-compression --include=name -l local_authorities_layer -o /data/local_authorities.mbtiles /data/local_authority_district.geojson
```

4. Now the app's ready to use (when you run the app lookup.json file will automatically be generated for you if the file does not exist)