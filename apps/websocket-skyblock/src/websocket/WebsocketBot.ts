import express from "express";
import { Server } from "http";
import { WebSocket, WebSocketServer } from "ws";

export class WebsocketBot {
    app: express.Express;
    server: Server;
    wss: WebSocketServer;

    constructor() {
        this.app = express();
        const port = process.env.PORT || 3000;

        this.server = this.app.listen(port, () => {
            console.log(`Listening on port ${port}.`);
        });

        this.wss = new WebSocketServer({ noServer: true });
    }

    Load() {
        function preError(e: Error) {
            console.log("error while upgrading websocket", e);
        }

        function postError(e: Error) {
            console.log("error while using websocket", e);
        }

        this.wss.on("connection", (ws, req) => {});

        this.server.on("upgrade", (req, socket, head) => {
            socket.addListener("error", preError);

            const strings = req.headers.cookie?.split(/[=;]/);

            if (!strings?.length) {
                socket.write("HTTP/1.1 401 Unauthorized\r\n\r\n");
                socket.destroy();
                return;
            }

            const cookie = {} as Record<string, any>;
            for (let i = 0; i < strings.length; i++) {
                const item = strings[i].trim();

                if (i % 2 === 0) {
                    cookie[item] = undefined;
                } else {
                    cookie[strings[i - 1].trim()] = item;
                }
            }

            console.log(cookie);
            // TODO: implement

            const isAuthorized = true;
            // cookie-auth
            if (!isAuthorized) {
                socket.write("HTTP/1.1 401 Unauthorized\r\n\r\n");
                socket.destroy();
                return;
            }

            this.wss.handleUpgrade(req, socket, head, (ws, req) => {
                socket.removeListener("error", preError);
                this.wss.emit("connection", ws, req);
            });
        });

        this.wss.on("connection", (ws, req) => {
            console.log("new connection");
            ws.on("error", postError);
            ws.on("message", (data, isBinary) => {
                console.log("message");

                this.wss.clients.forEach((client) => {
                    if (client.readyState === WebSocket.OPEN) {
                        client.send(data, { binary: isBinary });
                    }
                });
            });
        });
    }

    SendUpdates() {}

    CheckClients() {}
}
