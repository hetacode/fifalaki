package main

func main() {
	// TODO:
	// Write side
	// 1. download words list from https://sjp.pl/slownik/growy/ in cron scheduler - on start and then 24h
	// 2. unpack zip
	// 3. process words - take only 5-7 chars words
	// 4. put them to redis

	// TODO
	// Read side
	// 1. get 10 random keys
	// 2. create 4 length list of unique words from above keys
	// 3. send via grpc
}
