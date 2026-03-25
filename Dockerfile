FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# Avalonia needs these for Linux GUI
RUN apt-get update && apt-get install -y \
    libice6 libsm6 libx11-6 libxext6 libxrender1 \
    libfontconfig1 libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["./HomeAutomation"]
