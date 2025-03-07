FROM node:23.9-alpine3.21 AS stage1
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build && npm prune --production

FROM node:23.9-alpine3.21
USER 1000:1000
WORKDIR /app
COPY --from=stage1 --chown=1000:1000 /app/build ./build
COPY --from=stage1 --chown=1000:1000 /app/node_modules ./node_modules
COPY package.json .
ENV PORT 5050
EXPOSE 5050
CMD ["node","build"]