FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN mkdir -p app 
COPY app /app

WORKDIR /app

CMD ["dotnet", "Consumer.dll"]
EXPOSE 5000

