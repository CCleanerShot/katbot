yes | sudo docker compose down --rmi all
yes | sudo docker builder prune
yes | sudo docker image prune
yes | sudo docker compose build reverse-proxy
yes | sudo docker compose build websocket-skyblock
yes | sudo docker compose build discord-bot
yes | sudo docker compose build website
yes | sudo docker compose up


