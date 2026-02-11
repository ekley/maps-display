1. Install Docker (if you don't have it)

2. Pull docker image: metacollin/tippecanoe

3. Run the container interactively
docker run -it --rm -v "${PWD}:/data" metacollin/tippecanoe /bin/bash

4. Run Tippecanoe Inside the Container:
        tippecanoe --no-tile-compression --include=name -l local_authorities_layer -o /data/local_authorities.mbtiles /data/local_authority_district.geojson