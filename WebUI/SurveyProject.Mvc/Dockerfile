FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebUI/SurveyProject.Mvc/SurveyProject.Mvc.csproj", "WebUI/SurveyProject.Mvc/"]
COPY ["Infrastructure/SurveyProject.Infrastructure/SurveyProject.Infrastructure.csproj", "Infrastructure/SurveyProject.Infrastructure/"]
COPY ["Domain/SurveyProject.Entities/SurveyProject.Entities.csproj", "Domain/SurveyProject.Entities/"]
COPY ["Core/SurveyProject.Core/SurveyProject.Core.csproj", "Core/SurveyProject.Core/"]
COPY ["Application/SurveyProject.Services/SurveyProject.Services.csproj", "Application/SurveyProject.Services/"]
COPY ["Application/SurveyProject.DataTransferObjects/SurveyProject.DataTransferObjects.csproj", "Application/SurveyProject.DataTransferObjects/"]
RUN dotnet restore "WebUI/SurveyProject.Mvc/SurveyProject.Mvc.csproj"
COPY . .
WORKDIR "/src/WebUI/SurveyProject.Mvc"
RUN dotnet build "SurveyProject.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SurveyProject.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyProject.Mvc.dll"]
