# wxpusher中关注了的用户的id获取

[wxpusher中关注了的用户的id获取](Bilibili.Live.Checker/wxpusher中关注了的用户的id.jpg)


# 配置文件
appsetting.json


```json
{
  "wxpusher": {
    "APPTOKEN": "wxpusher的apptoken", /
    "users": [
      {
        "uid": "wxpusher中关注了的用户的id<https://wxpusher.zjiecode.com/admin/main/wxuser/list>",
        "name": "用户昵称/备注",
        "DNDPeriod": [
          "00:00:00-11:30:00",
          "13:00:00-18:00:00" //免打扰时间段
        ],
        "bilibili": {
          "uids": [
            "3493137045523067",
            "B站用户UID"
          ]
        }
      }
    ]
  }
}
```

# docker

## windows

E:\biliweixin\appsetting.json

`docker run -v /E/biliweixin/appsetting.json:/app/appsetting.json -e TZ=Asia/Shanghai -d --name bilivechk luacloud/bilibili.live.checker:latest`

## linux 

`docker run -v /docker-config/bilivechk/appsetting.json:/app/appsetting.json -e TZ=Asia/Shanghai -d --name bilivechk luacloud/bilibili.live.checker:latest`

# docker-compose

```yaml
version: "3.3"
services:
  bilivechk:
    image: luacloud/bilibili.live.checker:latest
    container_name: bilivechk
    environment:
      - TZ=Asia/Shanghai
    volumes:
      - /docker-config/bilivechk/appsetting.json:/app/appsetting.json
    restart: always
```