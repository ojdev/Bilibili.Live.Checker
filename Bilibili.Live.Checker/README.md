# wxpusher�й�ע�˵��û���id��ȡ

[!wxpusher�й�ע�˵��û���id��ȡ](Bilibili.Live.Checker/wxpusher�й�ע�˵��û���id.jpg)


# �����ļ�
appsetting.json


```json
{
  "wxpusher": {
    "APPTOKEN": "wxpusher��apptoken", /
    "users": [
      {
        "uid": "wxpusher�й�ע�˵��û���id<https://wxpusher.zjiecode.com/admin/main/wxuser/list>",
        "name": "�û��ǳ�/��ע",
        "DNDPeriod": [
          "00:00:00-11:30:00",
          "13:00:00-18:00:00" //�����ʱ���
        ],
        "bilibili": {
          "uids": [
            "3493137045523067",
            "Bվ�û�UID"
          ]
        }
      }
    ]
  }
}
```

# docker

## windows
`docker run -v /E/biliweixin/Bilibili.Live.Checker-master/Bilibili.Live.Checker/appsetting.json:/app/appsetting.json -e TZ=Asia/Shanghai -d --name bilivechk luacloud/bilibili.live.checker:latest`

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