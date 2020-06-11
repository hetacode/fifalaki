FROM golang:1.14-alpine as build
WORKDIR /build
COPY . .
RUN go build main.go

FROM alpine:3.12
EXPOSE 5003
COPY --from=build /build/main /app/
WORKDIR /app
CMD ["./main"]