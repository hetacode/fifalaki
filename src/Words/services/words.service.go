package services

import (
	"context"
	"log"
	"math/rand"
	"sort"

	"github.com/hetacode/fifalaki/words/db"
	proto "github.com/hetacode/fifalaki/words/gen/ProtoContracts"
)

// WordsServices struct
type WordsServices struct {
	DB *db.RedisKeyValueDB
}

// GetWordsPackage prepare 4 unique words from redis db and put them to result
func (s *WordsServices) GetWordsPackage(ctx context.Context, req *proto.GetWordsPackageReq) (*proto.GetWordsPackageRes, error) {
	words := make([]string, 40)
	n := 0
	// fetch 40 random words
	for n < 40 {
		k, v, err := s.DB.GetRandom()
		if err != nil {
			return nil, err
		}
		log.Printf("get random words '%s' with '%s' value", k, v)
		words[n] = k
		n++
	}

	// take 4 unique words
	sort.Strings(words)
	j := 0
	i := 0
	for i < len(words)-1 {
		i++
		if words[j] == words[i] {
			continue
		}
		j++
		words[j] = words[i]
	}
	withoutDuplicates := words[:j+1]
	selectedWords := make([]string, 4)
	n = 0
	for n < 4 {
		i := rand.Intn(len(withoutDuplicates))
		selectedWords[n] = withoutDuplicates[i]
		withoutDuplicates = withoutDuplicates[:len(withoutDuplicates)-1]
		n++
	}
	res := &proto.GetWordsPackageRes{
		Words: selectedWords,
	}
	return res, nil
}
