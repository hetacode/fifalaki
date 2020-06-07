package services

import (
	"log"
	"time"
)

// WordsProcessorService structure
type WordsProcessorService struct {
}

// Processing write side logic
// 1. download words list from https://sjp.pl/slownik/growy/ in cron scheduler - on start and then 24h
// 2. unpack zip
// 3. process words - take only 5-7 chars words
// 4. put them to redis
func (s *WordsProcessorService) Processing() {
	log.Printf("Words processing is started on %s", time.Now().Format("Mon Jan _2 15:04:05 2006"))
}
