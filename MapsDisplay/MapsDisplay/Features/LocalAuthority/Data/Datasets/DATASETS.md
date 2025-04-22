### Using Custom GeoJSON Files 

If you want to use your own GeoJSON files instead of the default ones, follow these steps to generate a corresponding `.mbtiles` and `.json` files:

---

### ⚠️ **Prerequisites**

Before starting, make sure you have **Docker** installed on your machine. You can download and install Docker from the official website: [Docker Installation](https://www.docker.com/get-started).

---

### 🧰 **1. Pull the Tippecanoe Docker Image**

Navigate to:

```
Features/LocalAuthority/Data/Datasets
```

Open **PowerShell** or **Terminal** with administrator privileges,
and pull the Tippecanoe image:

```bash
docker pull metacollin/tippecanoe
```

---

### 🚢 **2. Run the Tippecanoe Container**

Run the Docker container interactively and mount the current directory:

```bash
docker run -it --rm -v "${PWD}:/data" metacollin/tippecanoe /bin/bash
```

---

### 🗺️ **3. Generate the `.mbtiles` File**

Inside the running container, use Tippecanoe to convert your GeoJSON dataset into an `.mbtiles` file:

```bash
tippecanoe --no-tile-compression --include=name -l local_authorities_layer -o /data/YOUR_GEOJSON_FILE_NAME.mbtiles /data/YOUR_GEOJSON_FILE_NAME.geojson
```

This will create your new `YOUR_GEOJSON_FILE_NAME.mbtiles` in the `/Datasets` directory.

---

### 🗺️ **4. Update the `.mbtiles` File Name**

Open:

```
Features/LocalAuthority/Controllers/TilesController.cs
```

and change:

```csharp
const string MBTILE_FILE_NAME = "local_authorities.mbtiles";
```

to:

```csharp
const string MBTILE_FILE_NAME = "YOUR_GEOJSON_FILE_NAME.mbtiles";
```

---

### 🧑‍💻 **5. Update `LookupGenerator.cs`**

Open:

```
Features/LocalAuthority/Data/Datasets/LookupGenerator.cs
```

and change:

```csharp
const string GEOJSON_FILE_NAME = "local_authority_district.geojson";
```

to:

```csharp
const string GEOJSON_FILE_NAME = "YOUR_GEOJSON_FILE_NAME.geojson";
```


---

### ⚡ **5. Lookup File Auto-Generation**

When you run the app, `YOUR_GEOJSON_FILE_NAME.json` will be generated automatically if doesn't exist.

💡 **Note:**  
The `lookup.json` file is a precomputed map of district names to their coordinates, extracted from the `GeoJSON` file. This significantly improves response times when handling client requests — for example, when locating a district based on a name search on the map.