!#/bin/bash

git clone --recursive https://github.com/CityOfZion/neo-sharp.git

cd sdk_container
docker build -t test:0.1 .
