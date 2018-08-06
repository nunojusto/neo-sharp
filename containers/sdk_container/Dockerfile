FROM microsoft/dotnet:latest

#RUN apt-get update && \
#    apt-get install -y libgflags-dev libsnappy-dev zlib1g-dev libbz2-dev liblz4-dev libzstd-dev && \
#    rm -rf /var/lib/apt/lists/* && \
RUN    mkdir /repo && \
    mkdir /neo-sharp-builds && \
    cd /repo && \
    git clone --recursive https://github.com/CityOfZion/neo-sharp.git  
#    cd neo-sharp/src/NeoSharp.Application && \
#    dotnet publish --configuration Release --output /neo-sharp-builds

VOLUME neo-sharp-builds

WORKDIR /repo/neo-sharp/src/NeoSharp.Application

ENTRYPOINT ["/usr/bin/dotnet", "publish", "--configuration", "Release", "--output", "/neo-sharp-builds"]
