package main

import (
	"log"
	"os"
	"strconv"

	"github.com/hetacode/fifalaki/words/db"
	"github.com/hetacode/fifalaki/words/services"
	"github.com/joho/godotenv"
	"github.com/robfig/cron/v3"
)

func main() {
	godotenv.Load()

	done := make(chan os.Signal)

	dbNumber, err := strconv.Atoi(os.Getenv("REDIS_SERVER_DB"))
	if err != nil {
		panic(err)
	}

	db := db.NewRedisDB(os.Getenv("REDIS_SERVER"), dbNumber)

	// Process words on startup
	words := &services.WordsProcessorService{
		DB: db,
	}
	go words.Processing()
	c := cron.New()
	c.AddFunc("@daily", words.Processing)
	c.Start()

	log.Printf("Words service processor is running")
	<-done
	// TODO
	// Read side
	// 1. get 10 random keys
	// 2. create 4 length list of unique words from above keys
	// 3. send via grpc
}
