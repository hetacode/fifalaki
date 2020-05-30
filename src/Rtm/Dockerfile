FROM node:14-alpine as build
WORKDIR /build
COPY Rtm/package.json package.json
RUN npm install
COPY Rtm/. .
RUN npm run build

FROM node:14-alpine
WORKDIR /app
COPY --from=build /build/node_modules /app/node_modules
COPY --from=build /build/out /app
CMD ["node", "index.js"]