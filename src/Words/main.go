package main

import (
	"log"
	"os"

	"github.com/hetacode/fifalaki/words/services"
	"github.com/robfig/cron/v3"
)

func main() {
	// TODO:

	done := make(chan os.Signal)
	// Process words on startup
	words := new(services.WordsProcessorService)
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
