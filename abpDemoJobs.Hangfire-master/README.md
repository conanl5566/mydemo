# AbpDemoJobs.Hangfire
Abp VNext BackgroundJobs 后台服务 Demo

## 数据库
### MySql,Mariadb

## docker 
```
docker run --name fangfire-mariadb -p 3306:3306 -e MYSQL_ROOT_PASSWORD=1234567 -d mariadb
docker exec -it xx bash
```

## 文档说明
https://docs.abp.io/zh-Hans/abp/latest/Background-Jobs

## 初始化数据库（mysql)
```
mysql -uroot -p1234567
create database DemoJobsHangfire character set utf8;
```
## 运行
```
dotnet restore
dotnet run .
```
