#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=development
WORKDIR /src
COPY ["BeanFastApi/BeanFastApi.csproj", "BeanFastApi/"]
COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
COPY ["DataTransferObjects/DataTransferObjects.csproj", "DataTransferObjects/"]
COPY ["Utility/Utilities.csproj", "Utility/"]
RUN dotnet restore "./././BeanFastApi/BeanFastApi.csproj"
COPY . .
WORKDIR "/src/BeanFastApi/."
RUN dotnet build "./BeanFastApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=development
RUN dotnet publish "./BeanFastApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish   
# FROM build AS publish
# ARG BUILD_CONFIGURATION=development
# RUN dotnet watch run 
#"./BeanFastApi/BeanFastApi.csproj"
#CMD ["cd BeanFastApi", "dotnet watch run"]
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeanFastApi.dll"]