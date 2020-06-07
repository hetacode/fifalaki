FROM node:14-alpine as build
WORKDIR /build
COPY web/package.json package.json
RUN npm install
COPY web/. .
RUN npm run build

FROM node:14-alpine
WORKDIR /app
COPY --from=build /build/build /app
RUN npm install -g serve
CMD ["serve", "-l", "3000"]