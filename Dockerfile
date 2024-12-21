# dotnet publish -c Release -o out
# docker run -d -p 8080:80 --name xthang xthang
# docker build -t xthang .
# Chọn base image cho .NET 8
# mcr.microsoft.com/dotnet/aspnet:6.0 AS base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Copy ứng dụng đã build từ máy local vào Docker container
COPY out/ /app/

# Đặt lệnh khởi động khi container chạy
ENTRYPOINT ["dotnet", "do_an.dll"]
