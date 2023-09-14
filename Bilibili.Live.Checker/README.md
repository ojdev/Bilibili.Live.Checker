# �����ļ�
appsetting.json
```json
{
  "wxpusher": {
    "APPTOKEN": "wxpusher��apptoken",
    "users": [
      {
        "uid": "wxpusher�й�ע�˵��û���id<https://wxpusher.zjiecode.com/admin/main/wxuser/list>",
        "name": "�û��ǳ�/��ע",
        "DNDPeriod": [
          "00:00:00-08:00:00" //�����ʱ�Σ��������ö��
        ],
        "bilibili": {
          "uids": [
            ""//Bվ�û�UID
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