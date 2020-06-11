package main

import (
	"fmt"
	"log"
	"net"
	"os"
	"strconv"

	"github.com/hetacode/fifalaki/words/db"
	proto "github.com/hetacode/fifalaki/words/gen/ProtoContracts"
	"github.com/hetacode/fifalaki/words/services"
	"github.com/joho/godotenv"
	"github.com/robfig/cron/v3"
	"google.golang.org/grpc"
)

func main() {
	godotenv.Load()

	dbNumber, err := strconv.Atoi(os.Getenv("REDIS_SERVER_DB"))
	if err != nil {
		panic(err)
	}

	db := db.NewRedisDB(os.Getenv("REDIS_SERVER"), dbNumber)

	// Process words on startup
	words := &services.WordsProcessorService{
		DB: db,
	}
	grpcService := &services.WordsServices{
		DB: db,
	}
	go words.Processing()
	c := cron.New()
	c.AddFunc("@daily", words.Processing)
	c.Start()

	lis, err := net.Listen("tcp", fmt.Sprintf(":%s", os.Getenv("PORT")))
	srv := grpc.NewServer()
	proto.RegisterWordsServiceServer(srv, grpcService)

	log.Printf("Words service processor is running on %s port", os.Getenv("PORT"))
	srv.Serve(lis)
}
