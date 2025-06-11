yes | sudo docker compose down
yes | sudo docker compose build reverse-proxy
yes | sudo docker compose build websocket-skyblock
yes | sudo docker compose build discord-bot
yes | sudo docker compose build website
yes | sudo docker compose up --detach

