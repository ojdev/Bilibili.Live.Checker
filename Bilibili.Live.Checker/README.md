# 配置文件
appsetting.json
```json
{
  "wxpusher": {
    "APPTOKEN": "wxpusher的apptoken",
    "users": [
      {
        "uid": "wxpusher中关注了的用户的id<https://wxpusher.zjiecode.com/admin/main/wxuser/list>",
        "name": "用户昵称/备注",
        "DNDPeriod": [
          "00:00:00-08:00:00" //免打扰时段，可以设置多个
        ],
        "bilibili": {
          "uids": [
            ""//B站用户UID
          ]
        }
      }
    ]
  }
}
```

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